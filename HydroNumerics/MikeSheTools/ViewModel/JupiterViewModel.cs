﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Input;
using FolderPickerLib;

using GalaSoft.MvvmLight.Command;

using HydroNumerics.Core;
using HydroNumerics.Core.WPF;
using HydroNumerics.Time.Core;
using HydroNumerics.Wells;
using HydroNumerics.JupiterTools;
using HydroNumerics.JupiterTools.JupiterPlus;
using HydroNumerics.MikeSheTools.Core;
using HydroNumerics.Geometry.Shapes;


namespace HydroNumerics.MikeSheTools.ViewModel
{  
  public class JupiterViewModel:BaseViewModel
  {
    private string DataBaseFileName;

    public JupiterViewModel()
    {
      CanReadJupiter = true;
      CanReadMikeShe = false;
      CanSaveHeads = false;
      CanSaveExtractions = false;
      OnlyRo = true;
      CVM = new ChangesViewModel();
      CVM.ChangesApplied += new EventHandler(CVM_ChangesApplied);
    }
    
    /// <summary>
    /// Changes made to background data. Make sure all view models are updated
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void CVM_ChangesApplied(object sender, EventArgs e)
    {
      allWells = null;
      BuildWellList();
      allPlants = null;
      BuildPlantList();
    }


    /// <summary>
    /// Gets the changesviewmodel
    /// </summary>
    public ChangesViewModel CVM {get; private set;}

    /// <summary>
    /// Gets the mike she view model
    /// </summary>
    public MikeSheViewModel Mshe { get; private set; }



    #region Wells

    #region Filters and sorters

    private Func<TimestampValue, bool> _onlyRoFilter
    {
      get
      {
        if (_onlyRo)
          return new Func<TimestampValue, bool>(var => var.Description == "Ro");
        else
          return new Func<TimestampValue, bool>(var => true);
      }
    }

    private Func<TimestampValue, bool> _periodFilter
    {
      get
      {
        return new Func<TimestampValue, bool>(var3 => var3.Time >= SelectionStartTime & var3.Time <= SelectionEndTime);
      }
    }

    private Func<IIntake, bool> ObsInPeriodFilter
    {
      get
      {
        if (MinNumberOfObservations)
          return new Func<IIntake, bool>(var2 => var2.HeadObservations.Items.Where(_onlyRoFilter).Where(_periodFilter).Count() >= NumberOfObs);
        else
          return new Func<IIntake, bool>(var2 => var2.HeadObservations.Items.Where(_onlyRoFilter).Where(_periodFilter).Count() <= NumberOfObs);
      }
    }


    private Func<WellViewModel, string> _wellSorter = new Func<WellViewModel, string>(var => var.DisplayName);


    #endregion

    #region Collections
    private IWellCollection wells;
    private Dictionary<string, WellViewModel> allWells;
    /// <summary>
    /// Gets all the wells
    /// </summary>
    public Dictionary<string, WellViewModel> AllWells
    {
      get
      {
        if (allWells == null & wells!=null)
        {
          allWells = new Dictionary<string, WellViewModel>();
          foreach(var v in wells)
            allWells.Add(v.ID, new WellViewModel(v, CVM));
        }
        return allWells;
      }
    }

    /// <summary>
    /// Gets the wells filtered by the filters and sorted by the sorter.
    /// </summary>
    public IEnumerable<WellViewModel> SortedAndFilteredWells{get;private set;}

    private void BuildWellList()
    {
      if (AllWells != null)
      {
         SortedAndFilteredWells = AllWells.Values.Where(var => var.Intakes.Any(ObsInPeriodFilter)).OrderBy(_wellSorter);
        NumberOfFixableWells = SortedAndFilteredWells.Count(var => var.HasFixableErrors);
        NumberOfFixedWells = SortedAndFilteredWells.Count(var => var.WasFixed);

        if (SortedAndFilteredWells.Count() > 0)
          CanSaveHeads = true;

        RaisePropertyChanged("SortedAndFilteredWells");
        RaisePropertyChanged("NumberOfFixableWells");
        RaisePropertyChanged("NumberOfFixedWells");
      }
    }

