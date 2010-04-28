﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using HydroNumerics.Time.Core;
using SharpMap.Geometries;

namespace HydroNumerics.HydroNet.Core
{
  [DataContract]
  public abstract class AbstractWaterBody:IHasTSOuput
  {
    #region Persisted data
    [DataMember]
    protected List<IWaterBody> _downStreamConnections = new List<IWaterBody>();

    [DataMember]
    protected List<IWaterSinkSource> _sinkSources = new List<IWaterSinkSource>();

    [DataMember]
    protected List<IEvaporationBoundary> _evapoBoundaries = new List<IEvaporationBoundary>();

    [DataMember]
    public int ID { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public double Volume { get; set; }

    [DataMember]
    public WaterBodyOutput Output { get; protected set; }

    /// <summary>
    /// Gets and sets the Water level
    /// </summary>
    [DataMember]
    public double WaterLevel { get; set; }

    #endregion

    #region Non-persisted Properties

    public DateTime CurrentStartTime { get; protected set; }

    /// <summary>
    /// Gets the collection of sinks and sources
    /// </summary>
    [DataMember]

    public Collection<IWaterSinkSource> SinkSources { get; protected set; }

    /// <summary>
    /// Gets the collection of downstream connections
    /// </summary>
    [DataMember]
    public Collection<IWaterBody> DownStreamConnections { get; protected set; }

    /// <summary>
    /// Gets the collection og evaporation boundaries
    /// </summary>
    [DataMember]
    public Collection<IEvaporationBoundary> EvaporationBoundaries { get; protected set; }

    #endregion

    #region Constructors


    public AbstractWaterBody()
    {
      Volume = 0;
      Output = new WaterBodyOutput(ID.ToString());
      SinkSources = new Collection<IWaterSinkSource>(_sinkSources);
      DownStreamConnections = new Collection<IWaterBody>(_downStreamConnections);
      EvaporationBoundaries = new Collection<IEvaporationBoundary>(_evapoBoundaries);

    }

    /// <summary>
    /// Use this constructor to create an empty lake
    /// </summary>
    /// <param name="VolumeOfLakeWater"></param>
    public AbstractWaterBody(double VolumeOfLakeWater)
      : this()
    {
      Volume = VolumeOfLakeWater;
    }


    #endregion

    /// <summary>
    /// Distributes water on downstream connections. Also logs chemical concentrations
    /// </summary>
    /// <param name="Water"></param>
    /// <param name="Start"></param>
    /// <param name="End"></param>
    protected void SendWaterDownstream(IWaterPacket Water, DateTime Start, DateTime End)
    {
      if(Water.GetType().Equals(typeof(WaterWithChemicals)))
        foreach (KeyValuePair<Chemical, TimeSeries> ct in Output.ChemicalsToLog)
        {
          ct.Value.AddTimeValueRecord(new TimeValue(Start, ((WaterWithChemicals)Water).GetConcentration(ct.Key)));
        }

      //Send water to downstream recipients
      if (_downStreamConnections.Count == 1)
        _downStreamConnections[0].ReceiveWater(Start, End, Water);
      else if (_downStreamConnections.Count > 1)
      {
        double fraction = Water.Volume/_downStreamConnections.Count;
        foreach (IWaterBody IW in _downStreamConnections)
          IW.ReceiveWater(CurrentStartTime, End, Water.Substract(fraction));
      }

    }
  }
}
