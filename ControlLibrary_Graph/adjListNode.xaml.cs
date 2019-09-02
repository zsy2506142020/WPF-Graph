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
    public class adjListNodeInfo : DependencyObject
    {
        //设置依赖属性
        public static readonly DependencyProperty AdjVexProperty = DependencyProperty.Register("AdjVex", typeof(int), typeof(adjListNodeInfo), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        //CLR属性包装器
        public int AdjVex
        {
            get { return (int)GetValue(AdjVexProperty); }
            set { SetValue(AdjVexProperty, value); }
        }

        public static readonly DependencyProperty WeightProperty = DependencyProperty.Register("Weight", typeof(int), typeof(adjListNodeInfo), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public int Weight
        {
            get { return (int)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        //SetBinding包装
        public BindingExpressionBase SetBind(DependencyProperty dp, BindingBase binding)
        {
            return BindingOperations.SetBinding(this, dp, binding);
        }
    }

    /// <summary>
    /// adjListNode.xaml 的交互逻辑
    /// </summary>
    public partial class adjListNode : UserControl
    {
        public adjListNodeInfo info;

        public adjListNode()
        {
            InitializeComponent();
            info = new adjListNodeInfo();
            this.adjVexLabel.SetBinding(Label.ContentProperty, new Binding("AdjVex") { Source = info });
            this.weiLabel.SetBinding(Label.ContentProperty, new Binding("Weight") { Source = info });
        }
        public void SetAdjVex(int adjVex)
        {
            info.AdjVex = adjVex;
        }
        public void SetWeight(int weight)
        {
            info.Weight = weight;
        }
    }
}
