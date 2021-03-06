﻿using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HydroNumerics.Core;
using HydroNumerics.MikeSheTools.Mike11;
using HydroNumerics.MikeSheTools.PFS.SheFile;
using HydroNumerics.MikeSheTools.PFS.Well;
using HydroNumerics.Wells;
using HydroNumerics.Time.Core;
using HydroNumerics.MikeSheTools.DFS;

using DHI.DHIfl;

namespace HydroNumerics.MikeSheTools.Core
{
  /// <summary>
  /// This class provides access to setup data, processed data and results.
  /// Access to processed data and results requires that the model is preprocessed and run, respectively. 
  /// </summary
  public class Model:FileClass,IDisposable
  {
    private ProcessedData _processed;
    private Results _results;
    private FileNames _files;
    private InputFile _input;
    private TimeInfo _time;
    private M11Setup _river;

    public event EventHandler SimulationFinished;

    public string DisplayName { get; set; }

    public Model(string SheFileName):base(SheFileName)
    {
      DisplayName = FileName;
    }


    private List<CalibrationParameter> parameters;
    /// <summary>
    /// Gets the list of calibration parameters
    /// </summary>
    public List<CalibrationParameter> Parameters
    {
      get
      {
        if (parameters == null)
        {
          parameters = new List<CalibrationParameter>();

          foreach (var v in Input.MIKESHE_FLOWMODEL.SaturatedZone.GeoUnitsSZProperties.GeoUnit_1s)
          {
            CalibrationParameter cp = new CalibrationParameter("HorConduc", v);
            cp.DisplayName = "Horisontal conductivity in " + v.SoilName;
            cp.ShortName = "Kh in " + v.SoilName;
            parameters.Add(cp);

            CalibrationParameter cp2 = new CalibrationParameter("VerConduc", v);
            cp2.DisplayName = "Vertical conductivity in " + v.SoilName;
            cp2.ShortName = "Kv in " + v.SoilName;
            parameters.Add(cp2);
          }

          if (Input.MIKESHE_FLOWMODEL.SaturatedZone.Drainage.TimeConstant.Type == 0)
          {
            CalibrationParameter cp = new CalibrationParameter("FixedValue", Input.MIKESHE_FLOWMODEL.SaturatedZone.Drainage.TimeConstant);
            cp.DisplayName = "Drainage Time Constant";
            cp.ShortName = cp.DisplayName;
            parameters.Add(cp);
          }

          CalibrationParameter cp3 = new CalibrationParameter("LeakageCoefficient", River.network.nwkfile.MIKE_11_Network_editor.MIKESHECOUPLING.MikeSheCouplings);
          cp3.DisplayName = "Global leakage coefficient";
          cp3.ShortName = cp3.DisplayName;
          parameters.Add(cp3);
        }
        return parameters;
      }
    }

    public M11Setup River
    {
      get
      {
        if (_river == null)
        {
          _river = new M11Setup();
          _river.ReadSetup(Files.Sim11FileName, true);
        }
        return _river;
      }
        
    }


    /// <summary>
    /// Gets the file names
    /// </summary>
    public FileNames Files
    {
      get { 
        if (_files == null)
          _files = new FileNames(Input);

        return _files; }
    }

    /// <summary>
    /// Gets the grid info object
    /// Returns null if the model has not been preprocessed.
    /// </summary>
    public MikeSheGridInfo GridInfo
    {
      get 
      {
        if (Processed !=null)
          return Processed.Grid;
        return null;
      }
    }

    /// <summary>
    /// Gets read and write access to the input in the .she-file.
    /// Remember to save changes.
    /// </summary>
    public InputFile Input
    {
      get {
        if (_input == null)
          _input = new InputFile(FileName);
        return _input;
      }
    }

    /// <summary>
    /// Gets read access to the results
    /// Returns null if there are no results
    /// </summary>
    public Results Results
    {
      get { 
        if (_results == null)
            _results = new Results(this);

        return _results; }
    }

    /// <summary>
    /// Gets read access to the processed data
    /// Returns null if the model has not been preprocessed.
    /// </summary>
    public ProcessedData Processed
    {
      get
      {
        if (_processed == null)
          if (File.Exists(Files.PreProcessed2D)) 
            _processed = new ProcessedData(Files);

        return _processed;
      }
    }


    private List<MikeSheWell> extractionWells;
    public List<MikeSheWell> ExtractionWells
    {
      get
      {
        if (extractionWells == null)
        {
          WellFile WF = new WellFile(Files.WelFileName);
          extractionWells = new List<MikeSheWell>();
          foreach (var w in WF.WEL_CFG.WELLDATA.WELLNO_1s)
          {
            
            MikeSheWell NewW = new MikeSheWell(w.ID, w.XCOR, w.YCOR, GridInfo);
            NewW.Terrain = w.LEVEL;
            NewW.AddNewIntake(1);
            double[] screenDepths = new double[GridInfo.NumberOfLayers];

            foreach (var filter in w.FILTERDATA.FILTERITEM_1s)
            {
              Screen sc = new Screen(NewW.Intakes.First());
              sc.BottomAsKote = filter.Bottom;
              sc.TopAsKote = filter.Top;

              for (int i = Math.Max(0,GridInfo.GetLayerFromDepth(NewW.Column, NewW.Row, sc.DepthToBottom.Value)); i < GridInfo.GetLayerFromDepth(NewW.Column, NewW.Row, sc.DepthToTop.Value); i++)
              {
                double d1 = Math.Max(sc.BottomAsKote.Value, GridInfo.LowerLevelOfComputationalLayers.Data[NewW.Row, NewW.Column,i]);
                double d2 = Math.Min(sc.TopAsKote.Value, GridInfo.UpperLevelOfComputationalLayers.Data[NewW.Row, NewW.Column, i]);
                screenDepths[i] += d2 - d1;
              }
            }

            int maxi=0;
            for (int i=0; i< screenDepths.Count();i++)
              if (screenDepths[i]>screenDepths[maxi])
                maxi = i;

            NewW.Layer = maxi;
            extractionWells.Add(NewW);
          }
        }
        return extractionWells;
      }
    }


