﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics.LinearAlgebra.Double;

using HydroNumerics.MikeSheTools.DFS;
using HydroNumerics.Geometry;

namespace HydroNumerics.MikeSheTools.Core
{
  public class MikeSheGridInfo:IDisposable,IEquatable<MikeSheGridInfo> 
  {
    private DFS3 _preproDFS3;
    private DFS2 _preproDFS2;

    private DataSetsFromDFS3 _lowerLevelOfComputationalLayers;
    private DataSetsFromDFS3 _thicknessOfComputationalLayers;
    private DataSetsFromDFS3 _boundaryConditionsForTheSaturatedZone;

    private DataSetsFromDFS2 _modelDomainAndGrid;
    private DataSetsFromDFS2 _surfaceTopography;
    private TopOfCell _upperLevelOfComputationalLayers;

    public double GridSize { get; private set; }
    public double XOrigin { get; private set; }
    public double YOrigin { get; private set; }
    private double _deleteValue;

    public MikeSheGridInfo(string PreProcessed3DFile, string PreProcessed2DFile)
    {
      _preproDFS3 = new DFS3(PreProcessed3DFile);
      _preproDFS2 =new DFS2(PreProcessed2DFile); 
 
      //Open Files and call initialize
      Initialize(_preproDFS3, _preproDFS2);
    }
    

    internal MikeSheGridInfo(DFS3 PreProcessed3D, DFS2 Preprocessed2D)
    {
      Initialize(PreProcessed3D, Preprocessed2D);
    }


    private void Initialize(DFS3 PreProcessed3D, DFS2 Preprocessed2D)
    {
      //Generate 3D properties
      if (PreProcessed3D != null)
      {
        for (int i = 0; i < PreProcessed3D.Items.Length; i++)
        {
          switch (PreProcessed3D.Items[i].Name)
          {
            case "Boundary conditions for the saturated zone":
              _boundaryConditionsForTheSaturatedZone = new DataSetsFromDFS3(PreProcessed3D, i + 1);
              break;
            case "Lower level of computational layers in the saturated zone":
              _lowerLevelOfComputationalLayers = new DataSetsFromDFS3(PreProcessed3D, i + 1);
              break;
            case "Thickness of computational layers in the saturated zone":
              _thicknessOfComputationalLayers = new DataSetsFromDFS3(PreProcessed3D, i + 1);
              break;
            default: //Unknown item
              break;
          }
        }
      }

      //Generate 2D properties by looping the items
      for (int i = 0; i < Preprocessed2D.Items.Length; i++)
      {
        switch (Preprocessed2D.Items[i].Name)
        {
          case "Model domain and grid":
            _modelDomainAndGrid = new DataSetsFromDFS2(Preprocessed2D, i + 1);
            break;
          case "Surface topography":
            _surfaceTopography = new DataSetsFromDFS2(Preprocessed2D, i + 1);
            break;
          default: //Unknown item
            break;
        }
      }

      _deleteValue = Preprocessed2D.DeleteValue;
      GridSize = Preprocessed2D.GridSize;

      NumberOfRows = Preprocessed2D.NumberOfRows; ;
      NumberOfColumns = Preprocessed2D.NumberOfColumns;
      if (PreProcessed3D == null)
        NumberOfLayers = 1;
      else
        NumberOfLayers = PreProcessed3D.NumberOfLayers;

      //For MikeShe the origin is lower left whereas it is center of lower left for DFS
      XOrigin = Preprocessed2D.XOrigin;
      YOrigin = Preprocessed2D.YOrigin;

    }

    /// <summary>
    /// Gets the Number of layers in the grid
    /// </summary>
    public int NumberOfLayers{get; private set;}

    /// <summary>
    /// Gets the Number of columns in the grid
    /// </summary>
    public int NumberOfColumns {get; private set;}
    
    /// <summary>
    /// Gets the Number of Rows in the grid
    /// </summary>
    public int NumberOfRows {get; private set;}

