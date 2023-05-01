using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;
using static Mind_Map_Tool.src.Tree;

namespace Mind_Map_Tool.src
{
    internal class BuildTreeInfo
    {
        // 全局变量树结构，所有的功能的数据来源
        public static Dictionary<string, Tree> dataTree = new Dictionary<string, Tree>();

        // 搜索框数据
        public static Dictionary<string, string> listData = new Dictionary<string, string>();

        /// <summary>
        ///     构建树结构对象 
        /// </summary>
        /// <param name="filePath"></param>
        public static void buildTreeInfo(string filePath) {
            // 解析Excel文件
            DataTable dt = ReadExcel.excelToTable(@"E:\c#space\Mind_Map_Tool\Mind_Map_Tool\Resources\基构云数据指标概览.xlsx");

            // 将Excel中的数据解析为树结构
            transTreeInfo(dt);
        }

        /// <summary>
        ///     将Excel中的数据解析为树结构
        /// </summary>
        /// <param name="dt"></param>
        private static void transTreeInfo(DataTable dt) 
        {
            if (dt == null) 
            {
                return;
            }
            List<string> strColumns = GetColumnsByDataTable(dt);

            Dictionary<string, TreeInfo> rowMap = new Dictionary<string, TreeInfo>();

            foreach (DataRow item in dt.Rows)
            {
                if (null == item[0] || item[0].ToString() == string.Empty || null == item[1] || item[1].ToString() == string.Empty)
                {
                    continue;
                }
                TreeInfo treeNode = new TreeInfo();
                string labelCode = item[0].ToString();
                treeNode.labelCode = labelCode;
                if (null != item[1] && item[1].ToString() != string.Empty)
                {
                    treeNode.labelName = item[1].ToString();
                }
                if (null != item[2] && item[2].ToString() != string.Empty)
                {
                    treeNode.fatherCode = item[2].ToString();
                }
                if (null != item[3] && item[3].ToString() != string.Empty)
                {
                    treeNode.labelType = int.Parse(item[3].ToString());
                }
                if (null != item[4] && item[4].ToString() != string.Empty)
                {
                    treeNode.saveDbOrTable = item[4].ToString();
                }
                if (null != item[5] && item[5].ToString() != string.Empty)
                {
                    treeNode.tableColumn = item[5].ToString();
                }
                if (null != item[6] && item[6].ToString() != string.Empty)
                {
                    treeNode.remark = item[6].ToString();
                }
                if (null != item[7] && item[7].ToString() != string.Empty)
                {
                    treeNode.dealUser = item[7].ToString();
                }
                rowMap.Add(labelCode, treeNode);
                if (null != treeNode.labelType && (treeNode.labelType == 0 || treeNode.labelType == 1))
                {
                    listData.Add(treeNode.labelCode, treeNode.labelName);
                }
            }
            buildDO(rowMap);
        }

        private static void buildDO(Dictionary<string, TreeInfo> rowMap)
        { 
            foreach (string labelCode in listData.Keys) 
            {
                // 当前结点 相关结点
                Tree tree = CardTree(labelCode, rowMap, null);
                // 将这些结点组装为树
                if (null != tree)
                {
                    dataTree.Add(labelCode, tree);
                }
            }
        }

        /// <summary>
        /// 根据datatable获得列名
        /// </summary>
        /// <param name="dt">表对象</param>
        /// <returns>返回结果的数据列数组</returns>
        private static List<string> GetColumnsByDataTable(DataTable dt)
        {
            List<string> strColumns = new List<string>();

            if (dt.Columns.Count > 0)
            {
                int columnNum = 0;
                columnNum = dt.Columns.Count; ;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strColumns.Add(dt.Columns[i].ColumnName);
                }
            }
            return strColumns;
        }

        /// <summary>
        ///     梳理跟当前结点相关的结点
        /// </summary>
        /// <param name="code">当前结点</param>
        /// <param name="rowMap">所有结点</param>
        /// <returns>相关结点列表</returns>
        private static Tree CardTree(string code, Dictionary<string, TreeInfo> rowMap, Tree result)
        {
            if (null == result)
            {
                result = new Tree();
            }
            // 当前结点
            TreeInfo item = rowMap[code];
            // 先将结点加入树
            result.Add(item);
            // 向左找，得到当前节点的父节点列表
            // 先向左遍历
            List<TreeInfo> LeftResult = BuildLeftTreeInfo(code, rowMap);
            if (null != LeftResult && LeftResult.Count > 0) 
            {
                // 循环遍历
                foreach (TreeInfo node in LeftResult)
                {
                    CardTree(node.labelCode, rowMap, result);
                }
            }
            return result;
        }

        /// <summary>
        ///     得到当前结点的所有父节点
        /// </summary>
        /// <param name="code"></param>
        /// <param name="rowMap"></param>
        /// <returns></returns>
        private static List<TreeInfo> BuildLeftTreeInfo(string code, Dictionary<string, TreeInfo> rowMap)
        {
            List<TreeInfo> LeftResult = new List<TreeInfo>();
            // 这个是当前结点信息
            TreeInfo item = rowMap[code];
            // 得到所有的父节点
            string fatherCodeList = item.fatherCode;
            if (fatherCodeList != null)
            {
                List<string> fatherCode = fatherCodeList.Split(',').ToList();
                foreach (string father in fatherCode)
                {
                    TreeInfo fatherItem = rowMap[father];
                    // 判断是否已经在结点中，如果不在，先添加进去
                    if (null != fatherItem)
                    {
                        LeftResult.Add(fatherItem);
                    }
                }
            }
            return LeftResult;
        }

        /// <summary>
        ///     得到当前结点的所有子节点
        /// </summary>
        /// <param name="code"></param>
        /// <param name="rowMap"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        //private static List<TreeInfo> BuildRightTreeInfo(string code, Dictionary<string, TreeInfo> rowMap, List<TreeInfo> result)
        //{
        //    List<TreeInfo> RightResult = new List<TreeInfo>();
        //    // 这个是当前结点信息
        //    TreeInfo item = rowMap[code];
        //    // 判断是否已经在结点中，如果不在，先添加进去
        //    if (null != item && !result.Contains(item))
        //    {
        //        RightResult.Add(item);
        //    }
        //    foreach (TreeInfo node in rowMap.Values)
        //    {
        //        string fatherCodeList = node.fatherCode;
        //        if (fatherCodeList != null)
        //        {
        //            List<string> fatherCode = fatherCodeList.Split(',').ToList();
        //            if (!result.Contains(node) && fatherCode.Contains(node.labelCode) )
        //            {
        //                RightResult.Add(node);
        //            }
        //        }
        //    }
        //    return RightResult;
        //}
    }
}
