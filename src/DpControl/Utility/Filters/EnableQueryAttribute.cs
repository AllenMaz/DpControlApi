using DpControl.Domain.Models;
using DpControl.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Mvc.Controllers;
using DpControl.Utility;
using System.Reflection;
using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;

namespace DpControl.Utility.Filters
{
    /// <summary>
    /// 查询过滤器
    /// 该类允许对Controller取得的结果集进行过滤，排序等查询操作
    /// </summary>
    public class EnableQueryAttribute : ActionFilterAttribute
    {
        //查询参数对象
        private Query query ;

        public EnableQueryAttribute()
        {
            query = new Query();
        }

        /// <summary>
        /// 在controller action执行之后调用 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        /// <summary>
        /// 在controller action执行之前调用 
        /// 在方法执行前获取过滤，排序，搜索等查询条件
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Type actionReturnType = GetActionReturnType<ActionExecutingContext>(context);
            #region 获取查询参数
            var skipString = context.HttpContext.Request.Query["skip"].ToString().Trim();
            var topString = context.HttpContext.Request.Query["top"].ToString().Trim();
            var orderbyString = context.HttpContext.Request.Query["orderby"].ToString().Trim();
            var selectString = context.HttpContext.Request.Query["select"].ToString().Trim();
            
            query.skip = GetSkipParam(skipString);
            query.top = GetTopParam(topString);
            query.orderby = GetOrderbyParam(orderbyString,actionReturnType);
            query.select = GetSelectParam(selectString,actionReturnType);
            #endregion

            //给Action的查询参数赋值
            string queryKey = GetActionQueryArgumentKey(context.ActionArguments);
            context.ActionArguments[queryKey] = query;
        }

        private string GetActionQueryArgumentKey(IDictionary<string,object> actionArguments)
        {
            string queryKey = string.Empty;
            foreach (string key in actionArguments.Keys)
            {
                if (actionArguments[key].GetType() == typeof(Query))
                {
                    queryKey = key;
                    break;
                }
            }
            return queryKey;
        }

        /// <summary>
        /// 获取skip参数
        /// </summary>
        /// <param name="skipString"></param>
        private int? GetSkipParam(string skipString)
        {
            int? skipParma = null;
            var regex = @"^[1-9]([0-9]*)$|^[0-9]$";
            bool isSkipaAvailable = Regex.IsMatch(skipString, regex);
            if (isSkipaAvailable)
            {
                skipParma = System.Convert.ToInt32(skipString);
            }
            return skipParma;
        }

        /// <summary>
        /// 获取top参数
        /// </summary>
        /// <param name="topString"></param>
        /// <returns></returns>
        private int? GetTopParam(string topString)
        {
            int? topParam = null;
            var regex = @"^[1-9]([0-9]*)$|^[0-9]$";
            bool isSkipaAvailable = Regex.IsMatch(topString, regex);
            if (isSkipaAvailable)
            {
                topParam = System.Convert.ToInt32(topString);
            }
            return topParam;
        }

        /// <summary>
        /// 获取orderby参数
        /// </summary>
        /// <param name="orderbyString"></param>
        /// <param name="actionReturnType"></param>
        /// <returns></returns>
        private OrderBy GetOrderbyParam(string orderbyString, Type actionReturnType)
        {
            OrderBy orderbyParam = new OrderBy();
            string orderbyParamString = orderbyString;
            //校验字符串是否匹配 desc asc后缀
            bool isOrderbyHasBehavior = Regex.IsMatch(orderbyString, @".*( desc| asc)$");
            if (isOrderbyHasBehavior)
            {
                //如果查询后带 desc,或者 asc则把这部分内容截掉
                string regex = @"( desc| asc)";
                Regex re = new Regex(regex);
                orderbyParamString = Regex.Replace(orderbyString, regex, "");
                //同时获取orderbybehavior
                Regex reg = new Regex(regex);
                orderbyParam.OrderbyBehavior = reg.Match(orderbyString).Groups[1].Value.Trim();
            }
            //以逗号分隔字符串
            string[] arrOrderby = orderbyParamString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] OrderbyField = new string[arrOrderby.Length];
            //校验输入的排序参数的值的范围必须在当前方法返回类型中的字段值中
            for (int i = 0; i < arrOrderby.Length; i++)
            {
                var property = actionReturnType.GetProperty(arrOrderby[i]);
                if (property == null)
                {
                    //如果有任意一个值不属于方法返回值类型，则返回null
                    return null;
                }
                else
                {
                    //去除每个排序参数的前后空格
                    OrderbyField[i] = arrOrderby[i].Trim();
                }
            }