        /// <summary>
    /// Gets the Column index for this coordinate. Lower left is (0,0). 
    /// Returns -1 if UTMY is left of the grid and -2 if it is right.
    /// </summary>
    /// <param name="UTMY"></param>
    /// <returns></returns>
    public int GetColumnIndex(double UTMX)
    {
      double d = (UTMX - XOrigin) / GridSize;
      if (Math.IEEERemainder((UTMX - XOrigin), GridSize) == 0.0)
      {
        d--;
        d = Math.Max(0, d);
      }
      //Calculate as a double to prevent overflow errors when casting 
      double ColumnD = Math.Max(-1, Math.Floor(d));

      if (ColumnD > NumberOfColumns-1)
        return -2;
      return (int) ColumnD;
    }

    /// <summary>
    /// Gets the Row index for this coordinate. Lower left is (0,0). 
    /// Returns -1 if UTMY is below the grid and -2 if it is above.
    /// If a point is exactly between grid blocks the lower is chosen 
    /// </summary>
    /// <param name="UTMY"></param>
    /// <returns></returns>
    public int GetRowIndex(double UTMY)
    {
      double d = (UTMY - YOrigin) / GridSize;
      if (Math.IEEERemainder((UTMY - YOrigin), GridSize) == 0.0)
      {
        d--;
        d = Math.Max(0, d);
      }

      //Calculate as a double to prevent overflow errors when casting 
        double RowD = Math.Max(-1, Math.Floor(d));

      if (RowD > NumberOfRows-1)
        return -2;
      return (int)RowD;
    }


    /// <summary>
    /// Returns the indeces for a set of coordinates.
    /// Necessary to sent the output as par
    /// Returns true if the grid point is within the active domain.
    /// Note that Column and Row may have positive values and still the point is outside of the active domain
    /// Note this may return a different value than the DFS-file!!!
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    /// <param name="Column"></param>
    /// <param name="Row"></param>
    public bool TryGetIndex(double X, double Y, out int Column, out int Row)
    {
      Column = GetColumnIndex(X);
      Row = GetRowIndex(Y);
      if (Column < 0 | Row < 0)
        return false;
      return 1 == _modelDomainAndGrid.Data[Row, Column];
    }

    /// <summary>
    /// Returns the layer number. Lower layer is 0. 
    /// If -1 is returned Z is above the surface and if -2 is returned Z is below the bottom.
    /// Z is in meters above mean sea level
    /// </summary>
    /// <param name="Column"></param>
    /// <param name="Row"></param>
    /// <param name="Z"></param>
    /// <returns></returns>
    public int GetLayer(int Column, int Row, double Z)
    {
      if (Z > _surfaceTopography.Data[Row, Column])
        return -1;
      else if (Z < _lowerLevelOfComputationalLayers.Data[Row, Column, 0])
        return -2;
      else
      {
        int i = 0;
        while ( i < NumberOfLayers && Z > _lowerLevelOfComputationalLayers.Data[Row, Column, i])
          i++;
        return i - 1;
      }
    }

    /// <summary>
    /// Returns the layer number. Lower layer is 0. 
    /// If Depth is negative -1 is returned and if -2 is returned depth is below the bottom.
    /// If point is outside model domain -3 is returned.
    /// </summary>
    /// <param name="Column"></param>
    /// <param name="Row"></param>
    /// <param name="Depth"></param>
    /// <returns></returns>
    public int GetLayerFromDepth(double X, double Y, double Depth)
    {
      return GetLayerFromDepth(GetColumnIndex(X), GetRowIndex(Y), Depth);
    }


    /// <summary>
    /// Returns the layer number. Lower layer is 0. 
    /// If Depth is negative -1 is returned and if -2 is returned depth is below the bottom.
    /// If column or row is negative -3 is returned
    /// </summary>
    /// <param name="Column"></param>
    /// <param name="Row"></param>
    /// <param name="Depth"></param>
    /// <returns></returns>
    public int GetLayerFromDepth(int Column, int Row, double Depth)
    {
      if (Column < 0 || Row < 0)
        return -3;
      return GetLayer(Column, Row, _surfaceTopography.Data[Row, Column] - Depth);
    }

