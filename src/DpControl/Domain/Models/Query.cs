using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using DpControl.Domain.Execptions;
using Microsoft.AspNet.Mvc.ViewFeatures;
using System.Linq.Expressions;
using DpControl.Domain.Entities;

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
        /// Filter
        /// </summary>
        public Filter[] filter { get; set; }
        
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
            this.filter = null;
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

    public class Filter
    {
        /// <summary>
        /// key:PropertyName
        /// value:PropertyValue
        /// </summary>
        public Dictionary<string, string> FilterPropertyValue { get; set; }

        public string FilterOperater { get; set; }
    }

    public static class FilterOperators
    {
        public const string Equal = " eq ";
        public const string LessThan = " lt ";
        public const string MoreThan = " gt ";
        public const string And = " and ";
        public const string Or = " or ";

        public static readonly List<string> CompareOperators = new List<string>() {
            Equal,
            LessThan,
            MoreThan
        };
        
        public static readonly List<string> LogicalOperators = new List<string>() {
            And
        };
    }

    public static class QueryOperate<T>
    {
        public static IQueryable<T> Execute(IQueryable<T> queryData,Query query)
        {

            if (query != null)
            {
                int? skip = query.skip;
                int? top = query.top;
                OrderBy orderbyParam = query.orderby;
                Filter[] filterParams = query.filter;

                //Filter operate
                queryData = Filter(queryData,filterParams);
                //paging operate
                queryData = Paging(queryData, skip, top);
                //orderby operate
                queryData = OrderBy(queryData,orderbyParam);
            }
            return queryData;
        }
        #region Filter
        private static IQueryable<T> Filter(IQueryable<T> query, Filter[] filterParams)
        {
            for (int i = 0; i < filterParams.Length; i++)
            {
                string propertyName = filterParams[i].FilterPropertyValue.First().Key;
                string propertyValue = filterParams[i].FilterPropertyValue.First().Value;
                string filterOperator = filterParams[i].FilterOperater;
                query = ConstructWhereCondition(query, propertyName, propertyValue, filterOperator);

            }
            return query;
        }

        private static IQueryable<T> ConstructWhereCondition(IQueryable<T> query,string propertyName,object value,string filterOperator)
        {
            var valueTypr = typeof(T).GetProperty(propertyName).PropertyType;
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            //Conver value to propertyType
            var converValue = Convert.ChangeType(value,valueTypr);
            Expression right = Expression.Constant(converValue);
            Expression filter = Expression.Equal(left, right);
            if (filterOperator.Equals(FilterOperators.LessThan))
            {
                filter = Expression.LessThanOrEqual(left, right);
            }
            else if (filterOperator.Equals(FilterOperators.MoreThan))
            {
                filter = Expression.GreaterThanOrEqual(left, right);

            }
            else if (filterOperator.Equals(FilterOperators.Equal))
            {
                filter = Expression.Equal(left, right);
            }
            
            return query = query.Where(Expression.Lambda<Func<T, bool>>(filter, param));
        }

        private static IQueryable<T> ConstructWhereLessThanCondition(IQueryable<T> query, string propertyName, object value)
        {

            var property = typeof(T).GetProperty(propertyName);
            string typeName = property.PropertyType.Name;
           
            if (typeName == "Int16")
            {
                query = query.Where(v => System.Convert.ToInt16(v.GetType().GetProperty(propertyName).GetValue(v, null)) <= System.Convert.ToInt16(value));

            }
            else if (typeName == "Int32")
            {
                query = query.Where(v => System.Convert.ToInt32(v.GetType().GetProperty(propertyName).GetValue(v, null)) <= System.Convert.ToInt32(value));
            }
            else if (typeName == "Int64")
            {
                query = query.Where(v => System.Convert.ToInt64(v.GetType().GetProperty(propertyName).GetValue(v, null)) <= System.Convert.ToInt64(value));
            }
            else if (typeName == "DateTime")
            {
                query = query.Where(v => System.Convert.ToDateTime(v.GetType().GetProperty(propertyName).GetValue(v, null)) <= System.Convert.ToDateTime(value));
            }
            return query;
        }
        private static IQueryable<T> ConstructWhereMoreThanCondition(IQueryable<T> query, string propertyName, object value)
        {

            var property = typeof(T).GetProperty(propertyName);
            string typeName = property.PropertyType.Name;

            if (typeName == "Int16")
            {
                query = query.Where(v => System.Convert.ToInt16(v.GetType().GetProperty(propertyName).GetValue(v, null)) >= System.Convert.ToInt16(value));

            }
            else if (typeName == "Int32")
            {
                query = query.Where(v => System.Convert.ToInt32(v.GetType().GetProperty(propertyName).GetValue(v, null)) >= System.Convert.ToInt32(value));
            }
            else if (typeName == "Int64")
            {
                query = query.Where(v => System.Convert.ToInt64(v.GetType().GetProperty(propertyName).GetValue(v, null)) >= System.Convert.ToInt64(value));
            }
            else if (typeName == "DateTime")
            {
                query = query.Where(v => System.Convert.ToDateTime(v.GetType().GetProperty(propertyName).GetValue(v, null)) >= System.Convert.ToDateTime(value));
            }
            return query;
        }
        private static IQueryable<T> ConstructWhereEqualCondition(IQueryable<T> query,string propertyName,object value)
        {

            var property = typeof(T).GetProperty(propertyName);
            string typeName = property.PropertyType.Name;
            if (typeName == "String")
            {
                ParameterExpression param = Expression.Parameter(typeof(T), "c");
                Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
                Expression right = Expression.Constant(value);
                Expression filter = Expression.Equal(left, right);
                query = query.Where(Expression.Lambda<Func<T, bool>>(filter, param));

                query = query.Where(v => System.Convert.ToString(v.GetType().GetProperty(propertyName).GetValue(v, null)) == value.ToString());

            }
            else if (typeName == "Int16")
            {
                query = query.Where(v => System.Convert.ToInt16(v.GetType().GetProperty(propertyName).GetValue(v, null)) == System.Convert.ToInt16(value));
                
            }
            else if (typeName == "Int32")
            {
                query = query.Where(v => System.Convert.ToInt32(v.GetType().GetProperty(propertyName).GetValue(v, null)) == System.Convert.ToInt32(value));
            }
            else if (typeName == "Int64")
            {
                query = query.Where(v => System.Convert.ToInt64(v.GetType().GetProperty(propertyName).GetValue(v, null)) == System.Convert.ToInt64(value));
            }
            else if (typeName == "Boolean")
            {
                query = query.Where(v => System.Convert.ToBoolean(v.GetType().GetProperty(propertyName).GetValue(v, null)) == System.Convert.ToBoolean(value));
            }
            else if (typeName == "DateTime")
            {
                query = query.Where(v => System.Convert.ToDateTime(v.GetType().GetProperty(propertyName).GetValue(v, null)) == System.Convert.ToDateTime(value));
            }
            return query;
        }
        #endregion
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
            if (orderbyParam != null)
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
                        return result;
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
                        return result;
                    }
                }
            }
            return query;
        }

        #endregion
    }
}
