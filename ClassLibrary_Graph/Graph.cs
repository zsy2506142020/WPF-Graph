using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary_Graph
{
    //顶点类
    public class Node<T>
    {
        private T data; //数据域
        //构造器
        public Node(T v)
        {
            data = v;
        }
        //数据域属性
        public T Data
        {
            get { return data; }
            set { data = value; }
        }
        //重写Equals方法
        public bool Equals(Node<T> v)
        {
            if (this.data.Equals(v.data))
                return true;
            return false;
        }
    }

    //弧类
    public class Edge<T>
    {
        private T tail;
        private T head;
        //构造器
        public Edge(T t, T h)
        {
            tail = t;
            head = h;
        }
        //数据域属性
        public T Tail
        {
            get { return tail; }
            set { tail = value; }
        }
        public T Head
        {
            get { return head; }
            set { head = value; }
        }
    }

    //图的接口
    public interface IGraph<T>
    {
        //获取顶点数
        int GetNumOfVertex();
        //获取弧的数目
        int GetNumOfEdge();
        //在两个顶点之间添加权为v的弧
        bool SetEdge(Node<T> v1, Node<T> v2, int v);
        //删除两个顶点之间的弧
        bool DelEdge(Node<T> v1, Node<T> v2);
        //判断两个顶点之间是否有弧
        bool IsEdge(Node<T> v1, Node<T> v2);
        //判断点是否在图中
        bool IsNode(Node<T> v);
        //添加顶点
        bool SetNode(Node<T> v);
        //删除顶点
        bool DelNode(Node<T> v);

    }

    //有向图邻接表类的实现
    public class adjListNode<T>
    {
        private int adjvex;//邻接顶点, adjvex为索引
        private adjListNode<T> next;//下一个邻接表结点
        private int weight; //权

        //邻接顶点属性
        public int Adjvex
        {
            get { return adjvex; }
            set { adjvex = value; }
        }

        //下一个邻接表结点属性
        public adjListNode<T> Next
        {
            get { return next; }
            set { next = value; }
        }

        //权属性
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        //构造器
        public adjListNode(int vex, int wei)
        {
            adjvex = vex;
            next = null;
            weight = wei;
        }

    }

    //有向图邻接表的顶点结点类
    public class VexNode<T>
    {
        private Node<T> data; //图的顶点
        private adjListNode<T> firstAdj; //邻接表的第1个结点
        private int indegree;//入度
        private int outdegree;//出度

        public Node<T> Data
        {
            get { return data; }
            set { data = value; }
        }

        //邻接表的第1个结点属性
        public adjListNode<T> FirstAdj
        {
            get { return firstAdj; }
            set { firstAdj = value; }
        }

        //邻接表的入度属性
        public int Indegree
        {
            get { return indegree; }
            set { indegree = value; }
        }

        //邻接表的出度属性
        public int Outdegree
        {
            get { return outdegree; }
            set { outdegree = value; }
        }

        //构造器
        public VexNode()
        {
            data = null;
            firstAdj = null;
            indegree = 0;
            outdegree = 0;
        }

        //构造器
        public VexNode(Node<T> nd)
        {
            data = nd;
            firstAdj = null;
            indegree = 0;
            outdegree = 0;
        }

        //构造器
        public VexNode(Node<T> nd, adjListNode<T> alNode)
        {
            data = nd;
            firstAdj = alNode;
            indegree = 0;
            outdegree = 0;
        }
    }

    //有向图邻接表类 
    public class GraphAdjList<T> : IGraph<T>
    {
        //邻接表数组
        private VexNode<T>[] adjList;

        //索引器
        public VexNode<T> this[int index]
        {
            get { return adjList[index]; }
            set { adjList[index] = value; }
        }

        //构造器
        public GraphAdjList()
        {
            adjList = null;
        }
        //构造器
        public GraphAdjList(Node<T> node)
        {
            adjList = new VexNode<T>[1];
            adjList[0] = new VexNode<T>();
            adjList[0].Data = node;
            adjList[0].FirstAdj = null;
            adjList[0].Indegree = 0;
            adjList[0].Outdegree = 0;
        }

        //构造器
        public GraphAdjList(T data)
        {
            Node<T> node = new Node<T>(data);
            adjList = new VexNode<T>[1];
            adjList[0] = new VexNode<T>();
            adjList[0].Data = node;
            adjList[0].FirstAdj = null;
            adjList[0].Indegree = 0;
            adjList[0].Outdegree = 0;
        }

        //构造器
        public GraphAdjList(Node<T>[] nodes)
        {
            adjList = new VexNode<T>[nodes.Length];
            for (int i = 0; i < nodes.Length; ++i)
            {
                adjList[i] = new VexNode<T>();
                adjList[i].Data = nodes[i];
                adjList[i].FirstAdj = null;
                adjList[i].Indegree = 0;
                adjList[i].Outdegree = 0;
            }
        }

        //构造器
        public GraphAdjList(GraphAdjList<T> newGraph)
        {
            adjList = new VexNode<T>[newGraph.GetNumOfVertex()];
            adjListNode<T> pre, p;
            for (int i = 0; i < newGraph.GetNumOfVertex(); ++i)
            {
                adjList[i] = new VexNode<T>();
                adjList[i].Data = new Node<T>(newGraph.adjList[i].Data.Data);
                adjList[i].Indegree = newGraph.adjList[i].Indegree;
                adjList[i].Outdegree = newGraph.adjList[i].Outdegree;

                if (newGraph.adjList[i].FirstAdj == null)
                    adjList[i].FirstAdj = null;
                else
                {
                    adjListNode<T> GraphP = newGraph.adjList[i].FirstAdj;
                    pre = new adjListNode<T>(GraphP.Adjvex, GraphP.Weight);
                    adjList[i].FirstAdj = pre;
                    while (GraphP.Next != null)
                    {
                        p = new adjListNode<T>(GraphP.Next.Adjvex, GraphP.Next.Weight);
                        pre.Next = p;
                        pre = p;
                        GraphP = GraphP.Next;
                    }
                    pre.Next = GraphP.Next;//null
                }
            }
        }

        //获取顶点的数目
        public int GetNumOfVertex()
        {
            if (adjList != null)
                return adjList.Length;
            else
                return 0;
        }

        //获取弧的数目
        public int GetNumOfEdge()
        {
            if (adjList == null)
                return 0;
            int i = 0;

            foreach (VexNode<T> nd in adjList)  //以nd为弧尾
            {
                adjListNode<T> p = nd.FirstAdj; //以p为弧头
                while (p != null)
                {
                    ++i;
                    p = p.Next;
                }
            }

            return i;//有向图
            //return i / 2;//无向图
        }

        //判断v是否是图的顶点
        public bool IsNode(Node<T> v)
        {
            if (adjList == null)
                return false;
            //遍历邻接表数组
            foreach (VexNode<T> nd in adjList)//以nd为弧尾
            {
                //如果v等于nd的data，则v是图中的顶点，返回true
                if (v.Equals(nd.Data))
                {
                    return true;
                }
            }
            return false;
        }

        //获取顶点v在邻接表数组中的索引
        public int GetIndex(Node<T> v)
        {
            if (adjList == null)
                return -1;
            int i = -1;
            //遍历邻接表数组
            for (i = 0; i < adjList.Length; ++i)
            {
                //邻接表数组第i项的data值等于v，则顶点v的索引为i
                if (v.Equals(adjList[i].Data))
                {
                    return i;
                }
            }
            return i;
        }

        //判断v1到v2是否存在弧
        public bool IsEdge(Node<T> v1, Node<T> v2)
        {
            if (adjList == null)
                return false;
            //v1或v2不是图的顶点
            if (!IsNode(v1) || !IsNode(v2))
            {
                //Console.WriteLine("Node is not belong to Graph!");
                return false;
            }
            adjListNode<T> p = adjList[GetIndex(v1)].FirstAdj;
            while (p != null)
            {
                if (p.Adjvex == GetIndex(v2))
                {
                    return true;
                }
                p = p.Next;
            }

            return false;
        }

        //在顶点v1和v2之间添加权值为v的弧
        public bool SetEdge(Node<T> v1, Node<T> v2, int v)
        {
            //v1或v2不是图的顶点或者v1和v2之间存在边
            if (!IsNode(v1) || !IsNode(v2) || IsEdge(v1, v2))
            {
                //Console.WriteLine("Node is not belong to Graph!");
                return false;
            }

            //权值不对
            if (v < 1)
            {
                //Console.WriteLine("Weight is not right!");
                return false;
            }

            //处理顶点v1的邻接表
            adjListNode<T> p = new adjListNode<T>(GetIndex(v2), v);

            if (adjList[GetIndex(v1)].FirstAdj == null)//顶点v1无邻接顶点
            {
                adjList[GetIndex(v1)].FirstAdj = p;
                adjList[GetIndex(v1)].Outdegree++;
                adjList[GetIndex(v2)].Indegree++;
            }
            else//顶点v1有邻接顶点
            {
                p.Next = adjList[GetIndex(v1)].FirstAdj;
                adjList[GetIndex(v1)].FirstAdj = p; //头插法
                adjList[GetIndex(v1)].Outdegree++;
                adjList[GetIndex(v2)].Indegree++;
            }
            return true;
        }


        //删除顶点v1和v2之间的边
        public bool DelEdge(Node<T> v1, Node<T> v2)
        {
            //v1或v2不是图的顶点
            if (!IsNode(v1) || !IsNode(v2))
            {
                //Console.WriteLine("Node is not belong to Graph!");
                return false;
            }

            //顶点v1到v2到有弧
            if (IsEdge(v1, v2))
            {
                //处理顶点v1的邻接表中的顶点v2的邻接表结点
                adjListNode<T> p = adjList[GetIndex(v1)].FirstAdj;
                adjListNode<T> pre = null;  //被删除的前一项

                //firstAdj就是v2
                if (p.Adjvex == GetIndex(v2))
                {
                    adjList[GetIndex(v1)].FirstAdj = p.Next;
                }
                else
                {
                    while (p != null)
                    {
                        if (p.Adjvex != GetIndex(v2))
                        {
                            pre = p;
                            p = p.Next;
                        }
                        else
                        {
                            pre.Next = p.Next;
                            p = p.Next;
                        }
                    }

                }
                adjList[GetIndex(v1)].Outdegree--;
                adjList[GetIndex(v2)].Indegree--;
            }
            return true;
        }

        //改变v1到v2弧的权值
        public bool ChangeEdgeWei(Node<T> v1, Node<T> v2, int v)
        {
            //v1或v2不是图的顶点或者v1和v2之间不存在边
            if (!IsNode(v1) || !IsNode(v2) || !IsEdge(v1, v2))
            {
                //Console.WriteLine("Node is not belong to Graph!");
                return false;
            }

            //权值不对
            if (v < 1)
            {
                //Console.WriteLine("Weight is not right!");
                return false;
            }

            //处理顶点v1的邻接表
            adjListNode<T> p = adjList[GetIndex(v1)].FirstAdj;

            while (p.Adjvex != GetIndex(v2))
                p = p.Next;
            p.Weight = v;
            return true;
        }

        //添加顶点
        public bool SetNode(Node<T> oldV)
        {

            //v已经在图中
            if (IsNode(oldV))
            {
                //Console.WriteLine("Node has been belong to Graph!");
                return false;
            }

            Node<T> v = new Node<T>(oldV.Data);
            VexNode<T>[] oldAdjList = adjList;
            if (oldAdjList == null)
                adjList = new VexNode<T>[1];
            else
                adjList = new VexNode<T>[adjList.Length + 1];

            //原表复制
            if (oldAdjList != null)
            {
                for (int i = 0; i < oldAdjList.Length; i++)
                {
                    adjList[i] = new VexNode<T>();
                    adjList[i].Data = oldAdjList[i].Data;
                    adjList[i].FirstAdj = oldAdjList[i].FirstAdj;
                    adjList[i].Indegree = oldAdjList[i].Indegree;
                    adjList[i].Outdegree = oldAdjList[i].Outdegree;
                }
            }

            //添加新点
            adjList[adjList.Length - 1] = new VexNode<T>();
            adjList[adjList.Length - 1].Data = v;
            adjList[adjList.Length - 1].FirstAdj = null;
            adjList[adjList.Length - 1].Indegree = 0;
            adjList[adjList.Length - 1].Outdegree = 0;

            return true;
        }

        //删除顶点
        public bool DelNode(Node<T> v)
        {
            //v不在图中
            if (!IsNode(v))
            {
                //Console.WriteLine("Node isn't belong to Graph!");
                return false;
            }

            int index = this.GetIndex(v);

            //修改原表，删除有关v的边
            for (int i = 0; i < adjList.Length; i++)
            {
                if (i == index) continue;

                //从adjlist[i]到v有弧
                if (IsEdge(adjList[i].Data, v))
                    DelEdge(adjList[i].Data, v);
                //从v到adjlist[i]有弧
                if (IsEdge(v, adjList[i].Data))
                    DelEdge(v, adjList[i].Data);
            }

            VexNode<T>[] oldAdjList = adjList;
            adjList = new VexNode<T>[adjList.Length - 1];

            //原表复制
            for (int i = 0; i < index; i++)
            {
                adjList[i] = new VexNode<T>();
                adjList[i].Data = oldAdjList[i].Data;
                adjList[i].FirstAdj = oldAdjList[i].FirstAdj;
                adjList[i].Indegree = oldAdjList[i].Indegree;
                adjList[i].Outdegree = oldAdjList[i].Outdegree;
                //修改邻接表
                for (adjListNode<T> p = adjList[i].FirstAdj; p != null; p = p.Next)
                    if (p.Adjvex > index)
                        p.Adjvex--; //当p的索引比删掉点的索引大，其值减一
            }
            for (int i = index; i < adjList.Length; i++)
            {
                adjList[i] = new VexNode<T>();
                adjList[i].Data = oldAdjList[i + 1].Data;
                adjList[i].FirstAdj = oldAdjList[i + 1].FirstAdj;
                adjList[i].Indegree = oldAdjList[i + 1].Indegree;
                adjList[i].Outdegree = oldAdjList[i + 1].Outdegree;
                //修改邻接表
                for (adjListNode<T> p = adjList[i].FirstAdj; p != null; p = p.Next)
                    if (p.Adjvex > index)
                        p.Adjvex--; //当p的索引比删掉点的索引大，其值减一
            }

            return true;
        }

        //获取索引位置的顶点
        public VexNode<T> GetVexNode(int index)
        {
            if (index < 0 || index >= adjList.Length)
                return null;
            else
                return adjList[index];
        }

        //拓扑排序
        public bool TopologicalSort(LinkedList<VexNode<T>> sort)
        {
            if (adjList == null)
                return false;
            int top = -1;//栈顶指针
            int[] count;
            VexNode<T> pv;
            adjListNode<T> pa;

            count = new int[adjList.Length];

            for (int i = 0; i < adjList.Length; i++)
                count[i] = adjList[i].Indegree;

            for (int i = 0; i < adjList.Length; i++) //栈中加入入度为零的顶点
            {
                if (count[i] == 0)
                {
                    count[i] = top;
                    top = i;
                }
            }

            for (int i = 0; i < adjList.Length; i++)//循环结点数次
            {
                if (top == -1)//若无有向环，top应在最后的循环最后才置-1
                {
                    return false;
                }

                pv = adjList[top];  //选定栈顶结点做pv
                top = count[top];   //pv出栈
                sort.AddLast(pv);   //排序队列中添加结点pv
                pa = pv.FirstAdj;   //对以pv为弧尾的弧的弧头pa遍历

                while (pa != null)
                {
                    if (--count[pa.Adjvex] == 0) //以pa为弧头的弧只有从pv来的一条
                    {
                        count[pa.Adjvex] = top; //pa入栈
                        top = pa.Adjvex;
                    }
                    pa = pa.Next;
                }
            }

            return true;
        }

        //关键路径
        public void CriticalPath(int[] e, int[] l, int[] sub, bool[] flag, Edge<T>[] edges, int[] ve, int[] vl, int[] topo_sort_index)
        {//topo_sort_index是拓扑排序的索引顺序
            if (adjList == null)
                return;
            int topo_sort_i;//拓扑排序的i
            int el_i = 0;//e，l数组的i
            adjListNode<T> p;
            int p_index;//p结点在图中索引
            //ve
            for (topo_sort_i = 0; topo_sort_i < adjList.Length; topo_sort_i++)
                ve[topo_sort_index[topo_sort_i]] = 0;
            for (topo_sort_i = 0; topo_sort_i < adjList.Length; topo_sort_i++)//拓扑正序
            {
                p = adjList[topo_sort_index[topo_sort_i]].FirstAdj;
                while (p != null)   //p遍历adjList[topo_sort_index[topo_sort_i]]的对应的弧头
                {
                    p_index = p.Adjvex;
                    if (ve[topo_sort_index[topo_sort_i]] + p.Weight > ve[p_index])
                        ve[p_index] = ve[topo_sort_index[topo_sort_i]] + p.Weight;
                    p = p.Next;
                }
            }
            //vl
            for (topo_sort_i = 0; topo_sort_i < adjList.Length; topo_sort_i++)
                vl[topo_sort_index[topo_sort_i]] = ve[topo_sort_index[adjList.Length - 1]];
            vl[0] = 0;
            for (topo_sort_i = adjList.Length - 2; topo_sort_i > 0; topo_sort_i--)//拓扑逆序
            {
                p = adjList[topo_sort_index[topo_sort_i]].FirstAdj;
                while (p != null)  //p遍历adjList[topo_sort_index[topo_sort_i]]的对应的弧头
                {
                    p_index = p.Adjvex;
                    if (vl[p_index] - p.Weight < vl[topo_sort_index[topo_sort_i]])
                        vl[topo_sort_index[topo_sort_i]] = vl[p_index] - p.Weight;
                    p = p.Next;
                }
            }

            //e,l，不必按照拓扑排序,此处以弧尾索引的升序做主序，邻接表结点逻辑顺序为次序
            for (int i = 0; i < adjList.Length; i++)
            {
                p = adjList[i].FirstAdj;
                while (p != null)
                {
                    p_index = p.Adjvex;
                    e[el_i] = ve[i];
                    l[el_i] = vl[p_index] - p.Weight;
                    sub[el_i] = l[el_i] - e[el_i];
                    flag[el_i] = (sub[el_i] == 0) ? true : false;
                    edges[el_i] = new Edge<T>(adjList[i].Data.Data, adjList[p.Adjvex].Data.Data);
                    p = p.Next;
                    el_i++;
                }
            }

        }
    }

}
