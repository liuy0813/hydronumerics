using System;
using System.Collections.Generic;
using DHI.Generic.MikeZero;
using HydroNumerics.MikeSheTools.PFS.SheFile;

namespace HydroNumerics.MikeSheTools.PFS.VegetationFile
{
  /// <summary>
  /// This is an autogenerated class. Do not edit. 
  /// If you want to add methods create a new partial class in another file
  /// </summary>
  public partial class Stage_13: PFSMapper
  {


    internal Stage_13(PFSSection Section)
    {
      _pfsHandle = Section;

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
    }

    public int Touched
    {
      get
      {
        return _pfsHandle.GetKeyword("Touched", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("Touched", 1).GetParameter(1).Value = value;
      }
    }

    public string STAGE_NAME
    {
      get
      {
        return _pfsHandle.GetKeyword("STAGE_NAME", 1).GetParameter(1).ToString();
      }
      set
      {
        _pfsHandle.GetKeyword("STAGE_NAME", 1).GetParameter(1).Value = value;
      }
    }

    public int END_DAY
    {
      get
      {
        return _pfsHandle.GetKeyword("END_DAY", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("END_DAY", 1).GetParameter(1).Value = value;
      }
    }

  }
}
