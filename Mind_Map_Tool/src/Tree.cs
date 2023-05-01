using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mind_Map_Tool.src.Tree;

namespace Mind_Map_Tool.src
{
    internal class Tree
    {
        public class Node
        {
            // 元素
            public TreeInfo Item { get; set; }

            // 上一个结点
            public List<Node>? LastNodes { get; set; }

            public Node(TreeInfo Item)
            { 
                this.Item = Item;
            }

            public void AddNode(Node node) 
            {
                TreeInfo oldTreeInfo = this.Item;
                TreeInfo newTreeInfo = node.Item;
                string oldFatherCodeList = oldTreeInfo.fatherCode;
                if (this.LastNodes == null) 
                {
                    this.LastNodes = new List<Node>();
                }
                List<Node> lastNodes = this.LastNodes;
                if (null != oldFatherCodeList) 
                {
                    List<string> oldFatherCodes = oldFatherCodeList.Split(',').ToList();
                    if (oldFatherCodes.Contains(newTreeInfo.labelCode))
                    {
                        // 说明新结点需要添加到父结点
                        lastNodes.Add(node);
                    }
                    else
                    {
                        // 那么需要更深一层
                        foreach (Node item in lastNodes)
                        {
                            item.AddNode(node);
                        }
                    }
                }
            }
        }

        // 头节点
        public Node Head { get; set; }

        /// <summary>
        ///     添加元素
        /// </summary>
        /// <param name="node">新增元素</param>
        public void Add(TreeInfo element)
        {
            // 1、先创建节点
            Node node = new Node(element);
            // 2、判断是否根节点
            if (this.Head == null) 
            { 
                this.Head = node;
            }
            else 
            {
                // 从根节点开始存放
                this.Head.AddNode(node);
            }
        }
    }
}
