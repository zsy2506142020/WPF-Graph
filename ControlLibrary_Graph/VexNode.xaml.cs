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
    public class VexNodeInfo : DependencyObject
    {
        //设置依赖属性
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(VexNodeInfo), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        //CLR属性包装器
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(string), typeof(VexNodeInfo), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public string Data
        {
            get { return (string)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty IndegreeProperty = DependencyProperty.Register("Indegree", typeof(int), typeof(VexNodeInfo), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public int Indegree
        {
            get { return (int)GetValue(IndegreeProperty); }
            set { SetValue(IndegreeProperty, value); }
        }

        public static readonly DependencyProperty OutdegreeProperty = DependencyProperty.Register("Outdegree", typeof(int), typeof(VexNodeInfo), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public int Outdegree
        {
            get { return (int)GetValue(OutdegreeProperty); }
            set { SetValue(OutdegreeProperty, value); }
        }

        //SetBinding包装
        public BindingExpressionBase SetBind(DependencyProperty dp, BindingBase binding)
        {
            return BindingOperations.SetBinding(this, dp, binding);
        }
    }

    /// <summary>
    /// VexNode.xaml 的交互逻辑
    /// </summary>
    public partial class VexNode : UserControl
    {
        public VexNodeInfo info;

        public VexNode()
        {
            InitializeComponent();
            info = new VexNodeInfo();
            this.indexLabel.SetBinding(Label.ContentProperty, new Binding("Index") { Source = info });
            this.dataLabel.SetBinding(Label.ContentProperty, new Binding("Data") { Source = info });
            this.indegreeLabel.SetBinding(Label.ContentProperty, new Binding("Indegree") { Source = info });
            this.outdegreeLabel.SetBinding(Label.ContentProperty, new Binding("Outdegree") { Source = info });
        }

        public void SetIndex(int index)
        {
            info.Index = index;
        }
        public void SetData(string data)
        {
            info.Data = data;
        }
        public void SetIndegree(int indegree)
        {
            info.Indegree = indegree;
        }
        public void SetOutdegree(int outdegree)
        {
            info.Outdegree = outdegree;
        }
    }
}
