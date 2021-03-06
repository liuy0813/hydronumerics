﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


using HydroNumerics.Geometry;
using HydroNumerics.Wells;
using HydroNumerics.Time.Core;

namespace HydroNumerics.JupiterTools
{
  /// <summary>
  /// A class representing a water production plant. Holds the historical pumping time series and the associated wells
  /// </summary>
  [DataContract]
  public class Plant: XYPoint, IComparable<Plant>
  {

    #region Properties

    /// <summary>
    /// Unique ID number
    /// </summary>
    [DataMember]
    public int IDNumber { get; private set; }

    /// <summary>
    /// Timeseries with extraction rates. 
    /// </summary>
   // [DataMember]
    public TimespanSeries Extractions { get; private set; }

    /// <summary>
    /// Time series with extraction from surface water.
    /// </summary>
    public TimespanSeries SurfaceWaterExtrations { get; private set; }
    
    /// <summary>
    /// The intakes associated to this plant
    /// </summary>
    public BindingList<PumpingIntake> PumpingIntakes { get; private set; }
    private bool PumpingIntakesChanged = true;
    private IWellCollection wells;

    public IWellCollection PumpingWells
    {
      get
      {
        if (PumpingIntakesChanged)
        {
          wells = new IWellCollection();
          foreach (PumpingIntake PI in PumpingIntakes)
            if (!wells.Contains(PI.Intake.well.ID))
              wells.Add(PI.Intake.well);
        }
        return wells;
      }
    }

    private JupiterWell[] wellsForWeb;

    [DataMember]
    public JupiterWell[] WellsForWeb
    {
      get
      {
        if (wellsForWeb == null)
        {
          List<JupiterWell> localwells = new List<JupiterWell>();
          foreach (IWell w in PumpingWells)
          {
            if (w is JupiterWell)
              localwells.Add(w as JupiterWell);
            else
              localwells.Add(new JupiterWell(w.ID, w.X, w.Y));

          }
          wellsForWeb = localwells.ToArray();
        }
        return wellsForWeb;
      }
    }


    private TimestampValue[] extractionForWeb;

    [DataMember]
    public TimestampValue[] ExtractionForWeb
    {
      get
      {
        if (extractionForWeb ==null)
          extractionForWeb = Extractions.AsTimeStamps.OrderBy(t=>t.Time).ToArray();
        return extractionForWeb;
      }
    }

    [DataMember]
    public ChemistrySample[] Chemistry { get; set; }
    
     


    public List<Plant> SubPlants { get; private set; }

    /// <summary>
    /// The name of the plant
    /// </summary>
    [DataMember]
    public string Name { get; set; }

    /// <summary>
    /// The street where the plant is located
    /// </summary>
    [DataMember]
    public string Address { get; set; }

    /// <summary>
    /// The postal code
    /// </summary>
    [DataMember]
    public int PostalCode { get; set; }

    /// <summary>
    /// The permit was given at this date
    /// </summary>
    [DataMember]
    public DateTime PermitDate { get; set; }

    /// <summary>
    /// The date at which this permit expires
    /// </summary>
    [DataMember]
    public DateTime PermitExpiryDate { get; set; }

    /// <summary>
    /// The yearly permit in m3
    /// </summary>
    [DataMember]
    public double Permit { get; set; }

    /// <summary>
    /// Gets and sets the commune number after 2007
    /// </summary>
    [DataMember]
    public int NewCommuneNumber { get; set; }

    /// <summary>
    /// Gets and sets the commune number before 2007
    /// </summary>
    [DataMember]
    public int OldCommuneNumber { get; set; }

    /// <summary>
    /// Integer indicating the activity of the plant
    /// </summary>
    [DataMember]
    public int Active { get; set; }

    /// <summary>
    /// The company type.
    /// </summary>
    [DataMember]
    public string CompanyType { get; set; }



    public int? SuperiorPlantNumber { get; set; }

    #endregion

    public Plant(int IDNumber)
    {
      Extractions = new TimespanSeries();

      PumpingIntakes = new BindingList<PumpingIntake>();

      PumpingIntakes.ListChanged += new ListChangedEventHandler(PumpingIntakes_ListChanged);
      
      SurfaceWaterExtrations = new TimespanSeries();
      SubPlants = new List<Plant>();
      this.IDNumber = IDNumber;
    }

    [OnDeserialized]
    public void Contruct(StreamingContext context)
    {
      Extractions = new TimespanSeries();
      PumpingIntakes = new BindingList<PumpingIntake>();

    }


    /// <summary>
    /// Distributes the extractions evenly on the active intakes
    /// </summary>
    public void DistributeExtraction(bool clearFirst)
    {
      Extractions.Sort();

      //The function to determine if an intake is active
      //The well should be a pumping well and start and end date should cover the year
      Func<PumpingIntake, int, bool> IsActive = new Func<PumpingIntake, int, bool>((var, var2) => var.Intake.well.UsedForExtraction & (var.StartNullable ?? DateTime.MinValue).Year <= var2 & (var.EndNullable ?? DateTime.MaxValue).Year >= var2);

      double[] fractions = new double[Extractions.Items.Count()];

      //Calculate the fractions based on how many intakes are active for a particular year.
      for (int i = 0; i < Extractions.Items.Count(); i++)
      {
        int CurrentYear = Extractions.Items[i].StartTime.Year;
        fractions[i] = 1.0 / PumpingIntakes.Count(var => IsActive(var, CurrentYear));
      }

      //Now loop the extraction values
      for (int i = 0; i < Extractions.Items.Count(); i++)
      {
        TimespanValue tsv = new TimespanValue(Extractions.Items[i].StartTime, Extractions.Items[i].EndTime, Extractions.Items[i].Value * fractions[i]);

        //Now loop the intakes
        foreach (PumpingIntake PI in PumpingIntakes)
        {
          IIntake I = PI.Intake;
          if (clearFirst)
            I.Extractions.Items.Clear();
          //Is it an extraction well?
          if (IsActive(PI, Extractions.Items[i].StartTime.Year))
          {
            I.Extractions.AddValue(tsv.StartTime, tsv.EndTime, tsv.Value);
          }
        }
      }
    }

    void PumpingIntakes_ListChanged(object sender, ListChangedEventArgs e)
    {
      PumpingIntakesChanged = true;
    }


    public override string ToString()
    {
      if (Name != null)
        return Name;
      else
        return IDNumber.ToString();
    }

    #region IComparable<Plant> Members

    /// <summary>
    /// Compares the name
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Plant other)
    {
      return Name.CompareTo(other.Name);
    }


    public override bool Equals(object obj)
    {
      return IDNumber.Equals(((Plant)obj).IDNumber);
    }

    public override int GetHashCode()
    {
      return IDNumber.GetHashCode();
    }

    #endregion
  }
}