    private List<MikeSheWell> observationWells;

    /// <summary>
    /// Gets the list of observation wells. Currently it does not read in the results but the observations to match
    /// </summary>
    public List<MikeSheWell> ObservationWells
    {
      get
      {
        if (observationWells == null)
        {
          observationWells = new List<MikeSheWell>();
          MikeSheWell CurrentWell;
          IIntake CurrentIntake;
          DFS0 _tso = null;

          foreach (var dt in Input.MIKESHE_FLOWMODEL.StoringOfResults.DetailedTimeseriesOutput.Item_1s)
          {
            if (dt.HydrComp == 101)
            {

              CurrentWell = new MikeSheWell(dt.Name, dt.X, dt.Y, GridInfo);
              CurrentWell.UsedForExtraction = false;
              CurrentIntake = CurrentWell.AddNewIntake(1);
              Screen sc = new Screen(CurrentIntake);
              sc.DepthToTop = dt.Z;
              sc.DepthToBottom = dt.Z;
              CurrentWell.Layer = GridInfo.GetLayerFromDepth(CurrentWell.Column, CurrentWell.Row, sc.DepthToTop.Value);
              CurrentWell.Terrain = GridInfo.SurfaceTopography.Data[CurrentWell.Row, CurrentWell.Column];

              //Read in observations if they are included
              if (dt.InclObserved == 1)
              {
                if (_tso == null || _tso.FileName != dt.TIME_SERIES_FILE.FILE_NAME)
                  _tso = new DFS0(dt.TIME_SERIES_FILE.FILE_NAME);

                //Loop the observations and add
                for (int i = 0; i < _tso.NumberOfTimeSteps; i++)
                {
                  double d =_tso.GetData(i, dt.TIME_SERIES_FILE.ITEM_NUMBERS);
                  if (d != _tso.DeleteValue)
                    CurrentIntake.HeadObservations.Items.Add(new TimestampValue((DateTime)_tso.TimeSteps[i], d));
                }
              }

              observationWells.Add(CurrentWell);
            }
          }

        }
        return observationWells;
      }
    }


    /// <summary>
    /// Gets a class holding info about time.
    /// </summary>
    public TimeInfo Time
    {
      get
      {
        if (_time == null)
          _time = new TimeInfo(Input);
        return _time;
      }
    }

    public void BeginRun()
    {
      Run(true,true);
    }

    /// <summary>
    /// Runs the Mike She model
    /// </summary>
    public void Run(bool Asynchronous, bool UseMzLauncher)
    {
      if (_input!= null)
        Input.Save();
      if (_river != null)
        River.network.nwkfile.Save();
      Dispose();
      PreprocessAndRun(Asynchronous, UseMzLauncher);
    }

    Process Runner;
    /// <summary>
    /// Preprocesses and runs Mike She
    /// Note that if the MzLauncher is used it uses the execution engine flags from the .she-file
    /// </summary>
    /// <param name="MsheFileName"></param>
    /// <param name="UseMZLauncher"></param>
    private void PreprocessAndRun(bool Async, bool UseMZLauncher)
    {
      Console.WriteLine("Starting simulation: " + FileName);

//      if (Runner==null)
        Runner = new Process();
      string path;
      DHIRegistry key = new DHIRegistry(DHIProductAreas.COMMON_COMPONNETS, false);
      key.GetHomeDirectory(out path);

      if (!File.Exists(Path.Combine(path, "Mshe_preprocessor.exe")))
        path += "\\x64";


      if (UseMZLauncher)
      {
        Runner.StartInfo.FileName = Path.Combine(path, "Mzlaunch.exe");
        Runner.StartInfo.Arguments = Path.GetFullPath(FileName) + " -exit";
      }
      else
      {
        Runner.StartInfo.FileName = Path.Combine(path, "Mshe_preprocessor.exe");
        Runner.StartInfo.Arguments = FileName;
        Runner.Start();
        Runner.WaitForExit();
        Runner.StartInfo.FileName = Path.Combine(path, "Mshe_watermovement.exe");
      }

      Runner.EnableRaisingEvents = true;
      Runner.Exited += new EventHandler(Runner_Exited);
      Runner.Start();
      
      if (!Async)
      {
        Runner.WaitForExit();
      }
    }

    /// <summary>
    /// Event handler for when the process is finished
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Runner_Exited(object sender, EventArgs e)
    {
      Console.WriteLine("Simulation finished: " + FileName);
      
      Runner.Dispose();
      if (SimulationFinished != null)
        SimulationFinished(this, e);
    }

      

    #region IDisposable Members

    public void Dispose()
    {
      if (_processed != null)
      {
        _processed.Dispose();
        _processed = null;
      }
      if (_results != null)
      {
        _results.Dispose();
        _results = null;
      }
      _input = null;
      extractionWells = null;
    }

    #endregion
  }
}
