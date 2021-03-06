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
  public partial class CALIBRATION_SPECIFICATION: PFSMapper
  {


    internal CALIBRATION_SPECIFICATION(PFSSection Section)
    {
      _pfsHandle = Section;

      MODEL_As = new List<MOUSE_Catchments>();
      for (int i = 1; i <= Section.GetSectionsNo(); i++)
      {
        PFSSection sub = Section.GetSection(i);
        switch (sub.Name)
        {
        case "GLOBAL_PARAMETERS":
          GLOBAL_PARAMETERS = new GLOBAL_PARAMETERS(sub);
          break;
        case "MEASUREMENTS":
          MEASUREMENTS = new Model_B(sub);
          break;
          default:
            if (sub.Name.Substring(0,6).Equals("MODEL_"))
            {
              MODEL_As.Add(new MOUSE_Catchments(sub));
              break;
            }
            _unMappedSections.Add(sub.Name);
          break;
        }
      }

    }

    public CALIBRATION_SPECIFICATION(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      MODEL_As = new List<MOUSE_Catchments>();
      GLOBAL_PARAMETERS = new GLOBAL_PARAMETERS("GLOBAL_PARAMETERS" );
      _pfsHandle.AddSection(GLOBAL_PARAMETERS._pfsHandle);

      MEASUREMENTS = new Model_B("MEASUREMENTS" );
      _pfsHandle.AddSection(MEASUREMENTS._pfsHandle);

    }

    public GLOBAL_PARAMETERS GLOBAL_PARAMETERS{get; private set;}

    public Model_B MEASUREMENTS{get; private set;}

    public List<MOUSE_Catchments> MODEL_As {get; private set;}

  }
}
