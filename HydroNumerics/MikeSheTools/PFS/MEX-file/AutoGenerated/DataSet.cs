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
  public partial class DataSet: PFSMapper
  {


    internal DataSet(PFSSection Section)
    {
      _pfsHandle = Section;

      Datas = new List<Data>();
      for (int i = 1; i <= Section.GetKeywordsNo("Data"); i++)
        Datas.Add(new Data(Section.GetKeyword("Data",i)));
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

    public DataSet(string pfsname)
    {
      _pfsHandle = new PFSSection(pfsname);

      Datas = new List<Data>();
      _pfsHandle.AddKeyword(new PFSKeyword("DataSetID", PFSParameterType.String, ""));

      _pfsHandle.AddKeyword(new PFSKeyword("TypeNo", PFSParameterType.Integer, 0));

    }

    public List<Data> Datas {get; private set;}
    public string DataSetID
    {
      get
      {
        return _pfsHandle.GetKeyword("DataSetID", 1).GetParameter(1).ToString();
      }
      set
      {
        _pfsHandle.GetKeyword("DataSetID", 1).GetParameter(1).Value = value;
      }
    }

    public int TypeNo
    {
      get
      {
        return _pfsHandle.GetKeyword("TypeNo", 1).GetParameter(1).ToInt();
      }
      set
      {
        _pfsHandle.GetKeyword("TypeNo", 1).GetParameter(1).Value = value;
      }
    }

  }
}
