using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Vanara.PInvoke;
using static Mind_Map_Tool.src.Tree;

namespace Mind_Map_Tool.src
{
    internal class WebController
    {
        /// <summary>
        ///     根据 标签Code 或 标签Name 匹配标签列表
        /// </summary>
        /// <param name="keyWord">标签Code 或 标签Name</param>
        /// <returns></returns>
        public static string GetTreeList(string keyWord)
        {
            // 得到所有解析的指标列表
            Dictionary<string, string> items = BuildTreeInfo.listData;
            List<Result> results = new List<Result>();
            // 将结果转为list
            foreach (var item in items)
            {
                string labelCode = item.Key;
                string labelName = item.Value;
                if (!string.IsNullOrEmpty(keyWord))
                {
                    if (labelCode.Contains(keyWord) || labelName.Contains(keyWord))
                    {
                        Result result = new Result();
                        result.labelCode = labelCode;
                        result.labelName = labelName;
                        results.Add(result);
                    }
                }
                else
                {
                    Result result = new Result();
                    result.labelCode = labelCode;
                    result.labelName = labelName;
                    results.Add(result);
                }
            }
            return JsonConvert.SerializeObject(results);
        }

        public static string GetTreeMap(string labelCode)
        {
            // 得到所有解析的指标列表
            Dictionary<string, Tree> items = BuildTreeInfo.dataTree;
            // 根据labelCode获取Tree
            Tree result = items[labelCode];
            return JsonConvert.SerializeObject(buildTreeResult(result.Head));
        }

        private static TreeResult buildTreeResult(Tree.Node node)
        {
            TreeResult result = new TreeResult();
            TreeInfo treeInfo = node.Item;
            result.id = treeInfo.labelCode;
            result.topic = treeInfo.labelCode + " " + treeInfo.labelName;
            result.direction = "left";
            result.expanded = true;
            List<Tree.Node> lastNodes = node.LastNodes;
            if (null != lastNodes)
            {
                List<TreeResult> childs = new List<TreeResult>();
                foreach (Tree.Node child in lastNodes)
                {
                    TreeResult children = buildTreeResult(child);
                    childs.Add(children);
                }

                result.children = childs;
            }
            return result;
        }
    }

    public class TreeResult
    {
        public string id { get; set; }
        public string topic { get; set; }
        public string? direction { get; set; }
        public Boolean? expanded { get; set; }
        public List<TreeResult>? children { get; set; }
    }

    public class Result
    { 
        public string labelCode { get; set; }
        public string labelName { get; set; }
    }
}
