﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

using HydroNumerics.Core;
using HydroNumerics.MikeSheTools.DFS;
using HydroNumerics.Geometry.Net;
using HydroNumerics.Geometry;


namespace HydroNumerics.MikeSheTools.Mike11
{

  public enum SourceType
  {
    Oracle,
    KMSWeb,
    HydroInform,
    DFS2
  }


  public class DEMSourceConfiguration:BaseViewModel
  {
    public OracleConnector Oracle {get;private set;}
    private SourceType _st;
    private string _dfs2File = "";

    private DFS2 DFSdem;


    public DEMSourceConfiguration()
    {
      Oracle = new OracleConnector("geusjup3", 1685, "FPH.DKDHM10", "mike11cs", "mike11cspw", "jupiter");
      _st = SourceType.HydroInform;
    }


    private Logger.LoggerDataClient ldc;

    private Logger.LoggerDataClient LDC
  {
      get
      {
        if (ldc == null)
        {
          BinaryMessageEncodingBindingElement binaryMessageEncoding = new BinaryMessageEncodingBindingElement();
          HttpTransportBindingElement httpTransport = new HttpTransportBindingElement() { MaxBufferSize = int.MaxValue, MaxReceivedMessageSize = int.MaxValue };

          // add the binding elements into a Custom Binding
          CustomBinding customBinding = new CustomBinding(binaryMessageEncoding, httpTransport);
          customBinding.Name = "CustomBinding_LoggerData";
          EndpointAddress endpointAddress = new EndpointAddress("http://hydroinform.cloudapp.net/loggerData.svc");
          ldc = new Logger.LoggerDataClient(customBinding, endpointAddress);
        }
        return ldc;
      }
    }

    public string Dfs2File
    {
      get { return _dfs2File; }
      set
      {
        if (value != _dfs2File)
        {
          _dfs2File = value;
          RaisePropertyChanged("Dfs2File");
        }
      }
    }


    public void LoadDfs2(string FileName)
    {
      _st = SourceType.DFS2;
      Dfs2File  = FileName;
      DFSdem = new DFS2(_dfs2File);

    }

    public bool TryFindDemHeight(double x, double y, out double? height)
    {
      return TryFindDemHeight(new XYPoint(x, y), out height);
    }

    public List<double?> FindManyHeights(IEnumerable<XYPoint> points)
    {
      if (DEMSource == SourceType.HydroInform)
      {
        return LDC.GetHeights(points.Select(p => new Logger.Coordinate() { Latitude = p.Latitude, Longitude = p.Longitude }).ToArray()).Select(c => c.Height).ToList();
      }
      else
      {
        return points.Select(p => FindHeight(p)).ToList();
      }
    }

    private double? FindHeight(IXYPoint point)
    {
      double? height;
      TryFindDemHeight(point, out height);
      return height;
    }

    /// <summary>
    /// Returns the height at the point using the method selected with the enums
    /// </summary>
    /// <param name="point"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public bool TryFindDemHeight(IXYPoint point, out double? height)
    {
      height = null;

      switch (DEMSource)
      {
        case SourceType.Oracle:
          return Oracle.TryGetHeight(point, out height);
        case SourceType.KMSWeb:
          return KMSData.TryGetHeight(point, 32, out height);
        case SourceType.DFS2:
          int col = DFSdem.GetColumnIndex(point.X);
          int row = DFSdem.GetRowIndex(point.Y);
          if (col >= 0 & row >= 0)
          {
            height = DFSdem.GetData(0, 1)[row, col];
            return true;
          }
          else
            return false;
        default:
          return false;
        case SourceType.HydroInform:
          {
            XYPoint p = point as XYPoint;
            if (LDC.State == System.ServiceModel.CommunicationState.Faulted)
              return false;
            var d = LDC.GetHeight(p.Latitude, p.Longitude);
            if (d.HasValue)
              height = d.Value;
            return d.HasValue;
          }
      }
      return false;
    }



    public string Password
    {
      get { return Oracle.Password; }
      set { 
        Oracle.Password = value;
        Oracle.BuildConnectionString();
        RaisePropertyChanged("ConnectionString");
      }
    }

    public string UserName
    {
      get { return Oracle.UserName; }
      set { Oracle.UserName = value;
      Oracle.BuildConnectionString();
      RaisePropertyChanged("ConnectionString");
      }
    }
  

    public string TableName
    {
      get { return Oracle.TableName; }
      set { Oracle.TableName = value; }
    }

    /// <summary>
    /// Get and set the Connectionstring
    /// </summary>
    public string ConnectionString
    {
      get { return Oracle.ConnectionString; }
      set
      {
        if (value != Oracle.ConnectionString)
        {
          Oracle.ConnectionString = value;
          RaisePropertyChanged("ConnectionString");
        }
      }
    }

    
    public string OracleServerName
    {
      get { return Oracle.ServerName; }
      set
      {
        if (value != Oracle.ServerName)
        {

          Oracle.ServerName = value;
          RaisePropertyChanged("OracleServerName");
          Oracle.BuildConnectionString();
          RaisePropertyChanged("ConnectionString");

        }
      }
    }

    public int PortNumber
    {
      get { return Oracle.PortNumber; }
      set { Oracle.PortNumber = value;
      Oracle.BuildConnectionString();
      RaisePropertyChanged("ConnectionString");
      }
    }

    public string DatabaseName
    {
      get { return Oracle.DatabaseName; }
      set
      {
        if (value != Oracle.DatabaseName)
        {

          Oracle.DatabaseName = value;
          RaisePropertyChanged("DatabaseName");
          Oracle.BuildConnectionString();
          RaisePropertyChanged("ConnectionString");
        }
      }
    }

    
    /// <summary>
    /// Gets and sets source type
    /// </summary>
    public SourceType DEMSource
    {
      get
      {
        return _st;
      }
      set
      {
        if (value!=_st)
        {
          _st =value;
          RaisePropertyChanged("DEMSource");
        }
      }
    }

  }
}
