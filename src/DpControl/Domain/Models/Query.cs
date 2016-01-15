using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class Query
    {      /// <summary>
           /// orderby 
           /// </summary>
        public OrderBy orderby { get; set; }


        /// <summary>
        /// 跳过前N条
        /// </summary>
        public int? skip { get; set; }

        /// <summary>
        /// 返回前N条
        /// </summary>
        public int? top { get; set; }

        /// <summary>
        /// 选中要返回的属性结果
        /// </summary>
        public string[] select { get; set; }
    }

    public class OrderBy
    {
        /// <summary>
        /// orderby field name
        /// </summary>
        public string[] OrderbyField { get; set; }

        /// <summary>
        /// orderby behavior (desc/asc or empty)
        /// </summary>
        public string OrderbyBehavior { get; set; }
    }
}
