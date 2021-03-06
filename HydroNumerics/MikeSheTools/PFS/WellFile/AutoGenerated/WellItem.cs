using System;
using System.Collections.Generic;
using DHI.Generic.MikeZero;
using HydroNumerics.MikeSheTools.PFS.SheFile;

namespace HydroNumerics.MikeSheTools.PFS.Well
{
  /// <summary>
  /// This is an autogenerated class. Do not edit. 
  /// If you want to add methods create a new partial class in another file
  /// </summary>
  public partial class WELLNO_1: PFSMapper
  {


    internal WELLNO_1(PFSSection Section)
    {
      _pfsHandle = Section;

      for (int i = 1; i <= Section.GetSectionsNo(); i++)
      {
        PFSSection sub = Section.GetSection(i);
        switch (sub.Name)
        {
        case "FILTERDATA":
          FILTERDATA = new FILTERDATA1(sub);
          break;
        case "LITOGRAFIDATA":
          LITOGRAFIDATA = new LITOGRAFIDATA(sub);
          break;
          default:
            _unMappedSections.Add(sub.Name);
          break;
        }
      }

    }

    public WELLNO_1(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      _pfsHandle.AddKeyword(new PFSKeyword("ID", PFSParameterType.String, ""));

      _pfsHandle.AddKeyword(new PFSKeyword("XCOR", PFSParameterType.Double, 0));

      _pfsHandle.AddKeyword(new PFSKeyword("YCOR", PFSParameterType.Double, 0));

      _pfsHandle.AddKeyword(new PFSKeyword("LEVEL", PFSParameterType.Integer, 0));

      _pfsHandle.AddKeyword(new PFSKeyword("WELLDEPTH1", PFSParameterType.Integer, 0));

      _pfsHandle.AddKeyword(new PFSKeyword("WELL_FIELD_ID", PFSParameterType.Integer, 0));

      FILTERDATA = new FILTERDATA1("FILTERDATA" );
      _pfsHandle.AddSection(FILTERDATA._pfsHandle);

      LITOGRAFIDATA = new LITOGRAFIDATA("LITOGRAFIDATA" );
      _pfsHandle.AddSection(LITOGRAFIDATA._pfsHandle);

    }

    public FILTERDATA1 FILTERDATA{get; private set;}

    public LITOGRAFIDATA LITOGRAFIDATA{get; private set;}

    public string ID
    {
      get
      {
        return _pfsHandle.GetKeyword("ID", 1).GetParameter(1).ToString();
      }
      set
      {
        _pfsHandle.GetKeyword("ID", 1).GetParameter(1).Value = value;
      }
    }

    public double XCOR
    {
      get
      {
        return _pfsHandle.GetKeyword("XCOR", 1).GetParameter(1).ToDouble();
      }
      set
      {
        _pfsHandle.GetKeyword("XCOR", 1).GetParameter(1).Value = value;
      }
    }

    public double YCOR
    {
      get
      {
        return _pfsHandle.GetKeyword("YCOR", 1).GetParameter(1).ToDouble();
      }
      set
      {
        _pfsHandle.GetKeyword("YCOR", 1).GetParameter(1).Value = value;
      }
    }

    public double LEVEL
    {
      get
      {
        return _pfsHandle.GetKeyword("LEVEL", 1).GetParameter(1).ToDouble();
      }
      set
      {
        _pfsHandle.GetKeyword("LEVEL", 1).GetParameter(1).Value = value;
      }
    }

    public int WELLDEPTH1
    {
      get
      {
        return _pfsHandle.GetKeyword("WELLDEPTH1", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("WELLDEPTH1", 1).GetParameter(1).Value = value;
      }
    }

    public int WELL_FIELD_ID
    {
      get
      {
        return _pfsHandle.GetKeyword("WELL_FIELD_ID", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("WELL_FIELD_ID", 1).GetParameter(1).Value = value;
      }
    }

  }
}
