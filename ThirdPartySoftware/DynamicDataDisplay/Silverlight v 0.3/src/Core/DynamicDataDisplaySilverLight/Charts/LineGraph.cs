﻿using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Collections.Generic;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Common.Auxiliary;

namespace Microsoft.Research.DynamicDataDisplay
{
    /// <summary>Series of points connected by one polyline</summary>
    public class LineGraph : PointsGraphBase, ILegendable
    {
        /// <summary>Filters applied to points before rendering</summary>
        private readonly FilterCollection filters = new FilterCollection();
        protected Path path = null;
        private LineGraphSettings settings = null;
        private LineGraphSettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
                if (path != null)
                {
                    path.StrokeThickness = settings.LineThickness;
                }
            }
        }

        /// <summary>
        /// Filters which are used to reduce the amount of points to draw
        /// </summary>
        public FilterCollection Filters {
            get {
                return filters;
            }
        }

        protected Plotter parentPlotter = null;
        
        protected FakePointList filteredPoints;

        private void refreshPath() {
            if (Plotter != null && path!=null) {
                if (Plotter.MainCanvas.Children.Contains(path)) {
                    Plotter.MainCanvas.Children.Remove(path);
                    Plotter.MainCanvas.Children.Add(path);
                }
            }

        }


        public bool ShowInPlotter
        {
          get { return (bool)GetValue(ShowInPlotterProperty); }
          set
          {

            SetValue(ShowInPlotterProperty, value);
          }
        }

        public static readonly DependencyProperty ShowInPlotterProperty =
            DependencyProperty.Register(
              "ShowInPlotter",
              typeof(bool),
              typeof(LineGraph),
              new PropertyMetadata(OnShowInPlotterPropertyChanged)
            );

        private static void OnShowInPlotterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
          LineGraph source = d as LineGraph;
          source.UpdateCore();
          if (source.Viewport != null)
            source.Viewport.Plotter.FitToView();
        }



        protected override void UpdateCore()
        {
            if (DataSource == null || Viewport == null || Viewport.Output == new Rect(0,0,0,0)) return;

            if (path.Clip == null) updateClippingRect();

            Rect output = Viewport.Output;
            var transform = GetTransform();

            PerformanceCounter.startStopwatch("Updating linegraph core: Getting points");
            
                IEnumerable<Point> points = GetPoints();
                PerformanceCounter.stopStopwatch("Updating linegraph core: Getting points");
                PerformanceCounter.startStopwatch("Updating linegraph core: Getting Content bounds");
            
                ContentBounds = BoundsHelper.GetViewportBounds(points, transform.DataTransform);
                PerformanceCounter.stopStopwatch("Updating linegraph core: Getting Content bounds");

                List<Point> transformedPoints = transform.DataToScreen(points);
                PerformanceCounter.startStopwatch("Updating linegraph core: Creating fake points");
                filteredPoints = new FakePointList(FilterPoints(transformedPoints),
                    output.Left, output.Right,output.Top,output.Bottom);
                PerformanceCounter.stopStopwatch("Updating linegraph core: Creating fake points");
                PerformanceCounter.startStopwatch("Updating linegraph core: adding segments");
                if (filteredPoints.Count!=0)
                {
                    segments.Clear();
                    
                       for (int i = 0; i < filteredPoints.Count; i++)
                        {
                            LineSegment segment = new LineSegment();
                            segment.Point = filteredPoints[i];
                            segments.Add(segment);
                       }
                 
                    figure.StartPoint = filteredPoints.StartPoint;
                PerformanceCounter.stopStopwatch("Updating linegraph core: adding segments");        
                }
        }

        private bool filteringEnabled = true;
        public bool FilteringEnabled
        {
            get { return filteringEnabled; }
            set
            {
                if (filteringEnabled != value)
                {
                    filteringEnabled = value;
                    filteredPoints = null;
                    UpdateCore();
                }
            }
        }

        protected List<Point> FilterPoints(List<Point> points)
        {
            if (!filteringEnabled)
                return points;

            var filteredPoints = filters.Filter(points, Viewport.Output);

            return filteredPoints;
        }

        private PathGeometry geom;
        protected PathFigureCollection figures;
        protected PathFigure figure;
        protected PathSegmentCollection segments;
        private List<LineSegment> segmentsList = new List<LineSegment>();
        private ToolTip tooltip = new ToolTip();

        public LineGraph() {
            if(Settings == null)
                Settings = new LineGraphSettings();
            filters.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(filters_CollectionChanged);
            path = new Path();
            geom = new PathGeometry();
            figures = new PathFigureCollection();
            figure = new PathFigure();
            segments = new PathSegmentCollection();
            figure.Segments = segments;
            figures.Add(figure);
            geom.Figures = figures;
            path.Data = geom;
            path.StrokeLineJoin = PenLineJoin.Round;
            path.StrokeThickness = settings.LineThickness;

        
            path.SetBinding(Path.StrokeProperty, new System.Windows.Data.Binding() { Source = this, Path = new PropertyPath("LineColor") });
            Filters.Add(new Microsoft.Research.DynamicDataDisplay.Filters.FrequencyFilter());
        }

        /// <summary>
        /// Create a linegraph using specific datasource and description
        /// </summary>
        /// <param name="pointSource">Data source</param>
        /// <param name="description">Description shown in legend</param>
        public LineGraph(IPointDataSource pointSource,string description)
            : this()
        {
            DataSource = pointSource;
            Settings = new LineGraphSettings() { Description = description};
        }

        /// <summary>
        /// Create a linegraph using specific datasource and settings
        /// </summary>
        /// <param name="pointSource">Data source</param>
        /// <param name="settings">Graph visual settings</param>
        public LineGraph(IPointDataSource pointSource,LineGraphSettings settings)
            : this()
        {
            this.Settings = settings;
            DataSource = pointSource;
        }

        public virtual Range<double> GetVerticalRange(Range<double> HorizontalRange)
        {
          int k = 0;
          double min = double.MaxValue;
          double max = double.MinValue;
          if (DataSource != null)
          {

            var localpoints = GetPoints();


            if (localpoints.Count() == 2)
            {
              return new Range<double>(localpoints.First().Y - 1e-10, localpoints.First().Y);
            }

             
            foreach (var item in localpoints)
            {
              if (HorizontalRange.Max >= item.X && HorizontalRange.Min <= item.X)
              {
                k++;
                min = Math.Min(item.Y, min);
                max = Math.Max(item.Y, max);
              }
            }
          }
          if (k > 0)
          {
            if (min == max)
              min -= 1e-10;
            return new Range<double>(min, max);
          }
          else
            return new Range<double>(0, 1);
        }


        void filters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            filteredPoints = null;
            UpdateCore();
        }

        protected void updateClippingRect() {
            if (path != null) {
                RectangleGeometry clippingGeom = new RectangleGeometry();
                clippingGeom.Rect = new Rect(0, 0, Plotter.MainCanvas.Width, Plotter.MainCanvas.Height);
                path.Clip = clippingGeom;
            }
        }

        void LineGraph_MouseLeave(object sender, MouseEventArgs e)
        {
            tooltip.IsOpen = false;
        }

        void LineGraph_MouseEnter(object sender, MouseEventArgs e)
        {
            tooltip.IsOpen = true;
        }

        #region IPlotterElement Members

        public override void OnPlotterAttached(Plotter plotter)
        {
            if (parentPlotter == null)
            {
                base.OnPlotterAttached(plotter);
                parentPlotter = plotter;
                Plotter.MainCanvas.SizeChanged += new SizeChangedEventHandler(MainCanvas_SizeChanged);
                Plotter.MainCanvas.Children.Add(path);
                if (IsTooltipEnabled)
                {
                    tooltip = new ToolTip
                    {
                        Content = Description
                    };
                    path.MouseEnter += new MouseEventHandler(LineGraph_MouseEnter);
                    path.MouseLeave += new MouseEventHandler(LineGraph_MouseLeave);
                    ToolTipService.SetToolTip(path, tooltip);
                }
            }
            UpdateCore();
        }

        void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateClippingRect();
        }

        public override void OnPlotterDetaching(Plotter plotter)
        {
            if (parentPlotter != null)
            {
                Plotter.MainCanvas.SizeChanged -= MainCanvas_SizeChanged;
                plotter.MainCanvas.Children.Remove(path);
                parentPlotter = null;
                base.OnPlotterDetaching(plotter);
            }
        }

        /// <summary>
        /// Get an associated plotter
        /// </summary>
        public Plotter Plotter
        {
            get { return parentPlotter; }
        }

        #endregion

        #region ILegendable Members

        /// <summary>
        /// if set to TRUE, a tooltip will be shown when mouse enters a graphs line
        /// </summary>
        public bool IsTooltipEnabled
        {
            get
            {
                if (Settings == null)
                    Settings = new LineGraphSettings();
                return Settings.IsToolTipEnabled;
            }
            set
            {
                if (Settings == null)
                    Settings = new LineGraphSettings();
                Settings.IsToolTipEnabled = value;
                if (Settings.IsToolTipEnabled)
                {
                    tooltip = new ToolTip
                    {
                        Content = Description
                    };
                    path.MouseEnter += new MouseEventHandler(LineGraph_MouseEnter);
                    path.MouseLeave += new MouseEventHandler(LineGraph_MouseLeave);
                    ToolTipService.SetToolTip(path, tooltip);
                }
                else {
                    ToolTipService.SetToolTip(path, null);
                    path.MouseEnter -= LineGraph_MouseEnter;
                    path.MouseLeave -= LineGraph_MouseLeave;
                }
            }
        }

        /// <summary>
        /// If true the linegraph will be shown in attached legend
        /// </summary>
        public bool ShowInLegend
        {
            get
            {
                if (Settings == null)
                    Settings = new LineGraphSettings();
                return Settings.ShowInLegend;
            }
            set
            {
                if (Settings == null)
                    Settings = new LineGraphSettings();
                Settings.ShowInLegend = value;
                if (VisualizationChanged != null) VisualizationChanged(this, null);
            }
        }



        public string Description
        {
          get { return (string)GetValue(DescriptionProperty); }
          set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(LineGraph), new PropertyMetadata("No description"));

        


        ///// <summary>
        ///// Get or set description string of the linegraph which can be seen in a legend or in a tooltip
        ///// </summary>
        //public string Description
        //{
        //    get {
        //        if (Settings == null)
        //            Settings = new LineGraphSettings();
        //        return Settings.Description;
        //    }
        //    set {
        //        if (Settings == null)
        //            Settings = new LineGraphSettings();
                
        //        Settings.Description = value;
        //        ChartPlotter chPlotter = Plotter as ChartPlotter;
        //        tooltip.Content = Settings.Description;
        //        if(IsTooltipEnabled)
        //            ToolTipService.SetToolTip(path, tooltip);                
        //        if (VisualizationChanged != null) VisualizationChanged(this, null);
        //    }
        //}

        public double LineThickness
        {
            get
            {
                if (Settings == null)
                    Settings = new LineGraphSettings();
                return Settings.LineThickness;
            }
            set
            {
                if (Settings == null)
                    Settings = new LineGraphSettings();
                Settings.LineThickness = value;
                path.StrokeThickness = Settings.LineThickness;
                refreshPath();
                if (VisualizationChanged != null) VisualizationChanged(this, null);
            }
        }



        public SolidColorBrush LineColor
        {
          get { return (SolidColorBrush)GetValue(LineColorProperty); }
          set { SetValue(LineColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register("LineColor", typeof(SolidColorBrush), typeof(LineGraph), new PropertyMetadata(new SolidColorBrush(SolidColorHelper.Next()), (sender, args) => ((LineGraph)sender).refreshPath()));

        

        /// <summary>
        /// Get or Set the color of the graph line
        /// </summary>
        //public Color LineColor
        //{
        //  get
        //  {
        //    if (Settings == null)
        //      Settings = new LineGraphSettings();
        //    return Settings.LineColor;
        //  }
        //  set
        //  {
        //    if (Settings == null)
        //      Settings = new LineGraphSettings();
        //    Settings.LineColor = value;
        //    path.Stroke = new SolidColorBrush(value);
        //    refreshPath();
        //    if (VisualizationChanged != null) VisualizationChanged(this, null);
        //  }
        //}

        public event EventHandler<EventArgs> VisualizationChanged;

        #endregion
    }

    /// <summary>
    /// Provides settings which affect visual representation and behavior of the linegraph
    /// </summary>
    public class LineGraphSettings:DependencyObject { 
        public bool IsToolTipEnabled {get;set;}
        public bool ShowInLegend { get; set; }

        public Color LineColor
        {
          get { return (Color)GetValue(LineColorProperty); }
          set { SetValue(LineColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register("LineColor", typeof(Color), typeof(LineGraphSettings), new PropertyMetadata(SolidColorHelper.Next()));

      
      
      public double LineThickness { get; set; }



        public string Description
        {
          get { return (string)GetValue(DescriptionProperty); }
          set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(LineGraphSettings), new PropertyMetadata("No description"));

        


        public LineGraphSettings() {
            IsToolTipEnabled = true;
            ShowInLegend = true;
            LineColor = SolidColorHelper.Next();
            LineThickness = 2.0;
        }
    }
}
