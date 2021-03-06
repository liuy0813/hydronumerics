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
  public partial class msm_SWQLocTreat_Coeff: PFSMapper
  {


    internal msm_SWQLocTreat_Coeff(PFSSection Section)
    {
      _pfsHandle = Section;

      for (int i = 1; i <= Section.GetSectionsNo(); i++)
      {
        PFSSection sub = Section.GetSection(i);
        switch (sub.Name)
        {
        case "Metadata":
          Metadata = new Metadata(sub);
          break;
        case "Rows":
          Rows = new Rows1(sub);
          break;
          default:
            _unMappedSections.Add(sub.Name);
          break;
        }
      }

    }

    public msm_SWQLocTreat_Coeff(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      Metadata = new Metadata("Metadata" );
      _pfsHandle.AddSection(Metadata._pfsHandle);

      Rows = new Rows1("Rows" );
      _pfsHandle.AddSection(Rows._pfsHandle);

    }

    public Metadata Metadata{get; private set;}

    public Rows1 Rows{get; private set;}

  }
}