    public int NumberOfFixableWells { get; private set; }

    public int NumberOfFixedWells { get; private set; }


    #endregion

    #region Counters
//    public int NumberOfWellsThatCanBeFixed

#endregion


    #endregion

    #region Plants

    private Func<PlantViewModel, string> _plantSorter = new Func<PlantViewModel, string>(var => var.DisplayName);
    private Func<Plant, bool> _currentPlantFilter = new Func<Plant, bool>(var => true);


    #region Collections

    private IPlantCollection Plants;

    private List<PlantViewModel> allPlants;
    /// <summary>
    /// Gets all the plants
    /// </summary>
    public IEnumerable<PlantViewModel> AllPlants
    {
      get
      {
        if (allPlants == null & Plants != null)
        {
          allPlants = new List<PlantViewModel>(Plants.Select(var => new PlantViewModel(var, this)));
        }
        return allPlants;
      }
    }


    /// <summary>
    /// Returns the plants sorted and filtered based on the selected dates and minimum extraction
    /// </summary>
    public IEnumerable<PlantViewModel> SortedAndFilteredPlants {get;private set;}

    bool sortPlantsNow = false;
    private void BuildPlantList()
    {
      if (AllPlants != null)
      {
        if (sortPlantsNow)
        {
          double extra;

          List<PlantViewModel> ToReturn = new List<PlantViewModel>();
          foreach (PlantViewModel p in AllPlants)
          {
            var ext = p.plant.Extractions.Items.Where(var2 => var2.StartTime >= SelectionStartTime & var2.EndTime <= SelectionEndTime);
            if (ext.Count() == 0)
              extra = 0;
            else
              extra = ext.Average(var => var.Value);
            if (extra >= MinYearlyExtraction)
              ToReturn.Add(p);
          }
          SortedAndFilteredPlants = ToReturn.OrderBy(_plantSorter);
          if (SortedAndFilteredPlants.Count() > 0)
            CanSaveExtractions = true;

        }
        else
          SortedAndFilteredPlants = AllPlants.OrderBy(_plantSorter);
      }
      RaisePropertyChanged("SortedAndFilteredPlants");
    }
    

    #endregion
    #endregion

    #region Selection properties

    private bool _onlyRo=false;
    public bool OnlyRo
    {
      get
      {
        return _onlyRo;
      }
      set
      {
        if (_onlyRo != value)
        {
          _onlyRo = value;
          RaisePropertyChanged("OnlyRo");
          BuildWellList();
        }
      }
    }


    private DateTime _selectionStartTime =  new DateTime(2000,1,1);
    public DateTime SelectionStartTime
    {
      get
      {
        return _selectionStartTime;
      }
      set
      {
        if (_selectionStartTime != value)
        {
          _selectionStartTime = value;
          BuildWellList();
          BuildPlantList();
          RaisePropertyChanged("SelectionStartTime");
        }
      }
    }

    private DateTime _selectionEndTime= DateTime.Now;
    public DateTime SelectionEndTime
    {
      get
      {
        return _selectionEndTime;
      }
      set
      {
        if (_selectionEndTime != value)
        {
          _selectionEndTime = value;
          BuildWellList();
          BuildPlantList();
          RaisePropertyChanged("SelectionEndTime");
        }
      }
    }

    private int _numberOfObs=0;

    public int NumberOfObs
    {
      get { return _numberOfObs; }
      set
      {
        if (_numberOfObs != value)
        {
          _numberOfObs = value;
          BuildWellList();
          RaisePropertyChanged("NumberOfObs");
        }
      }
    }

    private double _minYearLyExtraction = 0;
    public double MinYearlyExtraction
    {
      get { return _minYearLyExtraction; }
      set
      {
        if (_minYearLyExtraction != value)
        {
          _minYearLyExtraction = value;
          BuildPlantList();
          RaisePropertyChanged("MinYearlyExtraction");
        }
      }
    }