    /// <summary>
    /// Returns the X-coordinate of the left cell-boundary
    /// </summary>
    /// <param name="Column"></param>
    /// <returns></returns>
    public double GetX(int Column)
    {
      return XOrigin + GridSize * Column;
    }
    /// <summary>
    /// Returns the Y-coordinate of the lower cell-boundary
    /// </summary>
    /// <param name="Row"></param>
    /// <returns></returns>
    public double GetY(int Row)
    {
      return YOrigin + GridSize * Row;
    }
    /// <summary>
    /// Returns the X-coordinate of the cell-center
    /// </summary>
    /// <param name="Column"></param>
    /// <returns></returns>
    public double GetXCenter(int Column)
    {
      return GetX(Column) + 0.5 * GridSize;
    }
    /// <summary>
    /// Returns the Y-coordinate of the cell-center
    /// </summary>
    /// <param name="Row"></param>
    /// <returns></returns>
    public double GetYCenter(int Row)
    {
      return GetY(Row) + 0.5 * GridSize;
    }


    /// <summary>
    /// Interpolates the value in Matrix M to the point (UTMX, UTMY) using 4-point bilinear interpolation.
    /// If one or more of the four POINTS have delete values these POINTS are excluded and instead an inverse distance
    /// interpolation scheme is used. Boundary cells are also not included. 
    /// The out parameters count the number of delete values and boundary cells.
    /// If the cell in which the POINTS is located has a delete value the delete value is returned.
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    /// <param name="M"></param>
    /// <param name="DeleteValues"></param>
    /// <param name="BoundaryCells"></param>
    /// <returns></returns>
    public double Interpolate(double X, double Y, int Layer, Matrix M, out int DeleteValues, out int BoundaryCells)
    {
      BoundaryCells = 0;
      DeleteValues = 0;
      
      int column = GetColumnIndex(X);
      int row = GetRowIndex(Y);

      if (M[row, column] == _deleteValue)
        return _deleteValue;

      int columnLL = column;
      int rowLL = row;

      //Finds the coordinate of the lower left of the four cells closest to the point
      if (X < GetXCenter(column))
      {
        columnLL -= 1;
      }
      if (Y < GetYCenter(row))
      {
        rowLL -= 1;
      }

      double InterpolatedValue = 0;
      double dx1 = GetXCenter(columnLL) - X;
      double dy1 = GetYCenter(rowLL) - Y;
      double dx3 = GetXCenter(columnLL + 1) - X;
      double dy3 = GetYCenter(rowLL + 1) - Y;

      //Get the values of the four POINTS
      double P1 = ValueCheck(M,columnLL,rowLL, Layer, ref DeleteValues, ref BoundaryCells);
      double P2 = ValueCheck(M, columnLL, rowLL + 1, Layer, ref DeleteValues, ref BoundaryCells);
      double P3 = ValueCheck(M, columnLL + 1, rowLL + 1, Layer, ref DeleteValues, ref BoundaryCells);
      double P4 = ValueCheck(M, columnLL + 1, rowLL, Layer, ref DeleteValues, ref BoundaryCells);

      //Inverse distance
      if (P1 == _deleteValue | P2 == _deleteValue | P3 == _deleteValue | P4 == _deleteValue)
      {
        double distance = 0;
        if (P1 != _deleteValue)
        {
          double d1 = Math.Pow(Math.Pow(dx1, 2) + Math.Pow(dy1, 2), -1.5);
          distance += d1;
          InterpolatedValue += d1 * P1;
        }
        if (P2 != _deleteValue)
        {
          double d2 = Math.Pow(Math.Pow(dx1, 2) + Math.Pow(dy3, 2), -1.5);
          distance += d2;
          InterpolatedValue += d2 * P2;
        }
        if (P3 != _deleteValue)
        {
          double d3 = Math.Pow(Math.Pow(dx3, 2) + Math.Pow(dy3, 2), -1.5);
          distance += d3;
          InterpolatedValue += d3 * P3;
        }
        if (P4 != _deleteValue)
        {
          double d4 = Math.Pow(Math.Pow(dx3, 2) + Math.Pow(dy1, 2), -1.5);
          distance += d4;
          InterpolatedValue += d4 * P4;
        }
        if (distance != 0)
          InterpolatedValue = InterpolatedValue / distance;
        else
          return _deleteValue;
      }
      else
      {
        //Four-point bilinear interpolation
        P1 = P1 * dx3 * dy3;
        P2 = P2 * dx3 * (-dy1);
        P3 = P3 * (-dx1) * (-dy1);
        P4 = P4 * (-dx1) * dy3;
        InterpolatedValue = (P1 + P2 + P3 + P4) / Math.Pow(GridSize, 2);
      }

      return InterpolatedValue;
    }

