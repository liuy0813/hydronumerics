﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HydroNumerics.HydroNet.OpenMI;
using HydroNumerics.OpenMI.Sdk.Backbone;
using HydroNumerics.HydroNet.Core;
using HydroNumerics.Time.Core;
using OpenMI.Standard;

namespace HydroNumerics.HydroNet.OpenMI.UnitTest
{
    /// <summary>
    /// Summary description for EngineWrapperTest
    /// </summary>
    [TestClass]
    public class EngineWrapperTest
    {
        //string testDataPath; 
        System.Collections.Hashtable arguments;
        string inputFilename;

        public EngineWrapperTest()
        {
            //testDataPath = @"..\..\..\TestData\";
            arguments = new System.Collections.Hashtable();
            inputFilename = "EngineWrapperTestHydroNetInputFile.xml";
            arguments.Add("InputFilename", inputFilename);
            arguments.Add("TimestepLength", "2"); //2 seconds
        }

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
        // You can use the following additional attributes as you write your tests:
        //
         //Use ClassInitialize to run code before running the first test in the class
         //[ClassInitialize()]
         //public static void MyClassInitialize(TestContext testContext) 
         //{
            
         //}
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
         //Use TestInitialize to run code before running each test 
         [TestInitialize()]
         public void MyTestInitialize() 
         {
             Model model = CreateHydroNetModel();
             model.Save(inputFilename);
         }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void SaveModel()
        {
            Model model = CreateHydroNetModel();
            model.Save(inputFilename);
        }
        
        [TestMethod]
        public void Initialize()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
        }