    private bool minNumberOfObservations = true;
    public bool MinNumberOfObservations
    {
      get
      {
        return minNumberOfObservations;
      }
      set
      {
        if (minNumberOfObservations != value)
        {

          minNumberOfObservations = value;
          RaisePropertyChanged("MinNumberOfObservations");
          BuildWellList();
        }
      }
    }



    #endregion

    private StringBuilder log = new StringBuilder();

    private void AddLineToLog(string ToAdd)
    {
      log.AppendLine(ToAdd);
      RaisePropertyChanged("Log");
    }

    public string Log
    {
      get
      {
        return log.ToString();
      }
    }


    #region Import methods
    /// <summary>
    /// Opens a Jupiter database and reads requested data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ReadJupiter(string FileName)
    {

      DataBaseFileName = FileName;
      CanReadJupiter = false;
      CanReadMikeShe = true;

      Reader R = new Reader(FileName);
     
      Task t = Task.Factory.StartNew(() => wells = R.ReadWellsInSteps());
      t.Wait();
      

      //Start reading the remaining well data
      Task t4 = Task.Factory.StartNew(() => R.ReadLithology(wells));
      JupiterXLFastReader jxf = new JupiterXLFastReader(FileName);
      CVM.SetDataBaseConnection(jxf);
      Task t5 = Task.Factory.StartNew(() => jxf.ReadWaterLevels(wells));
      t5.ContinueWith((tt) => WellsRead());

      
      //Read plants
      Task t2 = Task.Factory.StartNew(() => Plants = R.ReadPlants(wells));
      t2.Wait();
      BuildPlantList();
      var t3 = t2.ContinueWith((tt) => R.FillInExtractionWithCount(Plants));
      t3.ContinueWith((tt) => PlantsRead());
    }


    /// <summary>
    /// This method is called when all plants have been read
    /// </summary>
    private void PlantsRead()
    {
      sortPlantsNow = true;
      foreach (Plant P in Plants)
      {
        P.DistributeExtraction(false);
        P.SurfaceWaterExtrations.Sort();
      }
      CVM.Plants = Plants;
      BuildPlantList();
    }

    /// <summary>
    /// This method is called when all wells have been read
    /// </summary>
    private void WellsRead()
    {
      foreach (IWell w in wells)
        foreach (IIntake I in w.Intakes)
          I.HeadObservations.Sort();

      BuildWellList();
      CVM.Wells = wells;
    }


    #endregion


    #region Commands

    #region LoadJupiter
    RelayCommand loadDatabase;

    /// <summary>
    /// Gets the command that loads the database
    /// </summary>
    public ICommand LoadDatabaseCommand
    {
      get
      {
        if (loadDatabase == null)
        {
          loadDatabase = new RelayCommand(() => this.LoadDataBase(), () => this.CanReadJupiter);
        }
        return loadDatabase;
      }
    }

    private bool CanReadJupiter { get; set; }

    private void LoadDataBase()
    {
      Microsoft.Win32.OpenFileDialog openFileDialog2 = new Microsoft.Win32.OpenFileDialog();
      openFileDialog2.Filter = "Known file types (*.mdb)|*.mdb";
      openFileDialog2.ShowReadOnly = true;
      openFileDialog2.Title = "Select an Access 2000 file with data in JupiterXL format";

      if (openFileDialog2.ShowDialog().Value)
      {
        AsyncWithWait(() => ReadJupiter(openFileDialog2.FileName));
      }
    }


    #endregion

    #region SaveDetailedTimeSeries

    RelayCommand saveDetailedTimeSeriesCommand;

    /// <summary>
    /// Gets the command that saves the detailed time series files
    /// </summary>
    public ICommand SaveDetailedTimeSeriesCommand
    {
      get
      {
        if (saveDetailedTimeSeriesCommand == null)
        {
          saveDetailedTimeSeriesCommand = new RelayCommand(() => this.SaveDetailedTimeSeries(), () => CanSaveHeads);
        }
        return saveDetailedTimeSeriesCommand;
      }
    }

    
   
