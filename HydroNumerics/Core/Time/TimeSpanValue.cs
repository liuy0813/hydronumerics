﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using HydroNumerics.Core;


namespace HydroNumerics.Core.Time
{
  [DataContract]
  public class TimeSpanValue:GalaSoft.MvvmLight.ObservableObject, IComparable<TimeSpanValue>, IValue
  {
 

    public TimeSpanValue()
    {
    }

    public TimeSpanValue(DateTimeSize Period, double Value)
    {
      TimePeriod = Period;
      this.Value = Value;
    }



    public TimeSpanValue(DateTime Start, DateTime End, double Value)
    {
      StartTime = Start;
      EndTime = End;
      if (End < Start)
        throw new ArgumentOutOfRangeException("End cannot be before start");
      this.Value = Value;
    }

    private bool isEnabled = true;

    /// <summary>
    /// Gets and sets IsEnabled;
    /// </summary>
    [DataMember]
    public bool IsEnabled
    {
      get { return isEnabled; }
      set
      {
        if (value != isEnabled)
        {
          isEnabled = value;
          RaisePropertyChanged("IsEnabled");
        }
      }
    }

    private DateTimeSize _TimePeriod = new DateTimeSize();
    [DataMember]
    public DateTimeSize TimePeriod
    {
      get { return _TimePeriod; }
      set
      {
        if (_TimePeriod != value)
        {
          _TimePeriod = value;
          RaisePropertyChanged("TimePeriod");
        }
      }
    }
    


    /// <summary>
    /// Gets and sets StartTime;
    /// </summary>
    public DateTime StartTime
    {
      get { return TimePeriod.Start; }
      set
      {
        if (value != TimePeriod.Start)
        {
          TimePeriod.Start = value;
          RaisePropertyChanged("StartTime");
        }
      }
    }




    /// <summary>
    /// Gets and sets EndTime;
    /// </summary>
    public DateTime EndTime
    {
      get { return TimePeriod.End; }
      set
      {
        if (value != TimePeriod.End)
        {
          TimePeriod.End = value;
          RaisePropertyChanged("EndTime");
        }
      }
    }

    /// <summary>
    /// Gets the duration
    /// </summary>
    public TimeSpan Duration
    {
      get
      {
        return TimePeriod.Size;
      }
    }

    private double _value;

    /// <summary>
    /// Gets and sets Value;
    /// </summary>
    [DataMember]
    public double Value
    {
      get { return _value; }
      set
      {
        if (value != _value)
        {
          _value = value;
          RaisePropertyChanged("Value");
        }
      }
    }

    public override bool Equals(object obj)
    {
      var tsv = obj as TimeSpanValue;
      if (tsv == null)
        return false;
      else
      {
        return tsv.StartTime == StartTime && tsv.EndTime == EndTime && tsv.Value == Value;
      }
    }


    public override int GetHashCode()
    {
      int result = 17;
      result = result * 37 + StartTime.GetHashCode();
      result = result * 37 + EndTime.GetHashCode();
      result = result * 37 + Value.GetHashCode();
      return result;
    }

    public override string ToString()
    {
      return "Start = " + StartTime.ToString("yyyy-MM-dd HH:mm") + ", Value = " + Value.ToString() + ", End = " + EndTime.ToString("yyyy-MM-dd HH:mm");
    }

    /// <summary>
    /// First compares StartTime then Endtime then value.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(TimeSpanValue other)
    {
      int compare = TimePeriod.CompareTo(other.TimePeriod);
      if (compare == 0) //also same endtime, use value
        compare = Value.CompareTo(other.Value);
      return compare;
    }

    public bool TimeOverlaps(TimeSpanValue other)
    {
      return TimePeriod.OverLaps(other.TimePeriod);
    }

    public bool TimeIncludes(DateTime Time)
    {
      return TimePeriod.Includes(Time);
    }

  }
}