            orderbyParam.OrderbyField = OrderbyField;
            return orderbyParam;

        }

        /// <summary>
        /// 获取select参数
        /// </summary>
        /// <param name="orderbyString"></param>
        /// <param name="actionReturnType"></param>
        /// <returns></returns>
        private string[] GetSelectParam(string selectString, Type actionReturnType)
        {
            //以逗号分隔字符串
            string[] arrSelect = selectString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] selectField = new string[arrSelect.Length];
            //校验输入的select参数的值的范围必须在当前方法返回类型中的字段值中
            for (int i = 0; i < arrSelect.Length; i++)
            {
                var property = actionReturnType.GetProperty(arrSelect[i]);
                if (property == null)
                {
                    //如果有任意一个值不属于方法返回值类型，则返回null
                    return null;
                }
                else
                {
                    //去除每个select参数的前后空格
                    selectField[i] = arrSelect[i].Trim();
                }
            }
            
            return selectField;

        }

        /// <summary>
        /// 在controller action result执行之前调用 
        /// 获取返回结果后，对结果进行过滤，排序，搜索等操作
        /// </summary>
        /// <param name="context"></param>
        /// 
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            try
            {
                //获取方法的返回值类型
                Type actionReturnType = GetActionReturnType<ResultExecutingContext>(context);

                if (context.Result is ObjectResult)
                {
                    //获取返回的结果
                    var result = context.Result as ObjectResult;

                    //如果返回结果类型为list，则进行如下操作
                    if (result.Value.GetType().Name == "List`1")
                    {
                        //根据方法返回值类型，生成List<返回值类型>实例
                        IList genericList = CreateList(typeof(List<>), actionReturnType, result.Value);
                        //对实例集合进行查询操作
                        IList resultData = QueryResult(genericList, actionReturnType);
                        //对返回结果重新赋值
                        result.Value = resultData;

                    }

                }
            }
            catch (Exception e)
            {
                //出现异常，则不做处理，返回原数据
            }

        }

        private IList QueryResult(IList listData, Type actionReturnType)
        {
            IList result = listData;
            if (listData.Count != 0 )
            {
                int? skipParam = query.skip;
                int? topParam = query.top;
                OrderBy orderbyParam = query.orderby;

                result = OrderByResult(listData,orderbyParam);
                result = PageResult(result,skipParam,topParam);
                
            }
            return result;
        }

        /// <summary>
        /// 对查询结果进行排序
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        private IList OrderByResult(IList listData, OrderBy orderbyParam)
        {
            #region　orderby
            if (orderbyParam.OrderbyField.Length != 0)
            {
                string orderbyBehavior = orderbyParam.OrderbyBehavior;
                string[] orderbyField = orderbyParam.OrderbyField;
                if (!string.IsNullOrEmpty(orderbyBehavior) && orderbyBehavior.Trim().ToLower() == "desc")
                {
                    var finalResult = listData.Cast<object>().OrderByDescending(v => v.GetType().GetProperty(orderbyField[0]).GetValue(v, null));
                    for (int i = 1; i < orderbyField.Length; i++)
                    {
                        finalResult = finalResult.ThenByDescending(v => v.GetType().GetProperty(orderbyField[i]).GetValue(v, null));
                        if (i == orderbyField.Length - 1)
                        {
                            return finalResult.ToList();
                        }
                    }
                }
                else
                {
                    var finalResult = listData.Cast<object>().OrderBy(v => v.GetType().GetProperty(orderbyField[0]).GetValue(v, null));
                    for (int i = 1; i < orderbyField.Length; i++)
                    {
                        finalResult = finalResult.ThenBy(v => v.GetType().GetProperty(orderbyField[i]).GetValue(v, null));
                        if (i == orderbyField.Length - 1)
                        {
                            return finalResult.ToList();
                        }
                    }
                }

            }
            return listData;
            #endregion
        }

        /// <summary>
        /// 对结果进行分页
        /// </summary>
        /// <returns></returns>
        private IList PageResult(IList listData, int? skipParam, int? topParam)
        {
            var result = listData;
            #region paging by skip and top
            if (skipParam != null && query.top == null)
            {
                int skipNum = System.Convert.ToInt32(skipParam);
                result = listData.Cast<object>().Skip(skipNum).ToList();

            }
            else if (skipParam == null && query.top != null)
            {
                int topNum = System.Convert.ToInt32(topParam);
                result = listData.Cast<object>().Take(topNum).ToList();

            }
            else if (skipParam != null && query.top != null)
            {
                int skipNum = System.Convert.ToInt32(skipParam);
                int topNum = System.Convert.ToInt32(topParam);
                result = listData.Cast<object>().Skip(skipNum).Take(topNum).ToList();
            }
            return result;
            #endregion
        }

        /// <summary>
        /// 根据类型，生成实例
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="innerType"></param>
        /// <param name="args">实例值</param>
        /// <returns></returns>
        private IList CreateList(Type genericType, Type innerType, params object[] args)
        {
            IList lsResult = (IList)Activator.CreateInstance(genericType.MakeGenericType(innerType), args);
            return lsResult;
        }

        /// <summary>
        /// 获取方法的返回值类型
        /// 如：public async Task<IEnumerable<MCustomer>> GetAll()，返回值类型为MCustomer
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Type GetActionReturnType<T>(T context)
        {
            //利用反射获取泛型T的属性ActionDescriptor的值
            Type t = typeof(T);
            var controllerActionDescriptor = t.GetProperty("ActionDescriptor").GetValue(context, null)
                as ControllerActionDescriptor;
            //var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var actionReturnType = controllerActionDescriptor.MethodInfo.ReturnType;
                var actionClassType = GetClassType(actionReturnType);
                return actionClassType;

            }
            return null;
        }


        /// <summary>
        /// 获取Class类型
        /// </summary>
        /// <param name="converType"></param>
        /// <returns></returns>
        private Type GetClassType(Type converType)
        {
            Type returnType = converType ;
            //此对象是否表示构造的泛型类型的值
            var isConstructedGenericType = converType.IsConstructedGenericType;
            if (!isConstructedGenericType)
            {
                return converType;
            }
            var nextType = converType.GenericTypeArguments[0];
            return GetClassType(nextType);
        }
    }
}
