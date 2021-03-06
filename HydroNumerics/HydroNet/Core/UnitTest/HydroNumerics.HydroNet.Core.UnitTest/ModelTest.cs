﻿using HydroNumerics.HydroNet.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;

using HydroNumerics.Time.Core;

namespace HydroNumerics.HydroNet.Core.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for ModelTest and is intended
    ///to contain all ModelTest Unit Tests
    ///</summary>
  [TestClass()]
  public class ModelTest
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
    ///A test for MoveInTime
    ///</summary>
    [TestMethod()]
    public void MoveInTimeTest1()
    {
      var Network = NetworkBuilder.CreateBranch(10);


      Network.First().Sources.Add(new SinkSourceBoundary(1));

      Model target = new Model();
      target._waterBodies.AddRange(Network.Cast<IWaterBody>());

      DateTime Start = new DateTime(2010, 1, 1);
      DateTime End = new DateTime(2010, 1, 10);
      TimeSpan TimeStep = new TimeSpan(1, 0, 0, 0);

      target.SetState("Initial", Start, new WaterPacket(1));
      target.MoveInTime(End, TimeStep);

      Assert.AreEqual(Network.First().CurrentStoredWater.Volume, Network.Last().CurrentStoredWater.Volume,0.0001);
      Assert.AreEqual(End, Network.First().CurrentTime);

    }

    /// <summary>
    ///A test for MoveInTime
    ///100      300
    ///*         *
    /// *       *
    ///  *     *
    ///   *   *
    ///    * *
    ///     *
    ///     *
    ///     *
    ///     *
    ///     *
    ///    400
    ///</summary>
    [TestMethod()]
    public void MoveInTimeTest2()
    {
      SinkSourceBoundary b1 = new SinkSourceBoundary(100);
      b1.WaterSample = new WaterPacket(1, 1);

      SinkSourceBoundary b2 = new SinkSourceBoundary(300);
      b2.WaterSample = new WaterPacket(2, 1);

      var Network = NetworkBuilder.CreateSortedYBranch(5, b1, b2);

      Model target = new Model();
      target._waterBodies.AddRange(Network.Cast<IWaterBody>());


      DateTime Start = new DateTime(2010, 1, 1);
      DateTime End = new DateTime(2010, 1, 3);
      TimeSpan TimeStep = new TimeSpan(1, 0, 0, 0);
      target.SetState("Initial", Start, new WaterPacket(1));
      target.MoveInTime(End, TimeStep);
      Assert.AreEqual(Network.First().Output.Outflow.Items.Last().Value * 4, Network.Last().Output.Outflow.Items.Last().Value,0.0001);
      Assert.AreEqual(End, Network.First().CurrentTime);

      Assert.AreEqual(0.25, Network.Last().CurrentStoredWater.Composition[b1.WaterSample.Composition.Keys.First()], 0.0001);
      Assert.AreEqual(0.75, Network.Last().CurrentStoredWater.Composition[b2.WaterSample.Composition.Keys.First()], 0.0001);

    }

        /// <summary>
    ///A test for MoveInTime with ChemicalWater
    ///100      300
    ///*         *
    /// *       *
    ///  *     *
    ///   *   *
    ///    * *
    ///     *
    ///     *
    ///     *
    ///     *
    ///     *
    ///    400
    ///</summary>
    [TestMethod()]
    public void MoveInTimeTest3()
    {
//      FlowBoundary b1 = new FlowBoundary(100);
//      WaterPacket W1 = new WaterPacket(1, 1);
//      W1.AddChemical(new Chemical(new ChemicalType("Cl", 32), 2.3));

//      double CLConc = W1.GetConcentration("Cl");

//      b1.WaterSample = W1;

//      FlowBoundary b2 = new FlowBoundary(300);

//      WaterPacket W2 = new WaterPacket(2, 1);
//      W2.AddChemical(new Chemical(new ChemicalType("Na", 12), 2.3));

//      b2.WaterSample = W2;

//      var Network = NetworkBuilder.CreateSortedYBranch(5, b1, b2);

// //     foreach (Stream IW in Network)
////        IW.CurrentStoredWater = new WaterPacket(100);

//      Model target = new Model();
//      target._waterBodies.AddRange(Network.Cast<IWaterBody>());


//      DateTime Start = new DateTime(2010, 1, 1);
//      DateTime End = new DateTime(2010, 1, 2);
//      TimeSpan TimeStep = new TimeSpan(1, 0, 0, 0);
//      target.MoveInTime(Start, End, TimeStep);

//      Assert.AreEqual(((WaterPacket)Network.First().CurrentStoredWater).Chemicals["Cl"].Moles, ((WaterPacket)Network.Last().CurrentStoredWater).Chemicals["Cl"].Moles*4,0.000001);
//      //Assert.AreEqual(((WaterPacket)Network[5].CurrentRoutedWater).Chemicals["Cl"].Moles, ((WaterPacket)Network.Last().CurrentRoutedWater).Chemicals["Na"].Moles);
    }

    [TestMethod]
    public void CompareStreamAndLakes()
    {
      var StreamNetwork =NetworkBuilder.CreateBranch(10);
      var LakeNetwork =NetworkBuilder.CreateConnectedLakes(10);

      Model Streams = new Model();
      Streams._waterBodies.AddRange(StreamNetwork.Cast<IWaterBody>());
      Model Lakes = new Model();
      Lakes._waterBodies.AddRange(LakeNetwork.Cast<IWaterBody>());

      SinkSourceBoundary b1 = new SinkSourceBoundary(100);
      StreamNetwork.First().Sources.Add(b1);
      LakeNetwork.First().Sources.Add(b1);

      Stopwatch SW = new Stopwatch();
      Stopwatch SW2 = new Stopwatch();


      DateTime Start = new DateTime(2000, 1, 1);
      DateTime End = new DateTime(2000, 1, 10);

      Streams.SetState("Initial", Start, new WaterPacket(1));
      Lakes.SetState("Initial", Start, new WaterPacket(1));

      SW.Start();
      Streams.MoveInTime(End, TimeSpan.FromHours(5) );
      SW.Stop();
      SW2.Start();
      Lakes.MoveInTime(End, TimeSpan.FromHours(5));
      SW2.Stop();

      TimespanSeries TS1 = StreamNetwork.Last().Output.Items.First() as TimespanSeries;
      TimespanSeries TS2 = LakeNetwork.Last().Output.Items.First() as TimespanSeries;

      for (int i = 0; i < TS1.Items.Count; i++)
     {
       Assert.AreEqual(TS1.Items[i].Value, TS2.Items[i].Value, 0.000001);
       Assert.AreEqual(TS1.Items[i].StartTime, TS2.Items[i].StartTime);

     }


    }
  }
}