    private void SaveDetailedTimeSeries()
    {
      var dlg = new FolderPickerDialog();
      if (dlg.ShowDialog() == true)
      {
        var intakes = SortedAndFilteredWells.SelectMany(var => var.Intakes);
        MsheInputFileWriters.WriteDetailedTimeSeriesText(dlg.SelectedPath, intakes, SelectionStartTime, SelectionEndTime);
        MsheInputFileWriters.WriteToDatFile(System.IO.Path.Combine(dlg.SelectedPath, "Timeseries.dat"), intakes, _periodFilter, _onlyRoFilter);
        AsyncWithWait(() => MsheInputFileWriters.WriteDetailedTimeSeriesDfs0(dlg.SelectedPath, intakes, _periodFilter, _onlyRoFilter));
      }
    }

    #endregion

    #region SaveObservationsToShape


    RelayCommand saveObservationsToShapeCommand;

    /// <summary>
    /// Gets the command that saves the detailed time series files
    /// </summary>
    public ICommand SaveObservationsToShapeCommand
    {
      get
      {
        if (saveObservationsToShapeCommand == null)
        {
          saveObservationsToShapeCommand = new RelayCommand(() => AsyncWithWait(() => SaveObservationsToShape()), () => CanSaveHeads);
        }
        return saveObservationsToShapeCommand;
      }
    }

    private bool CanSaveHeads{get;set;}

    private void SaveObservationsToShape()
    {
      Microsoft.Win32.SaveFileDialog openFileDialog2 = new Microsoft.Win32.SaveFileDialog();
      openFileDialog2.Filter = "Known file types (*.shp)|*.sh";
      openFileDialog2.Title = "Save observations to a shape file";


      if (openFileDialog2.ShowDialog().Value)
      {
        var Jints = SortedAndFilteredWells.SelectMany(var => var.Intakes).Where(ObsInPeriodFilter).Cast<JupiterIntake>();
        MsheInputFileWriters.AddDataForNovanaPejl(Jints, SelectionStartTime, SelectionEndTime);

          foreach (var v in SortedAndFilteredWells)
          {
            foreach (JupiterIntake JI in v.Intakes.Where(var =>((JupiterIntake) var).Data!=null))
            {
              JI.Data["AUTOCORRECT"] = v.WasFixed;
              var sc = v.Screens.Where(var => var.Intake.IDNumber == JI.IDNumber);
              if (sc.Count() > 0)
              {
                if (Mshe != null)
                {
                  JI.Data["ORG_LAYTOP"] = sc.Max(var2 => var2.MsheTopLayer);
                  JI.Data["ORG_LAYBOT"] = sc.Min(var2 => var2.MsheBottomLayer);
                  var newl = sc.FirstOrDefault(var => var.NewMsheLayer.HasValue);
                  if (newl != null)
                    JI.Data["ADJUST_LAY"] = newl.NewMsheLayer.Value;
                }
              }
          }
        }
        WriteShapeFromDataRow(openFileDialog2.FileName, Jints);  
      }
    }

          /// <summary>
      /// Writes a point shape with entries for each intake in the list. Uses the dataRow as attributes.
      /// </summary>
      /// <param name="FileName"></param>
      /// <param name="Intakes"></param>
      /// <param name="Start"></param>
      /// <param name="End"></param>
    private void WriteShapeFromDataRow(string FileName, IEnumerable<JupiterIntake> Intakes)
    {
      ShapeWriter PSW = new ShapeWriter(FileName);
      foreach (JupiterIntake JI in Intakes)
      {
        PSW.WritePointShape(JI.well.X, JI.well.Y);
        PSW.Data.WriteData(JI.Data);
      }
      PSW.Dispose();
    }



    #endregion

    #region SaveExtractionsToShape

    RelayCommand saveExtractionsToShapeCommand;

    /// <summary>
    /// Gets the command that saves the detailed time series files
    /// </summary>
    public ICommand SaveExtractionsToShapeCommand
    {
      get
      {
        if (saveExtractionsToShapeCommand == null)
        {
          saveExtractionsToShapeCommand = new RelayCommand(() => AsyncWithWait(() => SaveExtractionsToShape()), () => CanSaveExtractions);
        }
        return saveExtractionsToShapeCommand;
      }
    }


