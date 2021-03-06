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
  public partial class MOUSE_HD_parameters: PFSMapper
  {


    internal MOUSE_HD_parameters(PFSSection Section)
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

      Model_type = new Model_type(_pfsHandle.GetKeyword("Model_type", 1));
      TRAP_Setup = new TRAP_Setup(_pfsHandle.GetKeyword("TRAP_Setup", 1));
      SaveStep_HrMiSec = new SaveStep_HrMiSec(_pfsHandle.GetKeyword("SaveStep_HrMiSec", 1));
      HD_summary = new HD_summary(_pfsHandle.GetKeyword("HD_summary", 1));
    }

    public MOUSE_HD_parameters(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      _pfsHandle.AddKeyword(new PFSKeyword("RTC_Computation", PFSParameterType.Boolean, true));

      _pfsHandle.AddKeyword(new PFSKeyword("UWC_Computation", PFSParameterType.Boolean, true));

      _pfsHandle.AddKeyword(new PFSKeyword("TRAP_Computation", PFSParameterType.Boolean, true));

      _pfsHandle.AddKeyword(new PFSKeyword("Simulation_start", PFSParameterType.String, ""));

      _pfsHandle.AddKeyword(new PFSKeyword("Simulation_end", PFSParameterType.String, ""));

      _pfsHandle.AddKeyword(new PFSKeyword("Dt_MaxSec", PFSParameterType.Integer, 0));

      _pfsHandle.AddKeyword(new PFSKeyword("Dt_MinSec", PFSParameterType.Integer, 0));

      _pfsHandle.AddKeyword(new PFSKeyword("Dt_IncreaseFactor", PFSParameterType.Double, 0));

      _pfsHandle.AddKeyword(new PFSKeyword("HotStart_DateTime", PFSParameterType.String, ""));

      _pfsHandle.AddKeyword(new PFSKeyword("Allow_OverWrite", PFSParameterType.Boolean, true));

      _pfsHandle.AddKeyword(new PFSKeyword("TRAP_AD_Hot", PFSParameterType.Boolean, true));

      _pfsHandle.AddKeyword(new PFSKeyword("HD_summary_NSE", PFSParameterType.String, ""));

      _pfsHandle.AddKeyword(new PFSKeyword("HD_summary_LSE", PFSParameterType.String, ""));

      Model_type = new Model_type("Model_type");
      _pfsHandle.AddKeyword(Model_type._keyword);
      TRAP_Setup = new TRAP_Setup("TRAP_Setup");
      _pfsHandle.AddKeyword(TRAP_Setup._keyword);
      SaveStep_HrMiSec = new SaveStep_HrMiSec("SaveStep_HrMiSec");
      _pfsHandle.AddKeyword(SaveStep_HrMiSec._keyword);
      HD_summary = new HD_summary("HD_summary");
      _pfsHandle.AddKeyword(HD_summary._keyword);
    }

    public Model_type Model_type{get; private set;}
    public TRAP_Setup TRAP_Setup{get; private set;}
    public SaveStep_HrMiSec SaveStep_HrMiSec{get; private set;}
    public HD_summary HD_summary{get; private set;}
    public bool RTC_Computation
    {
      get
      {
        return _pfsHandle.GetKeyword("RTC_Computation", 1).GetParameter(1).ToBoolean();
      }
      set
      {
        _pfsHandle.GetKeyword("RTC_Computation", 1).GetParameter(1).Value = value;
      }
    }

    public bool UWC_Computation
    {
      get
      {
        return _pfsHandle.GetKeyword("UWC_Computation", 1).GetParameter(1).ToBoolean();
      }
      set
      {
        _pfsHandle.GetKeyword("UWC_Computation", 1).GetParameter(1).Value = value;
      }
    }

    public bool TRAP_Computation
    {
      get
      {
        return _pfsHandle.GetKeyword("TRAP_Computation", 1).GetParameter(1).ToBoolean();
      }
      set
      {
        _pfsHandle.GetKeyword("TRAP_Computation", 1).GetParameter(1).Value = value;
      }
    }

    public string Simulation_start
    {
      get
      {
        return _pfsHandle.GetKeyword("Simulation_start", 1).GetParameter(1).ToString();
      }
      set
      {
        _pfsHandle.GetKeyword("Simulation_start", 1).GetParameter(1).Value = value;
      }
    }

    public string Simulation_end
    {
      get
      {
        return _pfsHandle.GetKeyword("Simulation_end", 1).GetParameter(1).ToString();
      }
      set
      {
        _pfsHandle.GetKeyword("Simulation_end", 1).GetParameter(1).Value = value;
      }
    }

    public int Dt_MaxSec
    {
      get
      {
        return _pfsHandle.GetKeyword("Dt_MaxSec", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("Dt_MaxSec", 1).GetParameter(1).Value = value;
      }
    }

    public int Dt_MinSec
    {
      get
      {
        return _pfsHandle.GetKeyword("Dt_MinSec", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("Dt_MinSec", 1).GetParameter(1).Value = value;
      }
    }

    public double Dt_IncreaseFactor
    {
      get
      {
        return _pfsHandle.GetKeyword("Dt_IncreaseFactor", 1).GetParameter(1).ToDouble();
      }
      set
      {
        _pfsHandle.GetKeyword("Dt_IncreaseFactor", 1).GetParameter(1).Value = value;
      }
    }

    public string HotStart_DateTime
    {
      get
      {
        return _pfsHandle.GetKeyword("HotStart_DateTime", 1).GetParameter(1).ToString();
      }
      set
      {
        _pfsHandle.GetKeyword("HotStart_DateTime", 1).GetParameter(1).Value = value;
      }
    }

    public bool Allow_OverWrite
    {
      get
      {
        return _pfsHandle.GetKeyword("Allow_OverWrite", 1).GetParameter(1).ToBoolean();
      }
      set
      {
        _pfsHandle.GetKeyword("Allow_OverWrite", 1).GetParameter(1).Value = value;
      }
    }

    public bool TRAP_AD_Hot
    {
      get
      {
        return _pfsHandle.GetKeyword("TRAP_AD_Hot", 1).GetParameter(1).ToBoolean();
      }
      set
      {
        _pfsHandle.GetKeyword("TRAP_AD_Hot", 1).GetParameter(1).Value = value;
      }
    }

    public string HD_summary_NSE
    {
      get
      {
        return _pfsHandle.GetKeyword("HD_summary_NSE", 1).GetParameter(1).ToString();
      }
      set
      {
        _pfsHandle.GetKeyword("HD_summary_NSE", 1).GetParameter(1).Value = value;
      }
    }

    public string HD_summary_LSE
    {
      get
      {
        return _pfsHandle.GetKeyword("HD_summary_LSE", 1).GetParameter(1).ToString();
      }
      set
      {
        _pfsHandle.GetKeyword("HD_summary_LSE", 1).GetParameter(1).Value = value;
      }
    }

  }
}
