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
  public partial class MAP: PFSMapper
  {


    internal MAP(PFSSection Section)
    {
      _pfsHandle = Section;

      for (int i = 1; i <= Section.GetSectionsNo(); i++)
      {
        PFSSection sub = Section.GetSection(i);
        switch (sub.Name)
        {
        case "GEOMETRY":
          GEOMETRY = new GEOMETRY(sub);
          break;
          default:
            _unMappedSections.Add(sub.Name);
          break;
        }
      }

    }

    public MAP(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      GEOMETRY = new GEOMETRY("GEOMETRY" );
      _pfsHandle.AddSection(GEOMETRY._pfsHandle);

    }

    public GEOMETRY GEOMETRY{get; private set;}

  }
}
