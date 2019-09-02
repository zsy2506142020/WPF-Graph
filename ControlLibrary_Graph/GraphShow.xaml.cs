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
    /// GraphShow.xaml 的交互逻辑
    /// </summary>
    public partial class GraphShow : UserControl
    {
        Boolean IsDrawing;//正在绘制圆，左击ing
        Boolean IsDragging;//正在拖拽圆，右击ing
        Boolean IsInGrid;//在圆内
        Boolean IsInArrow;//在箭头区域内
        Boolean IsConnecting;//正在连箭头，左击ing
        Boolean IsSettingArrow;//正在设置箭头，且光标移出起始圆

        Ellipse elips;//圆
        Label label;//圆标签
        Grid grid;//包含圆与标签的grid
        Point ptcenter;
        Point ptmousestart, ptElementStart;
        ClassLibrary_Graph.Arrow arrow;//箭头
        FrameworkElement elDragging;
        ClassLibrary_Graph.Arrow ChangeArrow;//正在使用的arrow
        Grid ChangeGrid;//正在使用的grid
        TextBox ChangeTextBox;

        Dictionary<string, Grid> gridDic;
        Dictionary<string, ClassLibrary_Graph.Arrow> arrowDic;
        Dictionary<string, TextBox> textBoxDic;

        ClassLibrary_Graph.GraphAdjList<string> graph;//对应的图

        int pointNamesNum = 0;
        public string[] pointNames =
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
            "AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ"
            //暂时设52个
        };

        public GraphShow()
        {
            InitializeComponent();
            gridDic = new Dictionary<string, Grid>();
            arrowDic = new Dictionary<string, ClassLibrary_Graph.Arrow>();
            textBoxDic = new Dictionary<string, TextBox>();
            graph = new ClassLibrary_Graph.GraphAdjList<string>();
        }

        void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeGrid = sender as Grid;
            IsInGrid = true;
            if (IsConnecting)
            {//当光标进入一个圆中，箭头不动
                double x2 = Canvas.GetLeft(ChangeGrid) + ChangeGrid.Width / 2;//箭尾为所在圆的圆心
                double y2 = Canvas.GetTop(ChangeGrid) + ChangeGrid.Height / 2;
                double theta = Math.Atan2(arrow.Y1 - y2, arrow.X1 - x2);

                arrow.X2 = x2 + ChangeGrid.Width / 2 * Math.Cos(theta);
                arrow.Y2 = y2 + ChangeGrid.Width / 2 * Math.Sin(theta);
            }
        }

        void grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeGrid = sender as Grid;
            ChangeGrid = null;
            if (IsInGrid)
            {
                IsInGrid = false;
            }
            //throw new NotImplementedException();
            if (IsConnecting)
            {
                arrow.X2 = e.GetPosition(this.GraphShowCanvas).X;
                arrow.Y2 = e.GetPosition(this.GraphShowCanvas).Y;
                if (IsSettingArrow)
                {//当光标离开起点圆时，将箭头添加至Canvas，若是其他圆则不添加，防止重复
                    Panel.SetZIndex(arrow, 0);//箭头的z顺序在圆之下
                    this.GraphShowCanvas.Children.Add(arrow);
                    IsSettingArrow = false;
                }
            }
            if (IsDragging)
            {
                IsDragging = false;
            }

        }

        void arrow_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeArrow = sender as ClassLibrary_Graph.Arrow;
            IsInArrow = true;
        }

        void arrow_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeArrow = sender as ClassLibrary_Graph.Arrow;
            ChangeArrow = null;
            if (IsInArrow)
            {
                IsInArrow = false;
            }
        }

        void textBox_TextChange(object sender, TextChangedEventArgs e)
        {
            ChangeTextBox = sender as TextBox;

            var textBoxNamevar = from t in textBoxDic where t.Value == ChangeTextBox select t.Key;
            string textBoxName = textBoxNamevar.FirstOrDefault();
            ClassLibrary_Graph.Node<string> node1 = new ClassLibrary_Graph.Node<string>(arrowDic[textBoxName].Tail);
            ClassLibrary_Graph.Node<string> node2 = new ClassLibrary_Graph.Node<string>(arrowDic[textBoxName].Head);
            graph.ChangeEdgeWei(node1, node2, int.Parse(ChangeTextBox.Text));
        }
        //点击左键，定义圆的圆心并绘制
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            //执行基本的方法
            base.OnMouseLeftButtonDown(e);

            //连箭头
            if (IsInGrid == true)
            {
                arrow = new ClassLibrary_Graph.Arrow();
                arrow.HeadHeight = 10;
                arrow.HeadWidth = 10;
                arrow.Stroke = Brushes.Black;
                arrow.StrokeThickness = 3;
                arrow.Tail = ChangeGrid.Name;

                arrow.X1 = Canvas.GetLeft(ChangeGrid) + ChangeGrid.Width / 2;//箭尾为所在圆的圆心
                arrow.Y1 = Canvas.GetTop(ChangeGrid) + ChangeGrid.Height / 2;

                //添加路由事件
                arrow.MouseEnter += new MouseEventHandler(arrow_MouseEnter);
                arrow.MouseLeave += new MouseEventHandler(arrow_MouseLeave);

                IsConnecting = true;
                IsSettingArrow = true;

                return;
            }

            //对箭头进行左击，不添加新圆
            if (IsInArrow == true)
            {
                return;
            }
            //创建一个grid对象，存放圆与标签
            grid = new Grid();
            grid.Width = 40;
            grid.Height = 40;
            ptcenter = e.GetPosition(this.GraphShowCanvas);//返回相对canvas的点位置
            grid.Name = pointNames[pointNamesNum];

            //创建一个Ellipse对象，并且把它加入到grid中
            elips = new Ellipse();//新建一个椭圆对象
            elips.Stroke = SystemColors.WindowTextBrush;
            elips.Width = 40;
            elips.Height = 40;//固定直径为40
            elips.Fill = Brushes.White;
            grid.Children.Add(elips);

            //创建一个label对象，并把它加入grid中
            label = new Label();
            label.Content = pointNames[pointNamesNum++];
            label.HorizontalAlignment = HorizontalAlignment.Center;//居中
            label.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(label);

            //添加路由事件
            grid.MouseEnter += new MouseEventHandler(grid_MouseEnter);
            grid.MouseLeave += new MouseEventHandler(grid_MouseLeave);
            grid.MouseRightButtonDown += new MouseButtonEventHandler(grid_MouseRightButtonDown);

            //grid添加到Canvas和gridDic和Graph中
            this.GraphShowCanvas.Children.Add(grid);
            gridDic.Add(grid.Name, grid);
            Panel.SetZIndex(grid, 1);
            Canvas.SetLeft(grid, ptcenter.X - grid.Width / 2);//ptcenter的(x,y)为elips的圆心坐标
            Canvas.SetTop(grid, ptcenter.Y - grid.Height / 2);
            ClassLibrary_Graph.Node<string> node = new ClassLibrary_Graph.Node<string>(label.Content.ToString());
            graph.SetNode(node);

            //获取鼠标，即使鼠标离开区域也可获取鼠标消息
            CaptureMouse();
            IsDrawing = true;

        }

        //单击右键进行拖拽，第一种方法不成功，暂用第二个方法
        //protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseRightButtonDown(e);
        //    //if (IsDrawing)
        //    //{
        //    //    return;
        //    //}
        //    ////得到点击的事件 为未来做准备
        //    //ptmousestart = e.GetPosition(this.GraphShowCanvas);
        //    //elDragging = this.GraphShowCanvas.InputHitTest(ptmousestart) as FrameworkElement;
        //    //此处elDragging是grid的child而不是grid本身，导致canvas.getleft无法读取
        //    //if (elDragging != null)
        //    //{
        //    //    //ptElementStart = new Point((double)elDragging.GetValue(Canvas.LeftProperty), (double)elDragging.GetValue(Canvas.TopProperty));
        //    //    ptElementStart = new Point(Canvas.GetLeft(elDragging), Canvas.GetTop(elDragging));
        //    //    IsDragging = true;

        //    //}

        //}
        private void grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //base.OnMouseRightButtonDown(e);
            if (IsDrawing)
            {
                return;
            }
            //得到点击的事件 为未来做准备
            ptmousestart = e.GetPosition(this.GraphShowCanvas);
            elDragging = sender as FrameworkElement;
            if (elDragging != null)
            {
                //ptElementStart = new Point((double)elDragging.GetValue(Canvas.LeftProperty), (double)elDragging.GetValue(Canvas.TopProperty));
                ptElementStart = new Point(Canvas.GetLeft(elDragging), Canvas.GetTop(elDragging));
                if (IsInGrid)
                {//对圆右键单击
                    IsDragging = true;
                }
            }
        }

        //双击右键删除圆或箭头
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.ChangedButton == MouseButton.Right && IsInGrid == true)
            {
                DelGrid(ChangeGrid.Name);
                ////删除圆
                //gridDic.Remove(ChangeGrid.Name);
                //this.GraphShowCanvas.Children.Remove(ChangeGrid);
                //ClassLibrary_Graph.Node<string> node = new ClassLibrary_Graph.Node<string>(ChangeGrid.Name);
                //graph.DelNode(node);

                ////删除相关的连线及textBox
                ////以其为尾的arrow：
                //string prefix = ChangeGrid.Name + "-";
                //List<string> DelKeys = new List<string>();//由于在foreach循环中无法使用remove，需要循环后再删除
                ////ClassLibrary_Graph.Node<string> node1 = new ClassLibrary_Graph.Node<string>("0");
                ////ClassLibrary_Graph.Node<string> node2 = new ClassLibrary_Graph.Node<string>("0");
                //foreach (string Key in arrowDic.Keys)
                //{
                //    if (Key.StartsWith(prefix))
                //    {
                //        DelKeys.Add(Key);
                //    }
                //}
                ////node1.Data = ChangeGrid.Name;
                //foreach (string delKey in DelKeys)
                //{
                //    //node2.Data = arrowDic[delKey].Head;
                //    //Graph.DelEdge(node1, node2);//Graph类里删除顶点不用再删除边
                //    this.GraphShowCanvas.Children.Remove(arrowDic[delKey]);
                //    arrowDic.Remove(delKey);

                //    this.GraphShowCanvas.Children.Remove(textBoxDic[delKey]);
                //    textBoxDic.Remove(delKey);
                //}
                //DelKeys.Clear();
                ////以其为头的arrow
                //string suffix = "-" + ChangeGrid.Name;
                //foreach (string Key in arrowDic.Keys)
                //{
                //    if (Key.EndsWith(suffix))
                //    {
                //        DelKeys.Add(Key);
                //    }
                //}
                ////node2.Data = ChangeGrid.Name;
                //foreach (string delKey in DelKeys)
                //{
                //    //node1.Data = arrowDic[delKey].Tail;
                //    //Graph.DelEdge(node1, node2);
                //    this.GraphShowCanvas.Children.Remove(arrowDic[delKey]);
                //    arrowDic.Remove(delKey);

                //    this.GraphShowCanvas.Children.Remove(textBoxDic[delKey]);
                //    textBoxDic.Remove(delKey);
                //}
            }
            else if (e.ChangedButton == MouseButton.Right && IsInArrow == true)
            {//删除箭头
                ClassLibrary_Graph.Node<string> node1 = new ClassLibrary_Graph.Node<string>(ChangeArrow.Tail);
                ClassLibrary_Graph.Node<string> node2 = new ClassLibrary_Graph.Node<string>(ChangeArrow.Head);
                graph.DelEdge(node1, node2);
                string arrowName = ChangeArrow.Tail + "-" + ChangeArrow.Head;
                arrowDic.Remove(arrowName);
                this.GraphShowCanvas.Children.Remove(ChangeArrow);

                //删除textBox
                this.GraphShowCanvas.Children.Remove(textBoxDic[arrowName]);
                textBoxDic.Remove(arrowName);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsDrawing && e.ChangedButton == MouseButton.Left)
            {
                IsDrawing = false;
                ReleaseMouseCapture();
            }
            else if (IsDragging && e.ChangedButton == MouseButton.Right)
            {
                IsDragging = false;
            }
            else if (!IsInGrid && IsConnecting && e.ChangedButton == MouseButton.Left)
            {//未在圆内释放
                IsConnecting = false;
                this.GraphShowCanvas.Children.Remove(arrow);
            }
            else if (IsInGrid && IsConnecting && e.ChangedButton == MouseButton.Left)
            {//在圆内释放
                IsConnecting = false;
                arrow.Head = ChangeGrid.Name;
                if (arrow.Tail == arrow.Head)
                {//起始点同圆，箭头失效
                    this.GraphShowCanvas.Children.Remove(arrow);
                }
                else
                {//将arrow添加arrowDic中
                    string arrowName = arrow.Tail + "-" + arrow.Head;
                    if (arrowDic.ContainsKey(arrowName))
                    {//已经存在从此尾到此头的箭头
                        this.GraphShowCanvas.Children.Remove(arrow);
                    }
                    else
                    {   //将箭头添加到arrowDic和Graph中
                        arrowDic.Add(arrowName, arrow);
                        ClassLibrary_Graph.Node<string> node1 = new ClassLibrary_Graph.Node<string>(arrow.Tail);
                        ClassLibrary_Graph.Node<string> node2 = new ClassLibrary_Graph.Node<string>(arrow.Head);
                        graph.SetEdge(node1, node2, 1);

                        //textbox用于输入权值
                        TextBox textBox = new TextBox();
                        textBoxDic.Add(arrowName, textBox);
                        this.GraphShowCanvas.Children.Add(textBox);
                        Canvas.SetLeft(textBox, (arrow.X1 + arrow.X2) / 2);
                        Canvas.SetTop(textBox, (arrow.Y1 + arrow.Y2) / 2);
                        //添加路由事件，当textbox内容改变时，同时改变这条边的权值
                        textBox.TextChanged += new TextChangedEventHandler(textBox_TextChange); ;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //执行基本的方法
            base.OnMouseMove(e);
            Point ptmouse = e.GetPosition(this.GraphShowCanvas);

            if (IsDrawing)//正在绘制圆
            {

            }
            //移动椭圆
            else if (IsDragging)
            {
                //改变椭圆位置
                Canvas.SetLeft(elDragging, ptElementStart.X + ptmouse.X - ptmousestart.X);
                Canvas.SetTop(elDragging, ptElementStart.Y + ptmouse.Y - ptmousestart.Y);
                //改变其相连箭头及其textBox的位置
                //以其为尾的arrow：
                string prefix = ChangeGrid.Name + "-";
                foreach (string Key in arrowDic.Keys)
                {
                    if (Key.StartsWith(prefix))
                    {
                        arrowDic[Key].X1 = Canvas.GetLeft(ChangeGrid) + ChangeGrid.Width / 2;//箭尾为所在圆的圆心
                        arrowDic[Key].Y1 = Canvas.GetTop(ChangeGrid) + ChangeGrid.Height / 2;

                        double theta = Math.Atan2(arrowDic[Key].Y1 - arrowDic[Key].Y2, arrowDic[Key].X1 - arrowDic[Key].X2);
                        arrowDic[Key].X2 =
                            Canvas.GetLeft(gridDic[Key.Remove(0, ChangeGrid.Name.Length + 1)])
                            + ChangeGrid.Width / 2
                            + ChangeGrid.Width / 2 * Math.Cos(theta);
                        arrowDic[Key].Y2 =
                            Canvas.GetTop(gridDic[Key.Remove(0, ChangeGrid.Name.Length + 1)])
                            + ChangeGrid.Height / 2
                            + ChangeGrid.Width / 2 * Math.Sin(theta);

                        Canvas.SetLeft(textBoxDic[Key], (arrowDic[Key].X1 + arrowDic[Key].X2) / 2);
                        Canvas.SetTop(textBoxDic[Key], (arrowDic[Key].Y1 + arrowDic[Key].Y2) / 2);
                    }
                }
                //以其为头的arrow
                string suffix = "-" + ChangeGrid.Name;
                foreach (string Key in arrowDic.Keys)
                {
                    if (Key.EndsWith(suffix))
                    {
                        double x2 = Canvas.GetLeft(ChangeGrid) + ChangeGrid.Width / 2;//箭尾为所在圆的圆心
                        double y2 = Canvas.GetTop(ChangeGrid) + ChangeGrid.Height / 2;
                        double theta = Math.Atan2(arrowDic[Key].Y1 - y2, arrowDic[Key].X1 - x2);

                        arrowDic[Key].X2 = x2 + ChangeGrid.Width / 2 * Math.Cos(theta);
                        arrowDic[Key].Y2 = y2 + ChangeGrid.Width / 2 * Math.Sin(theta);

                        Canvas.SetLeft(textBoxDic[Key], (arrowDic[Key].X1 + arrowDic[Key].X2) / 2);
                        Canvas.SetTop(textBoxDic[Key], (arrowDic[Key].Y1 + arrowDic[Key].Y2) / 2);
                    }
                }

            }
            //连箭头
            else if (IsConnecting && !IsInGrid)
            {//实时改变箭头坐标
                arrow.X2 = e.GetPosition(this.GraphShowCanvas).X;
                arrow.Y2 = e.GetPosition(this.GraphShowCanvas).Y;
            }
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
            if (e.Text.IndexOf('\x18') != -1)
            {
                if (IsDrawing)
                {
                    ReleaseMouseCapture();
                }
                else if (IsDragging)
                {
                    Canvas.SetLeft(elDragging, ptElementStart.X);
                    Canvas.SetTop(elDragging, ptElementStart.Y);
                    IsDragging = false;
                }
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            if (IsDrawing)
            {
                this.GraphShowCanvas.Children.Remove(elips);
                IsDrawing = false;
            }
        }

        public void DelGrid(string delName)
        {
            //删除圆
            this.GraphShowCanvas.Children.Remove(gridDic[delName]);
            gridDic.Remove(delName);
            ClassLibrary_Graph.Node<string> node = new ClassLibrary_Graph.Node<string>(delName);
            graph.DelNode(node);

            //删除相关的连线及textBox
            //以其为尾的arrow：
            string prefix = delName + "-";
            List<string> DelKeys = new List<string>();//由于在foreach循环中无法使用remove，需要循环后再删除
            foreach (string Key in arrowDic.Keys)
            {
                if (Key.StartsWith(prefix))
                {
                    DelKeys.Add(Key);
                }
            }
            foreach (string delKey in DelKeys)
            {
                this.GraphShowCanvas.Children.Remove(arrowDic[delKey]);
                arrowDic.Remove(delKey);

                this.GraphShowCanvas.Children.Remove(textBoxDic[delKey]);
                textBoxDic.Remove(delKey);
            }
            DelKeys.Clear();
            //以其为头的arrow
            string suffix = "-" + delName;
            foreach (string Key in arrowDic.Keys)
            {
                if (Key.EndsWith(suffix))
                {
                    DelKeys.Add(Key);
                }
            }
            foreach (string delKey in DelKeys)
            {
                this.GraphShowCanvas.Children.Remove(arrowDic[delKey]);
                arrowDic.Remove(delKey);

                this.GraphShowCanvas.Children.Remove(textBoxDic[delKey]);
                textBoxDic.Remove(delKey);
            }
        }

        public void MarkArrow(string markName)
        {
            arrowDic[markName].Stroke = Brushes.Red;
        }

        public void UnMarkArrow(string markName)
        {
            arrowDic[markName].Stroke = Brushes.Black;
        }

        public ClassLibrary_Graph.GraphAdjList<string> Graph
        {
            get { return graph; }
            set { this.graph = new ClassLibrary_Graph.GraphAdjList<string>(value); }
        }

    }
}