        [TestMethod]
        public void GetComponentID()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
            Assert.AreEqual("HydroNet", engineWrapper.GetComponentID());
        }

        [TestMethod]
        public void GetComponentDescription()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
            Assert.AreEqual("Conceptual model for trasport of water and solutes", engineWrapper.GetComponentDescription());
        }

        [TestMethod]
        public void GetModelID()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
            Assert.AreEqual("HydroNet test model", engineWrapper.GetModelID());
        }

        [TestMethod]
        public void GetModelDescription()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
            Assert.AreEqual("No modeldescription available", engineWrapper.GetModelDescription());
        }

        [TestMethod]
        public void Finish()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
            engineWrapper.Finish();
        }

        [TestMethod]
        public void GetOutputExchangeItems()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
            
            Assert.AreEqual(1, engineWrapper.GetOutputExchangeItemCount());

            List<HydroNumerics.OpenMI.Sdk.Backbone.OutputExchangeItem> outputExchangeItemsList = new List<OutputExchangeItem>();

            for (int i = 0; i < engineWrapper.GetOutputExchangeItemCount(); i++)
            {
                outputExchangeItemsList.Add(engineWrapper.GetOutputExchangeItem(i));
            }
            

            OutputExchangeItem outputExchangeItem = outputExchangeItemsList.First(myVar => myVar.Quantity.ID == "Leakage");
            Assert.AreEqual("Leakage", outputExchangeItem.Quantity.ID);
            Assert.AreEqual("Groundwater boundary under Upper Lake", outputExchangeItem.ElementSet.ID);
            Assert.AreEqual(ElementType.XYPolygon, outputExchangeItem.ElementSet.ElementType);
            Assert.AreEqual(6, outputExchangeItem.ElementSet.GetVertexCount(0));
            
        }

        [TestMethod]
        public void GetInputExchangeItems()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);

            Assert.AreEqual(2, engineWrapper.GetInputExchangeItemCount());

            List<HydroNumerics.OpenMI.Sdk.Backbone.InputExchangeItem> inputExchangeItemsList = new List<InputExchangeItem>();

            for (int i = 0; i < engineWrapper.GetInputExchangeItemCount(); i++)
            {
                inputExchangeItemsList.Add(engineWrapper.GetInputExchangeItem(i));
            }

            InputExchangeItem inputExchangeItem = inputExchangeItemsList.First(myVar => myVar.Quantity.ID == "Flow");
            Assert.AreEqual("Flow", inputExchangeItem.Quantity.ID);
            Assert.AreEqual("Inflow to Upper lake", inputExchangeItem.ElementSet.ID);
            Assert.AreEqual(ElementType.IDBased, inputExchangeItem.ElementSet.ElementType);

            inputExchangeItem = inputExchangeItemsList.First(myVar => myVar.Quantity.ID == "Head");
            Assert.AreEqual("Head", inputExchangeItem.Quantity.ID);
            Assert.AreEqual("Groundwater boundary under Upper Lake", inputExchangeItem.ElementSet.ID);
            Assert.AreEqual(ElementType.XYPolygon, inputExchangeItem.ElementSet.ElementType);
            Assert.AreEqual(6, inputExchangeItem.ElementSet.GetVertexCount(0));
        }

        [TestMethod]
        public void PerformTimeStep()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
            Assert.AreEqual(2, engineWrapper.HydroNetModel._waterBodies[0].CurrentStoredWater.Volume);
            engineWrapper.PerformTimeStep();
            engineWrapper.Finish();
        }

        [TestMethod]
        public void GetValues()
        {
            EngineWrapper engineWrapper = new EngineWrapper();
            engineWrapper.Initialize(arguments);
            engineWrapper.PerformTimeStep();

            Assert.AreEqual(1, ((IScalarSet)engineWrapper.GetValues("Leakage", "Groundwater boundary under Upper Lake")).Count);
            Assert.AreEqual(22.384, ((IScalarSet)engineWrapper.GetValues("Leakage", "Groundwater boundary under Upper Lake")).GetScalar(0),0.001);
        }

        [TestMethod]
        public void SetValues()
        {
          EngineWrapper_Accessor engineWrapper = new EngineWrapper_Accessor();
            engineWrapper.Initialize(arguments);
            engineWrapper.SetValues("Flow", "Inflow to Upper lake", new ScalarSet(new double[] { 5.5 }));
            Assert.AreEqual(5.5, ((SinkSourceBoundary)engineWrapper.model._waterBodies.First().Sources.First()).OverrideFlowRate.Value);

            engineWrapper.SetValues("Head", "Groundwater boundary under Upper Lake", new ScalarSet(new double[] { 7.6 }));
            Assert.AreEqual(7.6, ((GroundWaterBoundary)engineWrapper.model._waterBodies.First().GroundwaterBoundaries.First()).GroundwaterHead);
         
        }

        private Model CreateHydroNetModel()
        {
            // Upper Lake configuration
            Lake upperLake = new Lake("Upper Lake", 1000);

            //Simple inflow boundary
            SinkSourceBoundary inflow = new SinkSourceBoundary(2);
            inflow.Name = "Inflow to Upper lake";
            inflow.ContactGeometry = new HydroNumerics.Geometry.XYPoint(350, 625);
            upperLake.Sources.Add(inflow);

            //Ground water boundary
            HydroNumerics.Geometry.XYPolygon contactPolygon = new HydroNumerics.Geometry.XYPolygon();
            contactPolygon.Points.Add(new HydroNumerics.Geometry.XYPoint(350, 625));
            contactPolygon.Points.Add(new HydroNumerics.Geometry.XYPoint(447, 451));
            contactPolygon.Points.Add(new HydroNumerics.Geometry.XYPoint(715, 433));
            contactPolygon.Points.Add(new HydroNumerics.Geometry.XYPoint(863, 671));
            contactPolygon.Points.Add(new HydroNumerics.Geometry.XYPoint(787, 823));
            contactPolygon.Points.Add(new HydroNumerics.Geometry.XYPoint(447, 809));
            GroundWaterBoundary groundWaterBoundary = new GroundWaterBoundary();
            groundWaterBoundary.Connection = upperLake;
            groundWaterBoundary.ContactGeometry = contactPolygon;
            groundWaterBoundary.Distance = 2.3;
            groundWaterBoundary.HydraulicConductivity = 1e-4;
            groundWaterBoundary.GroundwaterHead = 3.4;
            groundWaterBoundary.Name = "Groundwater boundary under Upper Lake";
            upperLake.GroundwaterBoundaries.Add(groundWaterBoundary);

            //Stream between the lakes
            Stream stream = new Stream("stream", 2000, 2, 1.1);

            //Lower Lake configuration
            Lake lowerLake = new Lake("Lower Lake", 20);
 
            //Connecting the waterbodies.
            upperLake.AddDownStreamWaterBody(stream);
            stream.AddDownStreamWaterBody(lowerLake);

            //Creating the model
            Model model = new Model();          
            model._waterBodies.Add(upperLake);
            model._waterBodies.Add(stream);
            model._waterBodies.Add(lowerLake);

            DateTime startTime = new DateTime(2010, 1, 1);
            model.SetState("MyState", startTime, new WaterPacket(1000));
            upperLake.SetState("MyState", startTime, new WaterPacket(2));
            model.Name = "HydroNet test model";
            model.Initialize();

            return model;
        }

        [TestMethod]
        public void HydroNetExample01()
        {
            Lake lake = new Lake("L", 10000);
            SinkSourceBoundary flowBoundary = new SinkSourceBoundary(2);
            lake.Sources.Add(flowBoundary);
            Model model = new Model();
            model._waterBodies.Add(lake);
            model.SetState("Kurt", new DateTime(2010, 1, 1), new WaterPacket(1.2));
            
            for (int i = 0; i < 5; i++)
            {
                model.Update(model.CurrentTime.Add(new System.TimeSpan(0, 0, 1)));
            }
        }
    }
}