    private void SaveExtractionsToShape()
    {
      Microsoft.Win32.SaveFileDialog openFileDialog2 = new Microsoft.Win32.SaveFileDialog();
      openFileDialog2.Filter = "Known file types (*.shp)|*.sh";
      openFileDialog2.Title = "Save selected extractions to a shape file";

      if (openFileDialog2.ShowDialog().Value)
      {
        var Jints = MsheInputFileWriters.AddDataForNovanaExtraction(SortedAndFilteredPlants.Select(var => var.plant), SelectionStartTime, SelectionEndTime);

          foreach (var P in SortedAndFilteredPlants)
          {
            foreach (var v in P.Wells)
            {
              foreach (JupiterIntake JI in v.Intakes)
              {
                if (JI.Data != null)
                {
                  JI.Data["AUTOCORRECT"] = v.WasFixed;
                  var sc = v.Screens.Where(var => var.Intake.IDNumber == JI.IDNumber);
                  if (sc.Count() > 0)
                  {
                    if (Mshe != null)
                    {
                      JI.Data["ORG_LAYTOP"] = sc.Max(var2 => var2.MsheTopLayer);
                    JI.Data["ORG_LAYBOT"] = sc.Min(var2 => var2.MsheBottomLayer);
                    var newl = sc.FirstOrDefault(var => var.NewMsheLayer.HasValue);
                    if (newl != null)
                      JI.Data["ADJUST_LAY"] = newl.NewMsheLayer.Value;
                    }
                  
                }
              }
            }
          }
        }
        WriteShapeFromDataRow(openFileDialog2.FileName, Jints);
      }
    }

    #endregion

    #region SaveExtractions
    RelayCommand saveExtractionsCommand;

    /// <summary>
    /// Gets the command that saves the extration files
    /// </summary>
    public ICommand SaveExtractionsCommand
    {
      get
      {
        if (saveExtractionsCommand == null)
        {
          saveExtractionsCommand = new RelayCommand(() => this.SaveExtractions(), () => this.CanSaveExtractions);
        }
        return saveExtractionsCommand;
      }
    }


    private bool CanSaveExtractions {get;set;}

    private void SaveExtractions()
    {
      var dlg = new FolderPickerDialog();
      dlg.Title ="Select a folder where the extraction input files will be saved";
      if (dlg.ShowDialog() == true)
      {
        AsyncWithWait(()=> MsheInputFileWriters.WriteExtractionDFS0(dlg.SelectedPath, SortedAndFilteredPlants, SelectionStartTime, SelectionEndTime));
      }
    }


    public void SaveExtractionPermits(int DistributionYear, int startYear, int endyear)
    {
      var dlg = new FolderPickerDialog();
      dlg.Title = "Select a folder where the extraction input files will be saved";
      if (dlg.ShowDialog() == true)
      {
        AsyncWithWait(() => MsheInputFileWriters.WriteExtractionDFS0Permits(dlg.SelectedPath, SortedAndFilteredPlants, DistributionYear, startYear, endyear));
      }
    }


    #endregion

    #region SaveGMSExtractions
    RelayCommand saveGMSExtractionsCommand;

    /// <summary>
    /// Gets the command that saves the extration files
    /// </summary>
    public ICommand SaveGMSExtractionsCommand
    {
      get
      {
        if (saveGMSExtractionsCommand == null)
        {
          saveGMSExtractionsCommand = new RelayCommand(() => this.SaveGMSExtractions(), () => this.CanSaveExtractions);
        }
        return saveGMSExtractionsCommand;
      }
    }



    private void SaveGMSExtractions()
    {
      var dlg = new FolderPickerDialog();
      dlg.Title = "Select a folder where the extraction input files for GMS will be saved";
      if (dlg.ShowDialog() == true)
      {
        AsyncWithWait(() => MsheInputFileWriters.WriteGMSExtraction(dlg.SelectedPath, SortedAndFilteredPlants, SelectionStartTime, SelectionEndTime));
      }
    }

    #endregion 
    
    #region SaveLayerStatistics

    RelayCommand saveLayerStatisticsFilesCommand;

