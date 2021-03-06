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
  public partial class SOIL: PFSMapper
  {


    internal SOIL(PFSSection Section)
    {
      _pfsHandle = Section;

      for (int i = 1; i <= Section.GetSectionsNo(); i++)
      {
        PFSSection sub = Section.GetSection(i);
        switch (sub.Name)
        {
        case "DEFAULT_DATA":
          DEFAULT_DATA = new DEFAULT_DATA(sub);
          break;
          default:
            _unMappedSections.Add(sub.Name);
          break;
        }
      }

    }

    public SOIL(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      DEFAULT_DATA = new DEFAULT_DATA("DEFAULT_DATA" );
      _pfsHandle.AddSection(DEFAULT_DATA._pfsHandle);

    }

    public DEFAULT_DATA DEFAULT_DATA{get; private set;}

  }
}
