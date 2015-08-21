using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enums
{
    /// <summary>
    /// 定义水果列表
    /// </summary>
    public enum Fruits:byte
    {
        /// <summary>
        /// 未设置
        /// </summary>
        [Description("未设置")]
        None=0,
        /// <summary>
        /// 橙子
        /// </summary>
       [Description("橙子")]
        Orange=1,
        /// <summary>
        /// 香蕉
        /// </summary>
        [Description("香蕉")]
        Banana=2,
        /// <summary>
        /// 葡萄
        /// </summary>
        [Description("葡萄")]
        Grape=3,
        /// <summary>
        /// 菠萝
        /// </summary>
        [Description("菠萝")]
        Pineapple=4,
        /// <summary>
        /// 草莓
        /// </summary>
        [Description("草莓")]
        Strawberry=5,
        /// <summary>
        /// 西瓜
        /// </summary>
        [Description("西瓜")]
        Watermelon=6
    }

    /// <summary>
    /// 角色列表
    /// </summary>
    public enum Roles : byte
    {
        /// <summary>
        /// 管理员
        /// </summary>
        Administrator=1,
        /// <summary>
        /// 中介
        /// </summary>
        Agent=2,
        /// <summary>
        /// 普通用户
        /// </summary>
        Normal=3
    }
}
