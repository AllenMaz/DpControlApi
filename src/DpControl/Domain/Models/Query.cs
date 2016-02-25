using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace DpControl.Domain.Models
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

    public static class QueryOperate<T>
    {
        public static IQueryable<T> Execute(IQueryable<T> queryData,Query query)
        {
            int? skip = query.skip;
            int? top = query.top;
            OrderBy orderbyParam = query.orderby;

            //paging operate
            var result = Paging(queryData,skip,top);
            //orderby operate
            result = OrderBy(result, orderbyParam);

            return result;
        }

        #region Paging
        private static IQueryable<T> Paging(IQueryable<T> query, int? skip, int? top)
        {
            #region paging by skip and top
            if (skip != null && top == null)
            {
                int skipNum = System.Convert.ToInt32(skip);
                query = query.Skip(skipNum);

            }
            else if (skip == null && top != null)
            {
                int topNum = System.Convert.ToInt32(top);
                query = query.Take(topNum);

            }
            else if (skip != null && top != null)
            {
                int skipNum = System.Convert.ToInt32(skip);
                int topNum = System.Convert.ToInt32(top);
                query = query.Skip(skipNum).Take(topNum);
            }
            return query;
            #endregion
        }

        #endregion
        #region Order By
        private static IQueryable<T> OrderBy(IQueryable<T> query,OrderBy orderbyParam)
        {
            if (orderbyParam.OrderbyField.Length > 0)
            {
                string orderbyBehavior = orderbyParam.OrderbyBehavior;
                string[] orderbyField = orderbyParam.OrderbyField;

                if (!string.IsNullOrEmpty(orderbyBehavior) && orderbyBehavior.Trim().ToLower() == "desc")
                {
                    var result = query.OrderByDescending(v => v.GetType().GetProperty(orderbyField[0]).GetValue(v, null));
                    for (int i = 1; i < orderbyField.Length; i++)
                    {
                        result = result.ThenByDescending(v => v.GetType().GetProperty(orderbyField[i]).GetValue(v, null));
                        if (i == orderbyField.Length - 1)
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    var result = query.OrderBy(v => v.GetType().GetProperty(orderbyField[0]).GetValue(v, null));
                    for (int i = 1; i < orderbyField.Length; i++)
                    {
                        result = result.ThenBy(v => v.GetType().GetProperty(orderbyField[i]).GetValue(v, null));
                        if (i == orderbyField.Length - 1)
                        {
                            return result;
                        }
                    }
                }
            }

            return query;
        }

        #endregion
    }
}