    /// <summary>
    /// Returns true if the location is with the model area
    /// </summary>
    /// <param name="Location"></param>
    /// <returns></returns>
    public bool IsInModelArea(IXYPoint Location)
    {
      return IsInModelArea(Location.X, Location.Y);
    }

    /// <summary>
    /// Returns true if the location is with the model area
    /// </summary>
    /// <param name="Location"></param>
    /// <returns></returns>
    public bool IsInModelArea(double X, double Y)
    {
      int Column = GetColumnIndex(X);
      int Row = GetRowIndex(Y);
      if (Column < 0 | Row < 0)
        return false;
      return 1 == _modelDomainAndGrid.Data[Row, Column];
    }

    /// <summary>
    /// Returns the wells from the IEnumerable that are within the model area.
    /// Also sets the Column and Row if the Well is of type MikeSheWell.
    /// </summary>
    /// <param name="Wells"></param>
    /// <returns></returns>
    public IEnumerable<MikeSheWell> SelectByMikeSheModelArea(IEnumerable<MikeSheWell> Wells)
    {
      int Column;
      int Row;
      foreach (MikeSheWell W in Wells)
      {
        //Gets the index and sets the column and row
        if (TryGetIndex(W.X, W.Y, out Column, out Row))
        {
          W.Column = Column;
          W.Row = Row;
          yield return W;
        }
      }
    }

    /// <summary>
    /// Method used by interpolate routine
    /// </summary>
    /// <param name="M"></param>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="Layer"></param>
    /// <param name="DeleteValue"></param>
    /// <param name="BoundaryValue"></param>
    /// <returns></returns>
    private double ValueCheck(Matrix M, int column, int row, int Layer, ref int DeleteValue, ref int BoundaryValue)
    {
      double val = M[row, column];
      if (val == _deleteValue)
        DeleteValue++;
      if (_boundaryConditionsForTheSaturatedZone.Data[row, column, Layer] != 1)
      {
        BoundaryValue++;
        return _deleteValue;
      }
      return val;
    }


    public IXYDataSet ModelDomainAndGrid
    {
      get { return _modelDomainAndGrid; }
    }

    public IXYDataSet SurfaceTopography
    {
      get { return _surfaceTopography; }
    }

    public IXYZDataSet LowerLevelOfComputationalLayers
    {
      get { return _lowerLevelOfComputationalLayers; }
    }

    public IXYZDataSet ThicknessOfComputationalLayers
    {
      get { return _thicknessOfComputationalLayers; }
    }

    public IXYZDataSet BoundaryConditionsForTheSaturatedZone
    {
      get { return _boundaryConditionsForTheSaturatedZone; }
    }

    public IXYZDataSet UpperLevelOfComputationalLayers
    {
      get
      {
        if (_upperLevelOfComputationalLayers == null)
          _upperLevelOfComputationalLayers = new TopOfCell(LowerLevelOfComputationalLayers, SurfaceTopography);
        return _upperLevelOfComputationalLayers;
      }
    }



    #region IDisposable Members

    /// <summary>
    /// Disposes the underlying dfs-files if the object has been constructed directly from files
    /// </summary>
    public void Dispose()
    {
      //If the object has been constructed from files it is the owner of the dfs objects and
      //has to dispose them
      if (_preproDFS2 != null)
        _preproDFS2.Dispose();
      if (_preproDFS3 != null)
        _preproDFS3.Dispose();
    }

    #endregion

    #region IEquatable<MikeSheGridInfo> Members

    public bool Equals(MikeSheGridInfo other)
    {
      return NumberOfColumns == other.NumberOfColumns &&
        NumberOfRows == other.NumberOfRows &&
        NumberOfLayers == other.NumberOfLayers &&
        XOrigin == other.XOrigin &&
        YOrigin == other.YOrigin &&
        GridSize == other.GridSize;
    }

    #endregion
  }
}
