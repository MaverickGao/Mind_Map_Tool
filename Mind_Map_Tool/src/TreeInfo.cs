using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// 树对象，也是思维导图构成的结点
namespace Mind_Map_Tool.src
{
    public enum rowType
    {
        基础指标 = 0,
        计算指标 = 1,
        wind表 = 2,
        研究部表 = 3,
        excel表 = 4,
        windApi表 = 5,

    }

    internal class TreeInfo
    {
        // 指标/表编码
        public string labelCode { get; set; }

        // 指标/表名称
        public string? labelName { get; set; }

        // 父结点
        public string? fatherCode { get; set; }

        // 指标类型
        public int? labelType { get; set; }

        // 存储的库、表
        public string? saveDbOrTable { get; set; }

        // 来源表字段
        public string? tableColumn { get; set; }

        // 备注
        public string? remark { get; set; }

        // 处理人
        public string? dealUser { get; set; }

        /// 根据值获取枚举
        public string GetEnumNameByKey(int key)
        {
            return Enum.GetName(typeof(rowType), key);
        }
    }
}
