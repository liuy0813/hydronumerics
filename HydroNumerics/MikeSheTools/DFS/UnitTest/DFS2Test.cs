﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DHI.Generic.MikeZero;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using HydroNumerics.MikeSheTools.DFS;
using MathNet.Numerics.LinearAlgebra.Double;


namespace HydroNumerics.MikeSheTools.DFS.UnitTest
{
  [TestClass]
  public class DFS2Test
  {
    DFS2 _simpleDfs;


    [TestInitialize()]
    public void ConstructTest()
    {

      _simpleDfs = new DFS2(@"..\..\..\TestData\simpelmatrix.dfs2");
    }


    [TestMethod]
    public void CreateCompressedFromTemplate()
    {
      DFS2 dfsorg = new DFS2(@"..\..\..\TestData\Novomr1_inv_PreProcessed.DFS2");
      DFS2 dfs = new DFS2(@"..\..\..\TestData\Novomr1_inv_PreProcessed_rewritten.DFS2", dfsorg);

      for(int i =1;i<= dfsorg.NumberOfItems;i++)
        dfs.SetData(0, i, dfsorg.GetData(0, i));
      dfsorg.Dispose();
      dfs.Dispose();

      var f1 = new System.IO.FileInfo(@"..\..\..\TestData\Novomr1_inv_PreProcessed.DFS2");
      var f2 = new System.IO.FileInfo(@"..\..\..\TestData\Novomr1_inv_PreProcessed_rewritten.DFS2");

      Assert.AreEqual(f1.Length, f2.Length, 50);


    }


    [TestMethod]
    public void RescaleTest()
    {
      System.IO.File.Copy(@"..\..\..\TestData\Novomr1_inv_PreProcessed.DFS2", @"..\..\..\TestData\Novomr1_inv_PreProcessed_rescaled.DFS2",true);
      DFS2 dfs = new DFS2(@"..\..\..\TestData\Novomr1_inv_PreProcessed_rescaled.DFS2");
      dfs.GridSize = 5;
      dfs.XOrigin = 0;
      dfs.YOrigin = 0;
      dfs.Dispose();

      dfs = new DFS2(@"..\..\..\TestData\Novomr1_inv_PreProcessed_rescaled.DFS2");

      Assert.AreEqual(5, dfs.GridSize);
      Assert.AreEqual(0, dfs.XOrigin);
      Assert.AreEqual(0, dfs.YOrigin);

      dfs.Dispose();

    }


    [TestMethod]
    public void OpenTwiceTest()
    {
      DFS2 dfs = new DFS2(@"..\..\..\TestData\Novomr1_inv_PreProcessed.DFS2");

      List<DFS2> _files = new List<DFS2>();
      for (int i = 0; i < 100; i++)
      {
        _files.Add(new DFS2(@"..\..\..\TestData\Novomr1_inv_PreProcessed.DFS2"));
        Matrix M = _files[i].GetData(0, 1);
      }

      int k = 0;
      DFS2.MaxEntriesInBuffer = 5;

      for (int i = 1; i < dfs.Items.Count(); i++)
      {
        Matrix M = dfs.GetData(0, i);
      }

    }


