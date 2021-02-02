using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LES.Core.Model.ViewModels
{
    /// <summary>
    /// Select控件显示的组合
    /// </summary>
    public class SelectViewModel
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int value { get; set; }
    }
}
