﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using DHI.Generic.MikeZero.DFS;

using HydroNumerics.Geometry;

namespace HydroNumerics.MikeSheTools.DFS
{
  /// <summary>
  /// Provides read and write access to a .dfs2-file.
  /// Buffers the 25 latest accessed Matrices. This number can be regulated.
  /// Different instances of the same file will not give different entries in the static buffer.
  /// Matrices stored on Absolute Filename, Item and TimeStep. Writing will also alter the buffer.
  /// Not threadsafe.
  /// </summary>
  public class DFS2 : DFS2DBase
  {
    //Static list holding all Matrices read from dfs2-files
    private static Dictionary<string, Dictionary<int, Dictionary<int, CacheEntry>>> SuperCache = new Dictionary<string, Dictionary<int, Dictionary<int, CacheEntry>>>();
    private static LinkedList<CacheEntry> AccessList = new LinkedList<CacheEntry>();
    public static int MaxEntriesInBuffer = 25;

    private Object LockThis = new object();

    //DataBuffer. First on Item, then on timeStep. 
    private Dictionary<int, Dictionary<int, CacheEntry>> _bufferData;

    /// <summary>
    /// Creates a new DFS2 file from scratch
    /// </summary>
    /// <param name="FileName"></param>
    /// <param name="Title"></param>
    /// <param name="NumberOfItems"></param>
    public DFS2(string FileName, int NumberOfItems)
      : base(FileName, NumberOfItems)
    {
      _spaceAxis = SpaceAxisType.EqD2;
      BuildCache();
    }

    public DFS2(string FileName, int NumberOfItems, IGrid Grid)
      : base(FileName, NumberOfItems, Grid)
    {
      _spaceAxis = SpaceAxisType.EqD2;
      BuildCache();
    }


    public DFS2(string FileName, DFS2DBase TemplateDFS)
      : base(FileName,TemplateDFS)
    {
      _spaceAxis = SpaceAxisType.EqD2;
      BuildCache();
    }

    /// <summary>
    /// Opens an existing DFS2 file
    /// </summary>
    /// <param name="DFSFileName"></param>
    public DFS2(string DFSFileName)
      : base(DFSFileName)
    {
      BuildCache();
    }

    private void BuildCache()
    {
      if (!SuperCache.TryGetValue(AbsoluteFileName, out _bufferData))
      {
        _bufferData = new Dictionary<int, Dictionary<int, CacheEntry>>();
        SuperCache.Add(AbsoluteFileName, _bufferData);
      }
    }
    
    /// <summary>
    /// Sets the data for the timestep and item
    /// </summary>
    /// <param name="TimeStep"></param>
    /// <param name="Item"></param>
    /// <param name="Data"></param>
    public void SetData(int TimeStep, int Item, DenseMatrix Data)
    {
      lock (LockThis)
      {

        float[] fdata = new float[Data.ColumnCount * Data.RowCount];
        int m = 0;
        for (int i = 0; i < Data.RowCount; i++)
          for (int j = 0; j < Data.ColumnCount; j++)
          {
            fdata[m] = (float)Data[i, j];
            m++;
          }
        WriteItemTimeStep(TimeStep, Item, fdata);

        //Now add the new data to the buffer.
        Dictionary<int, CacheEntry> _timeValues;
        CacheEntry cen;
        if (!_bufferData.TryGetValue(Item, out _timeValues))
        {
          _timeValues = new Dictionary<int, CacheEntry>();
          _bufferData.Add(Item, _timeValues);
        }
        if (_timeValues.ContainsKey(TimeStep))
        {
          cen = _timeValues[TimeStep];
          cen.Data = Data;
          AccessList.Remove(cen);
        }
        else
        {
          cen = new CacheEntry(AbsoluteFileName, Item, TimeStep, Data);
          _timeValues.Add(TimeStep, cen);
          CheckBuffer();
        }
        AccessList.AddLast(cen);
      }
    }

    /// <summary>
    /// Removes the oldest Matrix from the dictionary if the Accesslist contains more than MaxNumberOfEntries
    /// </summary>
    private void CheckBuffer()
    {
      if (AccessList.Count > MaxEntriesInBuffer)
      {
        CacheEntry ToRemove = AccessList.First.Value;
        SuperCache[ToRemove.FileName][ToRemove.Item].Remove(ToRemove.TimeStep);
        AccessList.RemoveFirst();
      }
    }

    /// <summary>
    /// Returns a Matrix with the data for the TimeStep, Item
    /// TimeStep counts from 0, Item from 1.
    /// Lower left in Matrix is (0,0)
    /// </summary>
    /// <param name="TimeStep"></param>
    /// <param name="Item"></param>
    /// <param name="Layer"></param>
    /// <returns></returns>
    public DenseMatrix GetData(int TimeStep, int Item)
    {
      CacheEntry cen;

      Dictionary<int, CacheEntry> _timeValues;

      lock (LockThis)
      {
        if (!_bufferData.TryGetValue(Item, out _timeValues))
        {
          _timeValues = new Dictionary<int, CacheEntry>();
          _bufferData.Add(Item, _timeValues);
        }
        if (!_timeValues.TryGetValue(TimeStep, out cen))
        {
          var dfsdata = ReadItemTimeStep(TimeStep, Item);
          DenseMatrix _data = new DenseMatrix(_numberOfRows, _numberOfColumns);
          int m = 0;
          for (int i = 0; i < _numberOfRows; i++)
            for (int j = 0; j < _numberOfColumns; j++)
            {
              _data[i, j] = dfsdata[m];
              m++;
            }
          cen = new CacheEntry(AbsoluteFileName, Item, TimeStep, _data);
          _timeValues.Add(TimeStep, cen);
          CheckBuffer();
        }
        else
          AccessList.Remove(cen);
        AccessList.AddLast(cen);
      }
      return cen.Data;
    }

    /// <summary>
    /// Gets the data at coordinate set.
    /// Can be faster to use because it does not fill data into the matrix.
    /// This method does not use the databuffer.
    /// </summary>
    /// <param name="UTMX"></param>
    /// <param name="UTMY"></param>
    /// <param name="TimeStep"></param>
    /// <param name="Item"></param>
    /// <returns></returns>
    public double GetData(double UTMX, double UTMY, int TimeStep, int Item)
    {
      var dfsdata = ReadItemTimeStep(TimeStep, Item);
      return dfsdata[GetRowIndex(UTMY) * _numberOfColumns + GetColumnIndex(UTMX)];
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        //Clears the cache and eany entries in the access list
        SuperCache.Remove(AbsoluteFileName);
        foreach(var entry in AccessList.Where(var => var.FileName == AbsoluteFileName).ToArray())
          AccessList.Remove(entry);

        this._bufferData.Clear();
      }
      base.Dispose(disposing);
    }
  }
}

