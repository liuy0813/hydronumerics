﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using HydroNumerics.HydroNet.Core;
using HydroNumerics.HydroNet.ViewModel;

namespace HydroNumerics.HydroNet.View
{
  /// <summary>
  /// Interaction logic for SingleWBModel.xaml
  /// </summary>
  public partial class SingleWBModel : Window
  {

    private ObservableCollection<Model> Models = new ObservableCollection<Model>();

    public SingleWBModel()
    {
      InitializeComponent();
      tree.ItemsSource = Models;
      tree.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(tree_SelectedItemChanged);
     
    }

    void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      AbstractWaterBody ab = e.NewValue as AbstractWaterBody;
      if (ab != null)
        WaterView.DataContext = new WaterBodyViewModel(ab);
    }

    /// <summary>
    /// Loads a model
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
      openFileDialog.Multiselect = false;
      openFileDialog.Filter = "xml files | *.xml";
      openFileDialog.Multiselect = true;

      if (openFileDialog.ShowDialog().Value)
      {
        foreach (string s in openFileDialog.FileNames)
        {
          Models.Add(ModelFactory.GetModel(s));
        }
      }     
    }


    /// <summary>
    /// Runs the model
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      Model k = tree.SelectedValue as Model;

      if (k != null)
        k.RunScenario();

      //M.SetState("Initial",StartTime.SelectedDate.Value,new WaterPacket(1));
      //((Model)DataContext).MoveInTime(EndTime.SelectedDate.Value, TimeSpan.FromDays(1));

      //foreach (System.Windows.Controls.DataVisualization.Charting.Series ls in WaterView.OutputChart.Series)
      // // ls.Refresh();

      //foreach (System.Windows.Controls.DataVisualization.Charting.Series ls in WaterView.WBChart.Series)
      // // ls.Refresh();
     

    }

    /// <summary>
    /// Saves the model
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
      saveFileDialog.Filter = "xml files | *.xml";
     
      if (saveFileDialog.ShowDialog().Value)
      {
        Model M = tree.SelectedValue as Model;;
        M.Save(saveFileDialog.FileName);
      }
    }
  }
}