    /// <summary>
    /// Gets the command that saves the LayerStatistics files
    /// </summary>
    public ICommand SaveLayerStatisticsFilesCommand
    {
      get
      {
        if (saveLayerStatisticsFilesCommand == null)
        {
          saveLayerStatisticsFilesCommand = new RelayCommand(() => SaveLayerStatisticsFiles(), () => CanSaveHeads);
        }
        return saveLayerStatisticsFilesCommand;
      }
    }


    private void SaveLayerStatisticsFiles()
    {
      var dlg = new FolderPickerDialog();
      if (dlg.ShowDialog() == true)
      {
        //remove wells without xy, remove intakes with missing values and then apply ObsInperiodfilter again because there can be wells with multiple intakes where only is acceptable
        var intakes = SortedAndFilteredWells.Where(w=>w.X!=0 & w.Y!=0).SelectMany(var => var.Intakes.Where(var2=>!var2.HasMissingdData())).Where(ObsInPeriodFilter);
        AsyncWithWait(()=> MsheInputFileWriters.WriteToLSInput(dlg.SelectedPath, intakes, Mshe, _periodFilter, _onlyRoFilter));
      }
    }

    #endregion

    #region LoadMikeShe
    RelayCommand loadMikeSheCommand;

    /// <summary>
    /// Gets the command that loads the Mike she
    /// </summary>
    public ICommand LoadMikeSheCommand
    {
      get
      {
        if (loadMikeSheCommand == null)
        {
          loadMikeSheCommand = new RelayCommand(() => this.LoadMikeShe(), () => this.CanReadMikeShe);
        }
        return loadMikeSheCommand;
      }
    }

    private bool CanReadMikeShe { get; set; }

    private void LoadMikeShe()
    {
      Microsoft.Win32.OpenFileDialog openFileDialog2 = new Microsoft.Win32.OpenFileDialog();
      openFileDialog2.Filter = "Known file types (*.she)|*.she";
      openFileDialog2.ShowReadOnly = true;
      openFileDialog2.Title = "Select a Mike She input file";

      if (openFileDialog2.ShowDialog().Value)
      {
        AsyncWithWait(() => LoadMikeSheMethod(openFileDialog2.FileName));
      }
    }

    public void LoadMikeSheMethod(string filename)
    {
      Model mShe = new Model(filename);

      if (mShe.Processed != null)
      {
        CanReadMikeShe = false;
        if (Plants != null & wells != null)
          SelectByMikeShe(mShe);
        Mshe = new MikeSheViewModel(mShe, this);
        RaisePropertyChanged("Mshe");
      }
      else
      {
        throw new Exception("Mike She model must be preprocessed");
      }
    }

    #endregion

    #region DeselectWells
    RelayCommand deselectWellsWithShapeCommand;

    /// <summary>
    /// Gets the command that saves the detailed time series files
    /// </summary>
    public ICommand DeselectWellsWithShapeCommand
    {
      get
      {
        if (deselectWellsWithShapeCommand == null)
        {
          deselectWellsWithShapeCommand = new RelayCommand(() => this.DeselectWellsWithShape(), () => this.CanDeselectWellsWithShape);
        }
        return deselectWellsWithShapeCommand;
      }
    }


    private bool CanDeselectWellsWithShape
    {
      get
      {
        return SortedAndFilteredWells != null && SortedAndFilteredWells.Count() > 0;
      }
    }


    RelayCommand<System.Collections.IList> removeSelectedWellsCommand;

    /// <summary>
    /// Gets the command that saves the detailed time series files
    /// </summary>
    public ICommand RemoveSelectedWellsCommand
    {
      get
      {
        if (removeSelectedWellsCommand == null)
        {
          removeSelectedWellsCommand = new RelayCommand<System.Collections.IList>(param => this.RemoveWells(param), param => this.CanRemoveWells(param));
        }
        return removeSelectedWellsCommand;
      }
    }


    private bool CanRemoveWells(System.Collections.IList items)
    {
      if (items == null)
        return false;
      return items.Count > 0;
    }


