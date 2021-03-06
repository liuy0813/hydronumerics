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
  public partial class Rows1: PFSMapper
  {


    internal Rows1(PFSSection Section)
    {
      _pfsHandle = Section;

      rs = new List<r>();
      for (int i = 1; i <= Section.GetKeywordsNo("r"); i++)
        rs.Add(new r(Section.GetKeyword("r",i)));
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

      rHeader = new rHeader(_pfsHandle.GetKeyword("rHeader", 1));
    }

    public Rows1(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      rs = new List<r>();
      rHeader = new rHeader("rHeader");
      _pfsHandle.AddKeyword(rHeader._keyword);
    }

    public rHeader rHeader{get; private set;}
    public List<r> rs {get; private set;}
  }
}
