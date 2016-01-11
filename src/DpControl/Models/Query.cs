using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc;

namespace DpControl.Models
{
    public class Query
    {
        /// <summary>
        /// 排序
        /// 格式：orderby=name,price desc/asc
        ///       orderby=name 
        /// </summary>
        public OrderBy orderby { get; set; }


        /// <summary>
        /// 跳过前N条
        /// 格式：skip=10
        /// @"^[1-9]([0-9]*)$|^[0-9]$" 只能是0或正整数
        /// </summary>
        public int? skip { get; set; }

        /// <summary>
        /// 返回前N条
        /// 格式：top=20
        /// @"^[1-9]([0-9]*)$|^[0-9]$" 只能是0或正整数
        /// </summary>
        public int? top { get; set; }

        /// <summary>
        /// 选中要返回的属性结果
        /// 格式：select = name,price
        /// </summary>
        public string[] select { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //private string expand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //private string filter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //private string inlinecount { get; set; }


        /// <summary>
        /// Conver s value to result
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        //public static bool ConverQuerystringToObject(string queryString, out Query result)
        //{
        //}

        public  Query()
        {
            this.orderby = null;
            this.skip = null;
            this.top = null;
            this.select = null;
        }

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
