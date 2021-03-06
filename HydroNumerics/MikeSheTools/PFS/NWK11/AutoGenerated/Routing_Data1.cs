using System;
using System.Collections.Generic;
using DHI.Generic.MikeZero;
using HydroNumerics.MikeSheTools.PFS.SheFile;

namespace HydroNumerics.MikeSheTools.PFS.NWK11
{
  /// <summary>
  /// This is an autogenerated class. Do not edit. 
  /// If you want to add methods create a new partial class in another file
  /// </summary>
  public partial class Routing_Data1: PFSMapper
  {


    internal Routing_Data1(PFSSection Section)
    {
      _pfsHandle = Section;

      for (int i = 1; i <= Section.GetSectionsNo(); i++)
      {
        PFSSection sub = Section.GetSection(i);
        switch (sub.Name)
        {
        case "Elevation_Parameters":
          Elevation_Parameters = new Elevation_Parameters1(sub);
          break;
        case "Discharge_Parameters":
          Discharge_Parameters = new Discharge_Parameters(sub);
          break;
          default:
            _unMappedSections.Add(sub.Name);
          break;
        }
      }

      Location = new Location(_pfsHandle.GetKeyword("Location", 1));
      Attributes = new Attributes(_pfsHandle.GetKeyword("Attributes", 1));
    }

    public Routing_Data1(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      Location = new Location("Location");
      _pfsHandle.AddKeyword(Location._keyword);
      Attributes = new Attributes("Attributes");
      _pfsHandle.AddKeyword(Attributes._keyword);
      Elevation_Parameters = new Elevation_Parameters1("Elevation_Parameters" );
      _pfsHandle.AddSection(Elevation_Parameters._pfsHandle);

      Discharge_Parameters = new Discharge_Parameters("Discharge_Parameters" );
      _pfsHandle.AddSection(Discharge_Parameters._pfsHandle);

    }

    public Elevation_Parameters1 Elevation_Parameters{get; private set;}

    public Discharge_Parameters Discharge_Parameters{get; private set;}

    public Location Location{get; private set;}
    public Attributes Attributes{get; private set;}
  }
}
