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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace algorithm_implement
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MakeGraphWindow : Window
    {
        GraphAdjList<string> graph;
        LinkedList<VexNode<string>> topoSort ;
        int[] e, l, sub;
        bool[] flag;//true为关键路径
        Edge<string>[] edges;
        int[] ve, vl, topo_sort_index;
        Image image;
        Boolean NoCircular;
        public MakeGraphWindow()
        {
            InitializeComponent();
        }
        private void ButtonClick_SubmitGraph(object sender, RoutedEventArgs e)
        {

            this.graph = new ClassLibrary_Graph.GraphAdjList<string>(this.graphShowControl.Graph);

            topoSort = new LinkedList<VexNode<string>>();
            ShowGraphAdjListShowWindow();
            ShowCriticalPathWindow();
            
            ShowTopologicalSortWindow();
        }
        private void ShowGraphAdjListShowWindow()
        {
            GraphAdjListShowWindow nextGraphAdjListShowWindow = new GraphAdjListShowWindow();
            nextGraphAdjListShowWindow.SetGraph(graph);
            nextGraphAdjListShowWindow.ShowDialog();
        }

        private void ShowCriticalPathWindow()
        {
            NoCircular = graph.TopologicalSort(topoSort);
            if (!NoCircular)
            {
                MessageBox.Show("存在有向环，求关键路径失败");
                return;
            }

            GraphOperate();

            GenerateImage();

            InitCPWindow();
        }

        private void ShowTopologicalSortWindow()
        {
            //拓扑排序
            if (!NoCircular)
            {
                MessageBox.Show("存在有向环，拓扑排序失败");
                return;
            }

            TopologicalSortWindow nextTopologicalSortWindow = new TopologicalSortWindow();

            //按顺序删去点，每次都保存bitmap
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)this.graphShowControl.ActualWidth, (int)this.graphShowControl.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(this.graphShowControl);
            nextTopologicalSortWindow.AddStep(bitmap, graph);
            foreach (VexNode<string> node in topoSort)
            {
                this.graphShowControl.DelGrid(node.Data.Data);
                graph.DelNode(node.Data);

                bitmap = new RenderTargetBitmap((int)this.graphShowControl.ActualWidth, (int)this.graphShowControl.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(this.graphShowControl);

                if(graph.GetNumOfVertex()!=0)
                    nextTopologicalSortWindow.AddStep(bitmap, graph);
            }

            nextTopologicalSortWindow.ShowDialog();
        }

        private void GraphOperate()
        {

            e = new int[graph.GetNumOfEdge()];
            l = new int[graph.GetNumOfEdge()];
            sub = new int[graph.GetNumOfEdge()];
            flag = new bool[graph.GetNumOfEdge()];
            edges = new Edge<string>[graph.GetNumOfEdge()];
            ve = new int[graph.GetNumOfVertex()];
            vl = new int[graph.GetNumOfVertex()];
            topo_sort_index = new int[graph.GetNumOfVertex()];

            int i = 0;
            foreach (VexNode<string> node in topoSort)
            {
                topo_sort_index[i++] = graph.GetIndex(node.Data);
            }

            graph.CriticalPath(e, l, sub, flag, edges, ve, vl, topo_sort_index);

            i = 0;
            foreach (Edge<string> edge in edges)
            {
                string arrowName;
                if (flag[i++])
                {
                    arrowName = edge.Tail + "-" + edge.Head;
                    this.graphShowControl.MarkArrow(arrowName);
                }
            }
        }
        private void GenerateImage()
        {
            this.graphShowControl.UpdateLayout();
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)this.graphShowControl.ActualWidth, (int)this.graphShowControl.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(this.graphShowControl);

            image = new Image();
            image.Width = bitmap.Width;
            image.Height = bitmap.Height;
            image.Source = bitmap;
        }
        private void InitCPWindow()
        {
            CriticalPathWindow nextCriticalPathWindow = new CriticalPathWindow();
            nextCriticalPathWindow.Set(graph, image, this.e, l, sub, flag, edges, ve, vl, topo_sort_index);
            
            nextCriticalPathWindow.ShowDialog();

            int i = 0;
            foreach (Edge<string> edge in edges)
            {
                string arrowName;
                if (flag[i++])
                {
                    arrowName = edge.Tail + "-" + edge.Head;
                    this.graphShowControl.UnMarkArrow(arrowName);
                }
            }
            this.graphShowControl.UpdateLayout();
        }
    }
}
