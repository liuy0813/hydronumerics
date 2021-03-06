﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using HydroNumerics.Geometry;

namespace HydroNumerics.Wells
{
  /// <summary>
  /// A small class holding typical data to describe a well
  /// </summary>
  [DataContract]
  public class Well : XYPoint, IWell,IEquatable<IWell>
  {

    protected string _id;
    protected string _description;
    protected double _terrain;
 
    /// <summary>
    /// Gets and sets the Depth
    /// </summary>
    [DataMember]
    public double? Depth { get; set; }
    public bool UsedForExtraction { get; set; }
    /// <summary>
    /// Gets and set the start date
    /// </summary>
    [DataMember]
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// Gets and sets the end date
    /// </summary>
    [DataMember]
    public DateTime? EndDate { get; set; }


//    [DataMember]
    public IEnumerable<IIntake> Intakes
    {
      get
      {
        return _intakes;
        
      }
    }
    protected List<IIntake> _intakes = new List<IIntake>();

    #region Constructors

    

    public Well(string ID)
    {
      _id = ID;
    }

    public Well(string ID, double X, double Y):this(ID)
    {
      this.X = X;
      this.Y = Y;
    }
    #endregion

    /// <summary>
    /// Adds a new intake to the well
    /// </summary>
    /// <param name="IDNumber"></param>
    /// <returns></returns>
    public virtual IIntake AddNewIntake(int IDNumber)
    {
      Intake I = new Intake(this, IDNumber);
      _intakes.Add(I);
      return I;
    }

    public virtual void AddIntake(IIntake I)
    {
      _intakes.Add(I);
    }

    public override string ToString()
    {
      return _id;
    }

    #region Properties

    /// <summary>
    /// Gets and sets the ID of the well
    /// </summary>
    [DataMember]
    public string ID
    {
      get { return _id; }
      set { _id = value; }
    }

    /// <summary>
    /// Gets and sets a description
    /// </summary>
    [DataMember]
    public string Description
    {
      get { return _description; }
      set { _description = value; }
    }

    /// <summary>
    /// Gets and sets the terrain in meters above mean sea level
    /// </summary>
    [DataMember]
    public double Terrain
    {
      get { return _terrain; }
      set { _terrain = value; }
    }

    #endregion

    #region IEquatable<IWell> Members

    public bool Equals(IWell other)
    {
      return ID.Equals(other.ID);
    }

    public override bool Equals(object obj)
    {
      if (!(obj is IWell))
        return false;

      return ID.Equals(((IWell)(obj)).ID);
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }

    #endregion
  }
}
