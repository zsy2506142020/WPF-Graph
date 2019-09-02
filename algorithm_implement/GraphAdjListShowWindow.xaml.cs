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
    /// GraphAdjListShowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GraphAdjListShowWindow : Window
    {
        public GraphAdjListShowWindow()
        {
            InitializeComponent();
        }
        public void SetGraph(ClassLibrary_Graph.GraphAdjList<string> graph)
        {
            this.graphAdjListControl.Graph = graph;
        }
    }
}
