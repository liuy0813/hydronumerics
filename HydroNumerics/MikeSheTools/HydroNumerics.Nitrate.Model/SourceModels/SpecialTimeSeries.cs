﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using HydroNumerics.Core;
using HydroNumerics.Core.Time;

namespace HydroNumerics.Nitrate.Model
{
  public class FixedTimeSeries:BaseViewModel
  {

    SortedList<int, SortedList<int, float>> MonthlyValues = new SortedList<int,SortedList<int,float>>();

    public TimeStepUnit TimeStepSize { get; set; }


    private double _DeleteValue = 1e-035;

    public double DeleteValue
    {
      get { return _DeleteValue; }
      set
      {
        if (_DeleteValue != value)
        {
          _DeleteValue = value;
          RaisePropertyChanged("DeleteValue");
        }
      }
    }


    private SortedList<int, float> _InitialValues;
    public SortedList<int, float> InitialValues
    {
      get
      {
        if (_InitialValues == null)
          _InitialValues = MonthlyValues.Values.Where(v => v.Count == 12).First(); //Recycle first complete year
        return _InitialValues;
      }
      set
      {
        _InitialValues = value;
      }
    }


    
    #region Constructors
    public FixedTimeSeries()
    {
      TimeStepSize = TimeStepUnit.Month;
    }

    #endregion

    /// <summary>
    /// Gets the value at a point in time
    /// </summary>
    /// <param name="Time"></param>
    /// <returns></returns>
    public List<float> GetValues(DateTime First, DateTime Last)
    {
      List<float> toreturn = new List<float>();

      for (int i = First.Year; i <= Last.Year; i++)
      {
        int startmonth = 1;
        if (i == First.Year)
          startmonth = First.Month;

        int lastmonth = 12;
        if (i == Last.Year)
          lastmonth = Last.Month;

        SortedList<int, float> currentyear;

        if (!MonthlyValues.TryGetValue(i, out currentyear))
        {
          if (i < MonthlyValues.Keys.First())
            currentyear = InitialValues;
        }
        for (int j = startmonth; j <= lastmonth; j++)
        {
          if (currentyear.ContainsKey(j))
            toreturn.Add(currentyear[j]);
          else
            toreturn.Add((float)DeleteValue);
        }
      }
      return toreturn;
    }



    public void AddRange(DateTime Start, TimeSpan TimeStep, List<float> Values)
    {
      if(TimeStep == TimeSpan.FromDays(1))
      {
        float monthlyvalue=0;
        int daycounter =0;
        int currentyear= Start.Year;
        int currentmonth=Start.Month;

        for (int i =0;i<Values.Count();i++)
        {
          if (daycounter == DateTime.DaysInMonth(currentyear, currentmonth))
          {
            if (!MonthlyValues.ContainsKey(currentyear))
              MonthlyValues.Add(currentyear,new SortedList<int,float>());
            MonthlyValues[currentyear].Add(currentmonth, monthlyvalue / (86400f * DateTime.DaysInMonth(currentyear, currentmonth)));
            monthlyvalue =0;
            daycounter = 0;
            currentmonth++;
            if (currentmonth > 12)
            {
              currentmonth = 1;
              currentyear++;
            }
          }
          daycounter++;
          monthlyvalue += Values[i];
        }
        if (!MonthlyValues.ContainsKey(currentyear))
          MonthlyValues.Add(currentyear, new SortedList<int, float>());
        MonthlyValues[currentyear].Add(currentmonth, monthlyvalue / (86400f*DateTime.DaysInMonth(currentyear, currentmonth)));

      }
      RaisePropertyChanged("EndTime");
      RaisePropertyChanged("Sum");
      RaisePropertyChanged("Average");
      RaisePropertyChanged("Min");
      RaisePropertyChanged("Max");
    }


    public void Multiply(float factor)
    {
      foreach (var year in MonthlyValues)
      {
        var months = year.Value.Keys.ToList();
        foreach (var month in months)
          year.Value[month] *= factor;
      }
      if (_InitialValues != null)
      {
        var months = _InitialValues.Keys.ToList();
        foreach (var month in months)
          _InitialValues[month] *= factor;
      }
    }



    #region Properties

    /// <summary>
    /// Get the fixed time series as a TimeSpanseries
    /// </summary>
    public IEnumerable<TimeSpanValue> ToTimeSpanseries
    {
      get
      {
        foreach (var kvpyear in MonthlyValues)
        {
          foreach (var kvpmonth in kvpyear.Value)
          {
            DateTime start = new DateTime(kvpyear.Key, kvpmonth.Key, 1);
            yield return new TimeSpanValue(start, start.AddMonths(1), kvpmonth.Value);
          }
        }
      }
    }


    public DateTime StartTime
    {
      get { return new DateTime(MonthlyValues.First().Key, MonthlyValues.First().Value.First().Key,1) ; }
    }

    /// <summary>
    /// Gets the End time
    /// </summary>
    public DateTime EndTime
    {
      get { return new DateTime(MonthlyValues.Last().Key, MonthlyValues.Last().Value.Last().Key,1) ; }
    }
    
    
    /// <summary>
    /// Gets the sum og the values
    /// </summary>
    public double Sum
    {
      get
      {
        return MonthlyValues.SelectMany(kvp=>kvp.Value.Select(m=>m.Value)).Sum();
      }
    }

    /// <summary>
    /// Gets the average of the values
    /// </summary>
    public double Average
    {
      get
      {
        return MonthlyValues.SelectMany(kvp => kvp.Value.Select(m => m.Value)).Average();
      }
    }

    /// <summary>
    /// Gets the maximum of the values
    /// </summary>
    public double Max
    {
      get
      {
        return MonthlyValues.SelectMany(kvp => kvp.Value.Select(m => m.Value)).Max();
      }
    }

    /// <summary>
    /// Gets the minimum of the values
    /// </summary>
    public double Min
    {
      get
      {
        return MonthlyValues.SelectMany(kvp => kvp.Value.Select(m => m.Value)).Min();
      }
    }

    #endregion
  }
}
