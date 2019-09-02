using ClassLibrary_Graph;
using System;
using System.Collections.Generic;
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

namespace algorithm_implement
{
    /// <summary>
    /// TopologicalSortWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TopologicalSortWindow : Window
    {
        LinkedList<Grid> grids = new LinkedList<Grid>();
        public TopologicalSortWindow()
        {
            InitializeComponent();
        }
        public void AddStep(RenderTargetBitmap bitmap, GraphAdjList<string> graph)
        {
            Grid grid = new Grid();
            ColumnDefinition newColumnDefinition = new ColumnDefinition();
            //newColumnDefinition.Width = 1;
            grid.ColumnDefinitions.Add(newColumnDefinition);
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Image image = new Image();
            image.Width = bitmap.Width/2;
            image.Height = bitmap.Height/2;
            image.Source = bitmap;
            grid.Children.Add(image);
            Grid.SetColumn(image, 0);
            
            ControlLibrary_Graph.GraphAdjListShow graphAdjListShow = new ControlLibrary_Graph.GraphAdjListShow();
            graphAdjListShow.Graph = graph;
            grid.Children.Add(graphAdjListShow);
            Grid.SetColumn(graphAdjListShow, 1);

            grids.AddLast(grid);
            this.topologicalSortImages.ItemsSource = grids;
        }
    }
}
