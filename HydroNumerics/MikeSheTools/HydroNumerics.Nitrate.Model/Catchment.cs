﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;


using HydroNumerics.Core.Time;
using HydroNumerics.Core;
using HydroNumerics.Geometry;


namespace HydroNumerics.Nitrate.Model
{
  public class Catchment : BaseViewModel
  {


    public Catchment(int ID)
    {
      this.ID = ID;
      UpstreamConnections = new List<Catchment>();
      EndParticles = new List<Particle>();
      StartParticles = new List<Particle>();

      SourceModels = new List<ISource>();
      InternalReduction = new List<ISink>();
      MainStreamReduction = new List<ISink>();

    }



    #region Properties

    private List<IXYPolygon> _CoastalZones = new List<IXYPolygon>();
    public List<IXYPolygon> CoastalZones
    {
      get { return _CoastalZones; }
      set
      {
        if (_CoastalZones != value)
        {
          _CoastalZones = value;
          RaisePropertyChanged("CoastalZones");
        }
      }
    }



    private DMUStation _Measurements;
    public DMUStation Measurements
    {
      get { return _Measurements; }
      set
      {
        if (_Measurements != value)
        {
          _Measurements = value;
          RaisePropertyChanged("Measurements");
        }
      }
    }


    public ZoomTimeSeries M11Flow { get; set; }
    public ZoomTimeSeries Precipitation { get; set; }
    public TimeStampSeries Temperature { get; set; }
    public TimeStampSeries Leaching { get; set; }

    public FixedTimeStepSeries SimNitrate{get;set;}
    public FixedTimeStepSeries ObsNitrate{get;set;}


    private List<Lake> _Lakes = new List<Lake>();
    /// <summary>
    /// Gets the list of lakes within the catchment
    /// </summary>
    public List<Lake> Lakes
    {
      get { return _Lakes; }
    }

    public Lake BigLake { get; set; }

    private List<Wetland> _Wetlands = new List<Wetland>();
    /// <summary>
    /// Gets the list of lakes within the catchment
    /// </summary>
    public List<Wetland> Wetlands
    {
      get { return _Wetlands; }
    }


    private Catchment downstreamConnection;

    /// <summary>
    /// Gets and sets the downstream catchment
    /// </summary>
    public Catchment DownstreamConnection
    {
      get
      { return downstreamConnection; }
      set
      {
        if (value != downstreamConnection)
        {
          downstreamConnection = value;
          RaisePropertyChanged("DownstreamConnection");
        }
      }
    }

    /// <summary>
    /// Gets the list of upstream catchments
    /// </summary>
    public List<Catchment> UpstreamConnections { get; private set; }

    /// <summary>
    /// Gets the list of particles ending up in this catchment
    /// </summary>
    public List<Particle> EndParticles { get; set; }

    /// <summary>
    /// Gets the list of particles starting in this catchment.
    /// </summary>
    public List<Particle> StartParticles { get; set; }
    
    public List<Tuple<double, double>> ParticleBreakthroughCurves { get; set; }

    public List<Tuple<double, double>> ParticleBreakthroughCurvesOxidized { get; set; }



    private IXYPolygon _Geometry;
    /// <summary>
    /// Gets and sets the geometry
    /// </summary>
    public IXYPolygon Geometry
    {
      get { return _Geometry; }
      set
      {
        if (_Geometry != value)
        {
          _Geometry = value;
          RaisePropertyChanged("Geometry");
        }
      }
    }

    private DateTime _CurrentTime;
    /// <summary>
    /// Gets the current time
    /// </summary>
    public DateTime CurrentTime
    {
      get { return _CurrentTime; }
      private set
      {
        if (_CurrentTime != value)
        {
          _CurrentTime = value;
          RaisePropertyChanged("CurrentTime");
        }
      }
    }


    private DataRow _CurrentState;
    /// <summary>
    /// Gets the current state variables
    /// </summary>
    public DataRow CurrentState
    {
      get { return _CurrentState; }
      private set
      {
        if (_CurrentState != value)
        {
          _CurrentState = value;
          RaisePropertyChanged("CurrentState");
        }
      }
    }

    public DataTable StateVariables { get; set; }

    public List<ISource> SourceModels { get; internal set; }

    public List<ISink> InternalReduction { get; internal set; }

    public List<ISink> MainStreamReduction { get; internal set; }

    #endregion


    public void Initialize(DateTime Start, DateTime End)
    {



    }

