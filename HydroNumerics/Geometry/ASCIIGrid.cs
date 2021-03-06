﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics.LinearAlgebra.Double;


namespace HydroNumerics.Geometry
{

  public class ASCIIGrid:BaseGrid
  {
    public DenseMatrix Data {get;set;}
    public double DeleteValue { get; set; }
        
    public ASCIIGrid()
    {
      OriginAtCenter = false;
      Orientation = 0;
    }


    public void LoadPartial(string FileName, XYPolygon Area)
    {

      using (StreamReader sr = new StreamReader(FileName))
      {
        string line = sr.ReadLine();
        NumberOfColumns = int.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        NumberOfRows = int.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        XOrigin = double.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        YOrigin = double.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        GridSize = double.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        DeleteValue = double.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);


        double MinX = Area.Points.Min(x => x.X);
        double MaxX = Area.Points.Max(x => x.X);
        double MaxY = Area.Points.Max(p => p.Y);
        double MinY = Area.Points.Min(p => p.Y);



      }

    }

    public void Load(string FileName)
    {
      using (StreamReader sr = new StreamReader(FileName))
      {
        string line = sr.ReadLine();
        NumberOfColumns = int.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        NumberOfRows = int.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        XOrigin = double.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        YOrigin = double.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        GridSize = double.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        line = sr.ReadLine();
        DeleteValue = double.Parse(line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);

        Data = new DenseMatrix(NumberOfRows, NumberOfColumns);


        string[] DataRead;
        for (int j = 0; j < NumberOfRows; j++)
        {
          DataRead = sr.ReadLine().Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
          for (int i = 0; i < NumberOfColumns; i++)
          {
            Data[NumberOfRows - j - 1, i] = double.Parse(DataRead[i]);
          }
        }
      }
    }

    public void Save(string FileName)
    {
      using (StreamWriter sw = new StreamWriter(FileName, false, Encoding.Default))
      {
        sw.Write(this.ToString());
      }
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("NCOLS " + NumberOfColumns);
      sb.AppendLine("NROWS " + NumberOfRows);
      if (OriginAtCenter)
      {
        sb.AppendLine("XLLCENTER " + XOrigin);
        sb.AppendLine("YLLCENTER " + YOrigin);
      }
      else
      {
        sb.AppendLine("XLLCORNER " + XOrigin);
        sb.AppendLine("YLLCORNER " + YOrigin);
      }
      sb.AppendLine("CELLSIZE " + GridSize);
      sb.AppendLine("NODATA_VALUE " + DeleteValue);


      for (int j = Data.RowCount - 1; j >= 0; j--)
      {
          for (int i = 0; i < Data.ColumnCount; i++)
            sb.Append(Data[j, i].ToString() + " ");
        sb.AppendLine();
      }
      return sb.ToString();
    }
  }
}
