﻿using HydroNumerics.HydroNet.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

using HydroNumerics.Geometry;

namespace HydroNumerics.HydroNet.Core.UnitTest
{
    
    /// <summary>
    ///This is a test class for GroundWaterBoundaryTest and is intended
    ///to contain all GroundWaterBoundaryTest Unit Tests
    ///</summary>
  [TestClass()]
  public class GroundWaterBoundaryTest
  {

    GroundWaterBoundary target;
    double hydraulicConductivity;
    double area;
    double distance;
    double head;
    Lake s;


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
    [TestInitialize()]
    public void MyTestInitialize()
    {
      hydraulicConductivity = 0.0001; //m/s
      area = 2.5; //m2
      distance = 34; //m
      head = 22; // m
      s = new Lake("s",100);
     target = new GroundWaterBoundary(s, hydraulicConductivity, distance, head, XYPolygon.GetSquare(2.5));
     target.WaterSample = new WaterPacket(1, 150);
    }
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for GetSinkVolume
    ///</summary>
    [TestMethod()]
    public void GetSinkVolumeTest()
    {
      s.WaterLevel = 24;
      DateTime Start = DateTime.Now; 
      TimeSpan TimeStep = new TimeSpan(1,0,0); 
      double expected = area * hydraulicConductivity * (s.WaterLevel - head) / distance * TimeStep.TotalSeconds;
      double actual;
      actual = target.GetSinkVolume(Start, TimeStep);
      Assert.AreEqual(expected, actual, 0.000001);
    }

    /// <summary>
    ///A test for GetSourceWater
    ///</summary>
    [TestMethod()]
    public void GetSourceWaterTest()
    {
      s.WaterLevel = 18;
      TimeSpan TimeStep = new TimeSpan(1, 0, 0);
      DateTime Start = DateTime.Now;

      int ID = 1;
      IWaterPacket expected = new WaterPacket(ID, area * hydraulicConductivity * (head - s.WaterLevel) / distance * TimeStep.TotalSeconds);
      IWaterPacket actual;

      actual = target.GetSourceWater(Start, TimeStep);
      Assert.AreEqual(ID, actual.Composition.Keys.First());
      Assert.AreEqual(expected.Volume, actual.Volume,0.000001);
    }

    [TestMethod]
    public void RunOnstoredData()
    {
      Lake L = new Lake("Test", 1000);
      GroundWaterBoundary gwb = new GroundWaterBoundary(L, 14 - 5, 1, 10, (XYPolygon) L.Geometry);
      L.WaterLevel = 9;
      DateTime Start = new DateTime(2000, 1, 1);
      DateTime End = new DateTime(2000, 2, 1);
      L.GroundwaterBoundaries.Add(gwb);
      Model m = new Model();
      m._waterBodies.Add(L);
      m.SetState("Initial", Start, new WaterPacket(1));
      m.MoveInTime(End, TimeSpan.FromDays(1));

      DateTime Mid = new DateTime(2000,1,15);

      double flow = L.Output.GroundwaterInflow.GetValue(Mid);

      gwb.FlowType = GWType.Flow;
      gwb.HydraulicConductivity = 1;
      gwb.GroundwaterHead = 1;

      m.RestoreState("Initial");
      m.MoveInTime(End, TimeSpan.FromDays(1));
      Assert.AreEqual(flow, L.Output.GroundwaterInflow.GetValue(Mid));

    }

    

    [TestMethod()]
    public void SourceTest()
    {
      s.WaterLevel = 100;
      Assert.IsFalse(target.IsSource(DateTime.Now));
      s.WaterLevel = -100;
      Assert.IsTrue(target.IsSource(DateTime.Now));
    }
  }
}
