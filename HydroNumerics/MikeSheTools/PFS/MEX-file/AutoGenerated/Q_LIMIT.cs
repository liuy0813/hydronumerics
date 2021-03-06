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
  public partial class Q_LIMIT
  {

    internal PFSKeyword _keyword;

    internal Q_LIMIT(PFSKeyword keyword)
    {
       _keyword = keyword;
    }

    public Q_LIMIT(string keywordname)
    {
       _keyword = new PFSKeyword(keywordname);
       _keyword.AddParameter(new PFSParameter(PFSParameterType.Double, 0));
       _keyword.AddParameter(new PFSParameter(PFSParameterType.String, ""));
    }
    public double Par1
    {
      get { return _keyword.GetParameter(1).ToDouble();}
      set { _keyword.GetParameter(1).Value = value;}
    }

    public string Par2
    {
      get { return _keyword.GetParameter(2).ToString();}
      set { _keyword.GetParameter(2).Value = value;}
    }

  }
}
