using System;
using System.Collections.Generic;
using DHI.Generic.MikeZero;
using HydroNumerics.MikeSheTools.PFS.SheFile;

namespace HydroNumerics.MikeSheTools.PFS.MEX
{
  /// <summary>
  /// This is an autogenerated class. Do not edit. 
  /// If you want to add methods create a new partial class in another file
  /// </summary>
  public partial class MOUSE_PUMPS: PFSMapper
  {


    internal MOUSE_PUMPS(PFSSection Section)
    {
      _pfsHandle = Section;

      Pumps = new List<Pump>();
      for (int i = 1; i <= Section.GetKeywordsNo("Pump"); i++)
        Pumps.Add(new Pump(Section.GetKeyword("Pump",i)));
      for (int i = 1; i <= Section.GetSectionsNo(); i++)
      {
        PFSSection sub = Section.GetSection(i);
        switch (sub.Name)
        {
          default:
            _unMappedSections.Add(sub.Name);
          break;
        }
      }

      PumpHeader = new PumpHeader(_pfsHandle.GetKeyword("PumpHeader", 1));
    }

    public MOUSE_PUMPS(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      Pumps = new List<Pump>();
      _pfsHandle.AddKeyword(new PFSKeyword("SYNTAX_VERSION", PFSParameterType.Integer, 0));

      _pfsHandle.AddKeyword(new PFSKeyword("UNIT_TYPE", PFSParameterType.Integer, 0));

      PumpHeader = new PumpHeader("PumpHeader");
      _pfsHandle.AddKeyword(PumpHeader._keyword);
    }

    public PumpHeader PumpHeader{get; private set;}
    public List<Pump> Pumps {get; private set;}
    public int SYNTAX_VERSION
    {
      get
      {
        return _pfsHandle.GetKeyword("SYNTAX_VERSION", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("SYNTAX_VERSION", 1).GetParameter(1).Value = value;
      }
    }

    public int UNIT_TYPE
    {
      get
      {
        return _pfsHandle.GetKeyword("UNIT_TYPE", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("UNIT_TYPE", 1).GetParameter(1).Value = value;
      }
    }

  }
}
