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
    //[TypeConverter(typeof(QueryConverter))]
    [ModelBinder(BinderType = typeof(QueryModelBinder))]
    public class Query
    {
        /// <summary>
        /// 排序
        /// </summary>
        public string orderby { get; set; }

        /// <summary>
        /// 跳过前N条
        /// </summary>
        public string skip { get; set; }

        /// <summary>
        /// 返回前N条
        /// </summary>
        public string top { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public string expand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public string filter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public string inlinecount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public string select { get; set; }

        
        /// <summary>
        /// Conver s value to result
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ConverQuerystringToObject(string queryString, out Query result)
        {
            result = null;
            if (!string.IsNullOrEmpty(queryString))
            {

            }
            
            return true;

        }
    }
    
    
}
