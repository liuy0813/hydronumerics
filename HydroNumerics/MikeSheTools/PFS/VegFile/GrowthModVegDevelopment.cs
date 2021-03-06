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
  public partial class GrowthModVegDevelopment: PFSMapper
  {

    private List<Stage_11> _stage_1s = new List<Stage_11>();

    internal GrowthModVegDevelopment(PFSSection Section)
    {
      _pfsHandle = Section;

      for (int i = 1; i <= Section.GetSectionsNo(); i++)
      {
        PFSSection sub = Section.GetSection(i);
        switch (sub.Name)
        {
          default:
            if (sub.Name.Substring(0,6).Equals("Stage_"))
            {
              _stage_1s.Add(new Stage_11(sub));
              break;
            }
            _unMappedSections.Add(sub.Name);
          break;
        }
      }
    }

    public List<Stage_11> Stage_1s
    {
     get { return _stage_1s; }
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

    public int MzSEPfsListItemCount
    {
      get
      {
        return _pfsHandle.GetKeyword("MzSEPfsListItemCount", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("MzSEPfsListItemCount", 1).GetParameter(1).Value = value;
      }
    }

    public int NO_ITEM
    {
      get
      {
        return _pfsHandle.GetKeyword("NO_ITEM", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("NO_ITEM", 1).GetParameter(1).Value = value;
      }
    }

    public int CALC_PARAMETERS
    {
      get
      {
        return _pfsHandle.GetKeyword("CALC_PARAMETERS", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("CALC_PARAMETERS", 1).GetParameter(1).Value = value;
      }
    }

    public int GROWTH_RATE
    {
      get
      {
        return _pfsHandle.GetKeyword("GROWTH_RATE", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("GROWTH_RATE", 1).GetParameter(1).Value = value;
      }
    }

    public int LAI_DAMPING_D
    {
      get
      {
        return _pfsHandle.GetKeyword("LAI_DAMPING_D", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("LAI_DAMPING_D", 1).GetParameter(1).Value = value;
      }
    }

    public int LAI_DAMPING_A
    {
      get
      {
        return _pfsHandle.GetKeyword("LAI_DAMPING_A", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("LAI_DAMPING_A", 1).GetParameter(1).Value = value;
      }
    }

    public int MAX_CROP_YIELD
    {
      get
      {
        return _pfsHandle.GetKeyword("MAX_CROP_YIELD", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("MAX_CROP_YIELD", 1).GetParameter(1).Value = value;
      }
    }

    public int MAX_LAI
    {
      get
      {
        return _pfsHandle.GetKeyword("MAX_LAI", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("MAX_LAI", 1).GetParameter(1).Value = value;
      }
    }

    public int MAX_ROOT_DEPTH
    {
      get
      {
        return _pfsHandle.GetKeyword("MAX_ROOT_DEPTH", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("MAX_ROOT_DEPTH", 1).GetParameter(1).Value = value;
      }
    }

    public int SENESENSE
    {
      get
      {
        return _pfsHandle.GetKeyword("SENESENSE", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("SENESENSE", 1).GetParameter(1).Value = value;
      }
    }

    public int SPECIFIC_LAI
    {
      get
      {
        return _pfsHandle.GetKeyword("SPECIFIC_LAI", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("SPECIFIC_LAI", 1).GetParameter(1).Value = value;
      }
    }

    public int TIME_TO_MAX_LAI
    {
      get
      {
        return _pfsHandle.GetKeyword("TIME_TO_MAX_LAI", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("TIME_TO_MAX_LAI", 1).GetParameter(1).Value = value;
      }
    }

  }
}
