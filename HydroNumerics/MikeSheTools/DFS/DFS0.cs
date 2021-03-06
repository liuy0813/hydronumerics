﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DHI.Generic.MikeZero.DFS;

using MathNet.Numerics.LinearAlgebra.Double;


namespace HydroNumerics.MikeSheTools.DFS
{
  public class DFS0 : DFSBase
  {

    private SortedList<DateTime, DenseVector> Data { get; set; }
    private bool DataRead = false;

    private Dictionary<DateTime, int> DuplicateDataEntries;

    public DFS0(string DFSFileName)
      : base(DFSFileName)
    {
      Data = new SortedList<DateTime, DenseVector>();

      int k = 0;
      //Apparently it is possible to create dfs0-files with duplicate time entries. Also there could be a potential problem with non-sorted data
      foreach (var t in _timesteps)
      {
        if (!Data.ContainsKey(t))
          Data.Add(t, new DenseVector(NumberOfItems, DeleteValue));
        else
        {
          if (DuplicateDataEntries == null)
            DuplicateDataEntries = new Dictionary<DateTime, int>();
          DuplicateDataEntries.Add(t, k);
        }
        k++;
      }      
    }

    public DFS0(string DFSFileName, int NumberOfItems)
      : base(DFSFileName, NumberOfItems)
    {
      //Create the header
      _headerPointer = DfsDLLWrapper.dfsHeaderCreate(FileType.NeqtimeFixedspaceAllitems, "Title", "HydroNumerics", 1, NumberOfItems, StatType.RegularStat);
      _timeAxis = TimeAxisType.CalendarNonEquidistant;
      _spaceAxis = SpaceAxisType.EqD0;

      Data = new SortedList<DateTime, DenseVector>();

    }

    /// <summary>
    /// Gets the value for the Time step and Item
    /// TimeStep counts from zero, Item counts from 1
    /// </summary>
    /// <param name="TimeStep"></param>
    /// <param name="Item"></param>
    /// <returns></returns>
    public double GetData(int TimeStep, int Item)
    {
      if (!DataRead)
        ReadData();

      return Data.Values[TimeStep][Item - 1];
    }


    /// <summary>
    /// Gets the value for the Time step and Item
    /// </summary>
    /// <param name="TimeStep"></param>
    /// <param name="Item"></param>
    /// <returns></returns>
    public double GetData(DateTime TimeStep, int Item)
    {
      if (!DataRead)
        ReadData();
      return Data[TimeStep][Item - 1];
    }

    /// <summary>
    /// Gets values for all items at particular time step
    /// </summary>
    /// <param name="TimeStep"></param>
    /// <returns></returns>
    public DenseVector GetData(DateTime TimeStep)
    {
      return Data[TimeStep];
    }

    /// <summary>
    /// Sets the value for the Time step and Item
    /// TimeStep counts from zero, Item counts from 1
    /// </summary>
    /// <param name="TimeStep"></param>
    /// <param name="Item"></param>
    /// <param name="Value"></param>
    public void SetData(int TimeStep, int Item, double Value)
    {
      IsDirty = true;
      if (!DataRead) 
        ReadData();

      Data.Values[TimeStep][Item-1] = Value;
    }

    public void SetData(DateTime Time, int Item, double Value)
    {
      IsDirty = true;
      if (!DataRead)
        ReadData();
      if (!Data.ContainsKey(Time))
        Data.Add(Time, new DenseVector(NumberOfItems, DeleteValue));
     
      Data[Time][Item - 1] = Value;

    }

    private bool IsDirty = false;

    public bool InsertTimeStep(DateTime Time)
    {
     // IsDirty = true;
      if (!DataRead)
        ReadData();

      if (Data.ContainsKey(Time))
        return false;
      else
      {
        Data.Add(Time, new DenseVector(NumberOfItems, DeleteValue));
        WriteTime();
        return true;
      }
    }

    protected override void AppendTimeStep(DateTime Time)
    {
      if (_timeAxis == TimeAxisType.CalendarNonEquidistant & Data.Count > 0)
      {
        InsertTimeStep(Time);
      }
    }

    public override IList<DateTime> TimeSteps
    {
      get
      {
        return Data.Keys;
      }
    }


    public override DateTime TimeOfFirstTimestep
    {
      get
      {
        return base.TimeOfFirstTimestep;
      }
      set
      {
        if (TimeSteps.Count > 0)
        {
          var temp = Data.First().Value;
          Data.RemoveAt(0);
          Data.Add(value, temp);
        }
        else
          InsertTimeStep(value);
        WriteTime();
      }
    }

    public override void CopyFromTemplate(DFSBase dfs)
    {
      base.CopyFromTemplate(dfs);
      InsertTimeStep(dfs.TimeOfFirstTimestep);
      IsDirty = false;
    }



    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (IsDirty)
        {
          WriteTime();
          int ntimesteps = NumberOfTimeSteps;
          for (int i = 0; i < ntimesteps; i++)
            for (int j = 0; j <NumberOfItems; j++)
              WriteItemTimeStep(i,j+1,new float[]{(float)Data.Values[i][j]});
        }
      }
      base.Dispose(disposing);
    }

    private void ReadData()
    {

      for (int i = 0; i < NumberOfTimeSteps; i++)
      {
        for (int j = 0; j < NumberOfItems; j++)
        {
          var dfsdata = ReadItemTimeStep(i, j + 1);
          Data.Values[i][j] = dfsdata[0];
        }

      }
      DataRead = true;

    }

  }
}
