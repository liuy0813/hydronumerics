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
  public partial class AND_LINE
  {

    internal PFSKeyword _keyword;

    internal AND_LINE(PFSKeyword keyword)
    {
       _keyword = keyword;
    }

    public AND_LINE(string keywordname)
    {
       _keyword = new PFSKeyword(keywordname);
       _keyword.AddParameter(new PFSParameter(PFSParameterType.String, ""));
       _keyword.AddParameter(new PFSParameter(PFSParameterType.Missing, ""));
       _keyword.AddParameter(new PFSParameter(PFSParameterType.Missing, ""));
       _keyword.AddParameter(new PFSParameter(PFSParameterType.Missing, ""));
       _keyword.AddParameter(new PFSParameter(PFSParameterType.Missing, ""));
    }
    public string Par1
    {
      get { return _keyword.GetParameter(1).ToString();}
      set { _keyword.GetParameter(1).Value = value;}
    }

    public string Par2
    {
      get { return _keyword.GetParameter(2).ToString();}
      set { _keyword.GetParameter(2).Value = value;}
    }

    public string Par3
    {
      get { return _keyword.GetParameter(3).ToString();}
      set { _keyword.GetParameter(3).Value = value;}
    }

    public string Par4
    {
      get { return _keyword.GetParameter(4).ToString();}
      set { _keyword.GetParameter(4).Value = value;}
    }

    public string Par5
    {
      get { return _keyword.GetParameter(5).ToString();}
      set { _keyword.GetParameter(5).Value = value;}
    }

  }
}
