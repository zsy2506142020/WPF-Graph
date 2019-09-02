using ClassLibrary_Graph;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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
    public class VexCPInfo
    {
        public string Data { get; set; }
        public string Ve { get; set; }
        public string Vl { get; set; }
    }
    public class EdgeCPInfo
    {
        public string Edge { get; set; }
        public string E { get; set; }
        public string L { get; set; }
        public string Sub { get; set; }
    }

    /// <summary>
    /// CriticalPathWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CriticalPathWindow : Window
    {
        int[] e, l, sub;
        bool[] flag;//true为关键路径
        Edge<string>[] edges;
        int[] ve, vl, topo_sort_index;
        ObservableCollection<VexCPInfo> vexCPData = new ObservableCollection<VexCPInfo>();
        ObservableCollection<EdgeCPInfo> EdgeCPData = new ObservableCollection<EdgeCPInfo>();

        public CriticalPathWindow()
        {
            InitializeComponent();
            VexDataGrid.ItemsSource = vexCPData;
            EdgeDataGrid.ItemsSource = EdgeCPData;
        }
        public void Set(GraphAdjList<string> graph,Image image, int[] e, int[] l, int[] sub, bool[] flag, Edge<string>[] edges, int[] ve, int[] vl, int[] topo_sort_index)
        {
            this.CriticalPathGrid.Children.Add(image);
            Grid.SetColumn(image, 1);
            Grid.SetRow(image, 1);

            this.e = e;this.l = l;this.sub = sub;
            this.flag = flag;this.edges = edges;
            this.ve = ve;this.vl = vl;this.topo_sort_index = topo_sort_index;

            for(int i = 0; i < graph.GetNumOfVertex(); i++)
            {
                VexCPInfo newVexInfo = new VexCPInfo();
                newVexInfo.Data = graph.GetVexNode(i).Data.Data;
                newVexInfo.Ve = ve[i].ToString();
                newVexInfo.Vl = vl[i].ToString();

                vexCPData.Add(newVexInfo);
            }

            for (int i = 0; i < graph.GetNumOfEdge(); i++)
            {
                EdgeCPInfo newEdgeInfo = new EdgeCPInfo();
                
                newEdgeInfo.Edge = edges[i].Tail + "-" + edges[i].Head;
                newEdgeInfo.E = e[i].ToString();
                newEdgeInfo.L = l[i].ToString();
                newEdgeInfo.Sub = sub[i].ToString();

                EdgeCPData.Add(newEdgeInfo);
            }

            //VexDataGrid.DataContext = vexCPData;
            //EdgeDataGrid.DataContext = EdgeCPData;
            VexDataGrid.ItemsSource = vexCPData;
            EdgeDataGrid.ItemsSource = EdgeCPData;
        }
    }
}