    /// <summary>
    /// Takes a time step
    /// </summary>
    /// <param name="Endtime"></param>
    public void MoveInTime(DateTime Endtime)
    {

      double output = 0;
      CurrentTime = Endtime;

      CurrentState = StateVariables.Rows.Find(new object[] { ID, CurrentTime });

      if (CurrentState == null)
      {
        CurrentState = StateVariables.NewRow();
        CurrentState["ID"] = ID;
        CurrentState["Time"] = CurrentTime;
        StateVariables.Rows.Add(CurrentState);
      }


      foreach (var S in SourceModels)
      {
        double value;
        if (S.Update)
        {
          value = S.GetValue(this, Endtime) * DateTime.DaysInMonth(Endtime.Year, Endtime.Month) * 86400.0;
          CurrentState[S.Name] = value;
        }
        if (!CurrentState.IsNull(S.Name))
          output += (double)CurrentState[S.Name];
      }


      foreach (var R in InternalReduction)
      {
        double value;
        if (R.Update)
        {
          value = R.GetReduction(this, output, Endtime) * DateTime.DaysInMonth(Endtime.Year, Endtime.Month) * 86400.0;
          CurrentState[R.Name] = value;
        }
        if (!CurrentState.IsNull(R.Name))
          output -= (double)CurrentState[R.Name];
      }

      foreach (var ups in UpstreamConnections)
        output += ups.GetDownStreamOutput(Endtime);


      //Do the global reductions
      foreach (var R in MainStreamReduction)
      {
        double value;
        if (R.Update)
        {
          value = R.GetReduction(this, output, Endtime) * DateTime.DaysInMonth(Endtime.Year, Endtime.Month) * 86400.0;
          CurrentState[R.Name] = value;
        }
        if (!CurrentState.IsNull(R.Name))
          output -= (double)CurrentState[R.Name];
      }

      if (Precipitation != null)
        CurrentState["Precipitation"] = Precipitation.GetTs(TimeStepUnit.Month).GetValue(CurrentTime);
      if (Temperature != null)
        CurrentState["Air Temperature"] = Temperature.GetValue(CurrentTime, InterpolationMethods.DeleteValue);
      if (M11Flow != null)
      {
        CurrentState["M11Flow"] = M11Flow.GetTs(TimeStepUnit.Month).GetValue(CurrentTime) * DateTime.DaysInMonth(CurrentTime.Year, CurrentTime.Month) * 86400;
        CurrentState["NetM11Flow"] = NetInflow.GetTs(TimeStepUnit.Month).GetValue(CurrentTime) * DateTime.DaysInMonth(CurrentTime.Year, CurrentTime.Month) * 86400;
      }
        if (Leaching != null)
        CurrentState["Leaching"] = Leaching.GetValue(CurrentTime, InterpolationMethods.DeleteValue) * DateTime.DaysInMonth(CurrentTime.Year, CurrentTime.Month) * 86400;



      if (Measurements != null)
      {
        CurrentState["ObservedFlow"] = Measurements.Flow.GetValue(CurrentTime, InterpolationMethods.DeleteValue);
        CurrentState["ObservedNitrate"] = Measurements.Nitrate.GetValue(CurrentTime, InterpolationMethods.DeleteValue);

        if(ObsNitrate==null)
        {
          ObsNitrate = new FixedTimeStepSeries(){ TimeStepSize = TimeStepUnit.Month, StartTime = CurrentTime};
          SimNitrate = new FixedTimeStepSeries(){ TimeStepSize = TimeStepUnit.Month, StartTime = CurrentTime};
        }
        ObsNitrate.Add(Measurements.Nitrate.GetValue(CurrentTime, InterpolationMethods.DeleteValue));
        SimNitrate.Add(output);
      }

      CurrentState["DownStreamOutput"] = output;
    }



    public DataRow Accumulate(DataTable StateCopy, DateTime EndTime)
    {
      CurrentState = StateVariables.Rows.Find(new object[] { ID, EndTime });
      var currentAccumulated = StateCopy.Rows.Find(new object[] { ID, EndTime });

      if (!(bool)currentAccumulated["IsAccumulated"])
      {
        foreach (var v in UpstreamConnections)
        {
          var ups = v.Accumulate(StateCopy, EndTime);
          for (int i = 8; i < StateVariables.Columns.Count; i++)
          {
            if (StateVariables.Columns[i].DataType == typeof(double) & StateVariables.Columns[i].ColumnName != "DownStreamOutput" & !ups.IsNull(i))
            {
              if (currentAccumulated.IsNull(i))
                currentAccumulated[i] = 0;
              currentAccumulated[i] = (double)currentAccumulated[i] + (double)ups[i];
            }
          }
        }
        currentAccumulated["IsAccumulated"] = true;
      }
      return currentAccumulated;
    }


    /// <summary>
    /// Gets the output from the catchment. Calling this method will make the catchment take a time step and call upstream catchments
    /// </summary>
    /// <param name="EndTime"></param>
    /// <returns></returns>
    public double GetDownStreamOutput(DateTime EndTime)
    {

      MoveInTime(EndTime);
      CurrentState = StateVariables.Rows.Find(new object[] { ID, EndTime });

      return ((double)CurrentState["DownStreamOutput"]);
    }



    private ZoomTimeSeries _NetInflow;
    public ZoomTimeSeries NetInflow
    {
      get
      {
        if (_NetInflow == null && M11Flow != null)
        {
          _NetInflow = new ZoomTimeSeries();
          var ts = M11Flow.GetTs(TimeStepUnit.Month);
          _NetInflow.SetTs(ts);
          var ups = UpstreamConnections.Where(ca => ca.M11Flow != null).Select(ca => ca.M11Flow.GetTs(TimeStepUnit.Month));
          foreach (var u in ups)
          {
            _NetInflow.SetTs(TSTools.Substract(_NetInflow.GetTs(TimeStepUnit.Month), u));
          }
        }
        return _NetInflow;
      }
    }
  }
}
