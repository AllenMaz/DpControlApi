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
    [TypeConverter(typeof(QueryConverter))]
    [ModelBinder(BinderType = typeof(QueryModelBinder))]
    public class Query
    {
        /// <summary>
        /// 
        /// </summary>
        public string expand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string filter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string inlinecount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string orderby { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string select { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string skip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string top { get; set; }

        /// <summary>
        /// Conver s value to result
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out Query result)
        {
            result = null;

            var parts = s.Split(',');
            //if (parts.Length != 2)
            //{
            //    return false;
            //}

            string expand, filter;

            expand = parts[0];
            filter = parts[1];
            result = new Query()
            {
                expand = expand,
                filter = filter
            };
            return true;

        }
    }
    
    
}
