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

namespace ControlLibrary_Graph
{
    /// <summary>
    /// GraphAdjListShow.xaml 的交互逻辑
    /// </summary>
    public partial class GraphAdjListShow : UserControl
    {
        ClassLibrary_Graph.GraphAdjList<string> graph;//对应的图
        StackPanel adjListStackPanel;

        public GraphAdjListShow()
        {
            InitializeComponent();
        }

        private void SetVexNodes()
        {
            for (int i = 0; i < graph.GetNumOfVertex(); i++)
            {
                ClassLibrary_Graph.VexNode<string> vexNode = graph.GetVexNode(i);
                ControlLibrary_Graph.VexNode vexNodeControl = new ControlLibrary_Graph.VexNode();

                vexNodeControl.SetIndex(i);
                vexNodeControl.SetData(vexNode.Data.Data);
                vexNodeControl.SetIndegree(vexNode.Indegree);
                vexNodeControl.SetOutdegree(vexNode.Outdegree);
                this.VexNodeStackPanel.Children.Add(vexNodeControl);

                adjListStackPanel = new StackPanel();
                adjListStackPanel.Orientation = Orientation.Horizontal;
                adjListStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                SetAdjListNodes(vexNode, i);
                this.allAdjListStackPanel.Children.Add(adjListStackPanel);

                addLine(123, 42 * i + 21, 155, 42 * i + 21);

            }
        }

        private void SetAdjListNodes(ClassLibrary_Graph.VexNode<string> vexNode, int index)
        {
            int i = 0;//计数
            for (ClassLibrary_Graph.adjListNode<string> adjListNode = vexNode.FirstAdj; adjListNode != null; adjListNode = adjListNode.Next, i++)
            {
                ControlLibrary_Graph.adjListNode adjListNodeControl = new ControlLibrary_Graph.adjListNode();

                adjListNodeControl.SetAdjVex(adjListNode.Adjvex);
                adjListNodeControl.SetWeight(adjListNode.Weight);

                adjListStackPanel.Children.Add(adjListNodeControl);

                addLine(150 + 65 + 82 * i, 42 * index + 21, 150 + 5 + 82 + 82 * i, 42 * index + 21);
            }
            nullNode nullNodeControl = new nullNode();
            adjListStackPanel.Children.Add(nullNodeControl);

        }

        private void addLine(double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            line.StrokeThickness = 2;
            line.Stroke = Brushes.Black;
            this.GraphAdjListShowCanvas.Children.Add(line);
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
        }

        public ClassLibrary_Graph.GraphAdjList<string> Graph
        {
            get { return graph; }
            set
            {
                this.graph = new ClassLibrary_Graph.GraphAdjList<string>(value);
                SetVexNodes();
            }
        }
    }
}