    [TestMethod]
    public void CreateFile()
    {
      DFS2 df = new DFS2(@"..\..\..\TestData\test.dfs2", 1);
      df.NumberOfColumns = 5;
      df.NumberOfRows = 7;
      df.XOrigin = 9000;
      df.YOrigin = 6000;
      df.Orientation = 1;
      df.GridSize = 15;
      df.TimeOfFirstTimestep = new DateTime(2011, 10, 1, 23, 0, 0);
      var s = df.TimeOfFirstTimestep.ToString("yyyy-MM-dd");
      df.TimeStep = TimeSpan.FromHours(2);

      df.FirstItem.Name = "SGS Kriged dyn. corr.precip";
      df.FirstItem.EumItem = eumItem.eumIPrecipitationRate;
      df.FirstItem.EumUnit = eumUnit.eumUmillimeterPerDay;

      DenseMatrix m = new DenseMatrix(df.NumberOfRows, df.NumberOfColumns);
      m[3, 4] = 25;
      df.SetData(0,1,m);

      df.SetData(1, 1, m);
      m[3, 4] = 15;
      df.SetData(2, 1, m);
      df.Dispose();

      df = new DFS2(@"..\..\..\TestData\test.dfs2");

      Assert.AreEqual(eumItem.eumIPrecipitationRate, df.FirstItem.EumItem);
      Assert.AreEqual(eumUnit.eumUmillimeterPerDay, df.FirstItem.EumUnit);

      DenseMatrix m2 = df.GetData(1, 1);
      Assert.AreEqual(25, m2[3, 4]);


      DFS2 df2 = new DFS2(@"..\..\..\TestData\test2.dfs2", df);
      df2.SetData(0,1,m);

      Assert.AreEqual(eumItem.eumIPrecipitationRate, df2.FirstItem.EumItem);
      Assert.AreEqual(eumUnit.eumUmillimeterPerDay, df2.FirstItem.EumUnit);

      Assert.AreEqual(df.GridSize, df2.GridSize);
      Assert.AreEqual(df.NumberOfColumns, df2.NumberOfColumns);
           Assert.AreEqual(df.NumberOfRows, df2.NumberOfRows);
     Assert.AreEqual(df.Orientation, df2.Orientation);
     Assert.AreEqual(df.TimeOfFirstTimestep, df2.TimeOfFirstTimestep);
     Assert.AreEqual(df.TimeStep, df2.TimeStep);
     Assert.AreEqual(df.XOrigin, df2.XOrigin);
     Assert.AreEqual(df.YOrigin, df2.YOrigin);
     Assert.AreEqual(df.FirstItem.Name, df2.FirstItem.Name);
     Assert.AreEqual(df.Items.Count(), df2.Items.Count());
     Assert.AreEqual(df.DeleteValue, df2.DeleteValue);
    }


    [TestCleanup()]
    public void Destruct()
    {
      _simpleDfs.Dispose();
    }

    [TestMethod]
    public void GetDataTest1()
    {
      DenseMatrix M = _simpleDfs.GetData(0, 1);
      Assert.AreEqual(0, M[0, 0]);
      Assert.AreEqual(1, M[1, 0]);
      Assert.AreEqual(2, M[2, 0]);
      Assert.AreEqual(3, M[0, 1]);

      Assert.AreEqual(10, _simpleDfs.GetData(323, 125, 0, 1), 1e-5);

    }

    [TestMethod]
    public void CoordinateTest()
    {
      using (DFS2 dfs = new DFS2(@"..\..\..\TestData\Novomr1_inv_PreProcessed.DFS2"))
      {
        Assert.AreEqual(615000, dfs.XOrigin);
        Assert.AreEqual(6035000, dfs.YOrigin);
        Assert.AreEqual(500, dfs.GridSize);
      }
    }



    [TestMethod]
    public void WriteTest()
    {

      DFS2 outdata = new DFS2(@"..\..\..\TestData\simpelmatrixKopi.dfs2");

      DenseMatrix M = outdata.GetData(0, 1);
      DenseMatrix M2;

      M[2, 2] = 2000;

      for (int i = 0; i < 10; i++)
      {
        outdata.SetData(i + 8, 1, M);
        M2 = outdata.GetData(i + 8, 1);
        Assert.IsTrue(M.Equals(M2));
      }

      DateTime d = new DateTime(1950, 1, 1);

      string dd = d.ToShortDateString();
      outdata.TimeOfFirstTimestep = new DateTime(1950, 1, 1);
      outdata.TimeStep = new TimeSpan(20, 0, 0, 0);
      
      outdata.Dispose();

    }


    [TestMethod]
    [Ignore]
    public void GridEdit()
    {
      DFS2 dfs = new DFS2(@"C:\Jacob\OpenDA.DotNet_vs2008\mshe\Mshe_5x5\Initial Potential Head.dfs2");

      var m = dfs.GetData(0, 1);

      for (int i = 0; i < m.Data.Count(); i++)
      {
        m.Data[i] = -0.01 * i;
      }
      dfs.SetData(0,1,m);
      dfs.Dispose();


      

    }

  }
}