    private void RemoveWells(System.Collections.IList items)
    {
      var collection = items.Cast<WellViewModel>();

      foreach (var v in collection)
      {
        if (allWells.ContainsKey(v.DisplayName))
        {
          allWells.Remove(v.DisplayName);
          wells.Remove(v.DisplayName);
        }
      }
      BuildWellList();
    }


    private void LogThis(string line)
    {
      var c = System.IO.Directory.GetCurrentDirectory();
      using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),"Logfile.txt"), true))
      {
        sw.WriteLine(DateTime.Now.ToString() + ". " + line);
      }
    }

    private void DeselectWellsWithShape()
    {
      Microsoft.Win32.OpenFileDialog openFileDialog2 = new Microsoft.Win32.OpenFileDialog();
      openFileDialog2.Filter = "Known file types (*.shp)|*.shp";
      openFileDialog2.ShowReadOnly = true;
      openFileDialog2.Title = "Select a shape file with wells";

      if (openFileDialog2.ShowDialog().Value)
      {
        try
        {
          DotSpatial.Data.IFeatureSet fs = DotSpatial.Data.FeatureSet.OpenFile(openFileDialog2.FileName);

          foreach (var f in fs.Features)
          {
            string wellid = f.DataRow["BOREHOLENO"].ToString();
            if (allWells.ContainsKey(wellid))
            {
              allWells.Remove(wellid);
              wells.Remove(wellid);
            }
            else
            {
              wellid = wellid.Trim();
              if (allWells.ContainsKey(wellid))
              {
                allWells.Remove(wellid);
                wells.Remove(wellid);
              }
              else
              {
                wellid = " " + wellid;
                if (allWells.ContainsKey(wellid))
                {
                  allWells.Remove(wellid);
                  wells.Remove(wellid);
                }
                else
                {
                  wellid = " " + wellid;
                  if (allWells.ContainsKey(wellid))
                  {
                    allWells.Remove(wellid);
                    wells.Remove(wellid);
                  }
                }
              }
            }
          }
          fs.Dispose();
          BuildWellList();
        }
        catch (Exception e)
        {
          LogThis("Error: " + e.Message);
          LogThis(e.ToString());
          LogThis(e.TargetSite.ToString());
        }
      }
    }

    #endregion

    #region Remove and DeselectPlants

    RelayCommand deselectPlantsWithShapeCommand;
    RelayCommand removePlantsOfType;


    /// <summary>
    /// Gets the command that saves the detailed time series files
    /// </summary>
    public ICommand DeselectPlantsWithShapeCommand
    {
      get
      {
        if (deselectPlantsWithShapeCommand == null)
        {
          deselectPlantsWithShapeCommand = new RelayCommand(() => this.DeselectPlantsWithShape(), () => this.CanDeselectPlantsWithShape);
        }
        return deselectPlantsWithShapeCommand;
      }
    }


    /// <summary>
    /// Gets the command that saves the detailed time series files
    /// </summary>
    public ICommand RemovePlantsOfType
    {
      get
      {
        if (removePlantsOfType == null)
        {
          removePlantsOfType = new RelayCommand(() => AsyncWithWait(() => RemovePlantsOfTypeMethod()), () => CanDeselectPlantsWithShape);
        }
        return removePlantsOfType;
      }
    }

    private ICollectionView  _KeepRemove;
    public ICollectionView  KeepRemove
    {
      get {
        if (_KeepRemove == null)
        {
          _KeepRemove = new System.Windows.Data.CollectionView(new string[] { "Remove", "Keep" });
        }
        return _KeepRemove; }
      set
      {
        if (_KeepRemove != value)
        {
          _KeepRemove = value;
          RaisePropertyChanged("KeepRemove");
        }
      }
    }
    

    private ICollectionView _PlantTypes;
    public ICollectionView PlantTypes
    {
      get {
        if (_PlantTypes == null & AllPlants!=null)
        {
          PlantTypes = new System.Windows.Data.CollectionView(AllPlants.Select(p => p.CompanyType).Distinct());
        }
        
        return _PlantTypes; }
      set
      {
        if (_PlantTypes != value)
        {
          _PlantTypes = value;
          RaisePropertyChanged("PlantTypes");
        }
      }
    }

    private void RemovePlantsOfTypeMethod()
    {
      string typetoremove = PlantTypes.CurrentItem.ToString();

      for (int i = allPlants.Count - 1; i >= 0; i--)
      {
        if (KeepRemove.CurrentItem.ToString() == "Remove")
        {
          if (allPlants[i].CompanyType == typetoremove)
          {
            Plants.Remove(allPlants[i].ID);
            allPlants.RemoveAt(i);
          }
        }
        else
        {
          if (allPlants[i].CompanyType != typetoremove)
          {
            Plants.Remove(allPlants[i].ID);
            allPlants.RemoveAt(i);
          }
        }

      }
      BuildPlantList();
      PlantTypes = null;
    }


    private bool CanDeselectPlantsWithShape
    {
      get
      {
        return SortedAndFilteredPlants != null && SortedAndFilteredPlants.Count() > 0;
      }
    }

    private void DeselectPlantsWithShape()
    {
      Microsoft.Win32.OpenFileDialog openFileDialog2 = new Microsoft.Win32.OpenFileDialog();
      openFileDialog2.Filter = "Known file types (*.shp)|*.shp";
      openFileDialog2.ShowReadOnly = true;
      openFileDialog2.Title = "Select a shape file with plants";

      if (openFileDialog2.ShowDialog().Value)
      {
        using (ShapeReader sr = new ShapeReader(openFileDialog2.FileName))
        {
          for (int i = 0; i < sr.Data.NoOfEntries; i++)
          {
            int plantid = sr.Data.ReadInt(i, "PLANTID");
            var p = allPlants.SingleOrDefault(var => var.ID == plantid);
            if (p != null)
            {
              allPlants.Remove(p);
              Plants.Remove(plantid);
            }
          }
        }
        BuildPlantList();
      }
    }

    #endregion

    #region FixErrors
    RelayCommand fixErrorsCommand;

    /// <summary>
    /// Gets the command that saves the detailed time series files
    /// </summary>
    public ICommand FixErrorsCommand
    {
      get
      {
        if (fixErrorsCommand == null)
        {
          fixErrorsCommand = new RelayCommand(() => AsyncWithWait(() => FixErrors()), () => CanFixErrors);
        }
        return fixErrorsCommand;
      }
    }


    private bool CanFixErrors
    {
      get
      {
        return SortedAndFilteredWells != null && NumberOfFixableWells > 0;
      }
    }

    public void FixErrors()
    {
      foreach (var v in AllWells.Values)
        v.Fix();
      BuildWellList();
      BuildPlantList();
    }

    #endregion

    #endregion

    /// <summary>
    /// Removes wells and plants outside model area. If a well is assigned to a plant with another well in the model area it is kept in the collection
    /// Do not look at depths.
    /// </summary>
    /// <param name="mShe"></param>
    private void SelectByMikeShe(Model mShe)
    {
      Dictionary<string, WellViewModel> WellsToSave = new Dictionary<string,WellViewModel>();
      Dictionary<string, WellViewModel> WellsToKeep = new Dictionary<string, WellViewModel>();
      List<PlantViewModel> PLantsToKeep = new List<PlantViewModel>();

      foreach (var p in AllPlants)
      {
        if (p.Wells.Any(var => mShe.GridInfo.IsInModelArea(var.X, var.Y)))
        {
          PLantsToKeep.Add(p);
          foreach (var w in p.Wells)
            if (!WellsToSave.ContainsKey(w.DisplayName))
              WellsToSave.Add(w.DisplayName, w);
        }
        else
          Plants.Remove(p.ID);
      }

      allPlants = PLantsToKeep;

      foreach (var w in AllWells.Values)
      {
        if (w.LinkToMikeShe(mShe) || WellsToSave.ContainsKey(w.DisplayName))
          WellsToKeep.Add(w.DisplayName, w);
        else
          wells.Remove(w.DisplayName);
      }
      allWells = WellsToKeep;
      BuildWellList();
      BuildPlantList();
    }
  }
}
