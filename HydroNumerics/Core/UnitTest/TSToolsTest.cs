﻿using HydroNumerics.Core.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HydroNumerics.Nitrate.Model.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for TSToolsTest and is intended
    ///to contain all TSToolsTest Unit Tests
    ///</summary>
  [TestClass()]
  public class TSToolsTest
  {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion

    /// <summary>
    ///A test for GetTimeStep
    ///</summary>
    [TestMethod()]
    public void GetTimeStepTest()
    {
      DateTime Start = new DateTime(2000, 1, 1);

      Assert.IsTrue(TimeStepUnit.Second == TSTools.GetTimeStep(Start, new DateTime(2000, 1, 1, 0, 0, 1)));
      Assert.IsTrue(TimeStepUnit.Minute == TSTools.GetTimeStep(Start, new DateTime(2000, 1, 1, 0, 1, 0)));
      Assert.IsTrue(TimeStepUnit.Hour == TSTools.GetTimeStep(Start, new DateTime(2000, 1, 1, 1, 0, 0)));
      Assert.IsTrue(TimeStepUnit.Day == TSTools.GetTimeStep(Start, new DateTime(2000, 1, 2, 0, 0, 0)));
      Assert.IsTrue(TimeStepUnit.Month == TSTools.GetTimeStep(Start, new DateTime(2000, 2, 1, 0, 0, 0)));
      Assert.IsTrue(TimeStepUnit.Year == TSTools.GetTimeStep(Start, new DateTime(2001, 1, 1, 0, 0, 0)));

    }

    /// <summary>
    ///A test for ChangeZoomLevel
    ///</summary>
    [TestMethod()]
    public void ChangeZoomLevelTest()
    {
      FixedTimeStepSeries Data = new FixedTimeStepSeries() { TimeStepSize = TimeStepUnit.Month };
      Data.AddRange(new DateTime(2010, 1, 1), new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });

      var actual = TSTools.ChangeZoomLevel(Data, TimeStepUnit.Year, true);
      Assert.AreEqual(78, actual.GetValue(Data.StartTime));
      actual = TSTools.ChangeZoomLevel(Data, TimeStepUnit.Year, false);
      Assert.AreEqual(6.5, actual.GetValue(Data.StartTime), 1e-10);
      Assert.AreEqual(Data.DeleteValue, actual.GetValue(Data.StartTime.AddDays(400)));
    }

    [TestMethod()]
    public void ChangeZoomLevelTest2()
    {
      FixedTimeStepSeries Data = new FixedTimeStepSeries() { TimeStepSize = TimeStepUnit.Month };
      Data.AddRange(new DateTime(2010, 1, 1), new double[] { Data.DeleteValue, Data.DeleteValue, Data.DeleteValue });

      var actual = TSTools.ChangeZoomLevel(Data, TimeStepUnit.Year, true);
      Assert.AreEqual(Data.DeleteValue, actual.GetValue(Data.StartTime));
      actual = TSTools.ChangeZoomLevel(Data, TimeStepUnit.Year, false);
      Assert.AreEqual(Data.DeleteValue, actual.GetValue(Data.StartTime), 1e-10);
      Assert.AreEqual(Data.DeleteValue, actual.GetValue(Data.StartTime.AddDays(400)));
    }


    [TestMethod()]
    public void ChangeZoomLevelTest3()
    {
      FixedTimeStepSeries Data = new FixedTimeStepSeries() { TimeStepSize = TimeStepUnit.Month };
      Data.StartTime = new DateTime(2010, 1, 1);

      for (int i = 0; i < 36; i++)
      {
        Data.Add(i);
      }

      Assert.AreEqual(new DateTime(2012, 12, 1), Data.EndTime);

      var yearly = TSTools.ChangeZoomLevel(Data, TimeStepUnit.Year, false);

      Assert.AreEqual(5.5, yearly.Items[0].Value);
      Assert.AreEqual(17.5, yearly.Items[1].Value);
      Assert.AreEqual(29.5, yearly.Items[2].Value);


    }


    [TestMethod]
    public void ChangeZoomLevelTimespan()
    {
      TimeSpanSeries ts = new TimeSpanSeries();
      ts.Items.Add(new TimeSpanValue(new DateTime(2010, 1, 1, 12, 0, 0), new DateTime(2010, 1, 1, 12, 15, 0), 10));
      ts.Items.Add(new TimeSpanValue(new DateTime(2010, 1, 1, 12, 15, 0), new DateTime(2010, 1, 1, 12, 30, 0), 10));
      ts.Items.Add(new TimeSpanValue(new DateTime(2010, 10, 1, 12, 0, 0), new DateTime(2010, 10, 1, 12, 15, 0), 10));
      ts.TimeStepSize = TimeStepUnit.Minute;

      TimeSpanSeries ts2 = new TimeSpanSeries(TSTools.ChangeZoomLevel(ts, TimeStepUnit.Month, true));
      Assert.AreEqual(20, ts2.Max);
      Assert.AreEqual(2, ts2.Count);


    }

    [TestMethod]
    public void AlignTest()
    {
      TimeStampSeries ts1 = new TimeStampSeries();
      TimeStampSeries ts2 = new TimeStampSeries();

      for(int i =0;i< 100;i++)
      {
        ts1.Items.Add(new TimeStampValue(new DateTime(2010, 1, 1).AddDays(i), i));
        ts2.Items.Add(new TimeStampValue(new DateTime(2010, 2, 1).AddDays(i), i));
      }

      TimeStampValue[] t1;
      TimeStampValue[] t2;

      TSTools.AlignSeries(ts1, ts2, out t1, out t2);

      Assert.AreEqual(t1.Count(), t2.Count());

    }



    /// <summary>
    ///A test for Substract
    ///</summary>
    [TestMethod()]
    public void SubstractTest()
    {
      FixedTimeStepSeries ts1 = new FixedTimeStepSeries();
      FixedTimeStepSeries ts2 = new FixedTimeStepSeries();

      ts1.StartTime = new DateTime(2010, 1, 1);
      ts1.AddRange(new double[] { 2, 3, 4 });
      ts1.TimeStepSize = TimeStepUnit.Month;

      ts2.StartTime = new DateTime(2010, 1, 1);
      ts2.AddRange(new double[] { 1, 1, 1 });
      ts2.TimeStepSize = TimeStepUnit.Month;


      FixedTimeStepSeries actual;
      actual = TSTools.Substract(ts1, ts2);
      Assert.AreEqual(1, actual.GetValue(new DateTime(2010,1,1)));
      Assert.AreEqual(2, actual.GetValue(new DateTime(2010, 2, 1)));
    }
  }
}
