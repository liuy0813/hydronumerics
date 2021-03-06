﻿using HydroNumerics.MikeSheTools.PFS.MEX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HydroNumerics.MikeSheTools.PFS.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for MexFileTest and is intended
    ///to contain all MexFileTest Unit Tests
    ///</summary>
  [TestClass()]
  public class MexFileTest
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
    ///A test for MexFile Constructor
    ///</summary>
    [Ignore]
    [TestMethod()]
    public void MexFileConstructorTest()
    {
      string MexFileName = @"C:\Users\Jacob\Projekter\GEUS\Silkeborg\JacobGudbjerg\networkBase.mex";
      MexFile target = new MexFile(MexFileName);

      Assert.AreEqual(514, target.MOUSE_LINKS.Links.Count);

    }

    [TestMethod]
    public void ConstructorTest2()
    {


    }
  }
}
