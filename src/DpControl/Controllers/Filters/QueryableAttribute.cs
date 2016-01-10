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

namespace DpControl.Controllers.Filters
{
    /// <summary>
    /// 查询过滤器
    /// </summary>
    public class QueryableAttribute : ActionFilterAttribute
    {
        #region 过滤，排序，搜索，分页等查询条件
        /// <summary>
        /// 排序
        /// 格式：orderby=name,price desc/asc
        ///       orderby=name 
        /// </summary>
        private string orderby { get; set; }

        private string[] orderbyParams { get; set; }
        /// <summary>
        /// desc / asc
        /// </summary>
        private string orderbybehavior { get; set; }

        /// <summary>
        /// 跳过前N条
        /// 格式：skip=10
        /// @"^[1-9]([0-9]*)$|^[0-9]$" 只能是0或正整数
        /// </summary>
        private string skip { get; set; }

        /// <summary>
        /// 返回前N条
        /// 格式：top=20
        /// @"^[1-9]([0-9]*)$|^[0-9]$" 只能是0或正整数
        /// </summary>
        private string top { get; set; }

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
        /// 
        /// </summary>
        //private string select { get; set; }

        #endregion

        public  QueryableAttribute()
        {
            this.orderby = null;
            this.orderbybehavior = null;
            this.orderbyParams = null;
            this.skip = null;
            this.top = null;
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

            var orderbyString = context.HttpContext.Request.Query["orderby"].ToString().Trim();
            var skipString = context.HttpContext.Request.Query["skip"].ToString().Trim();
            var topString = context.HttpContext.Request.Query["top"].ToString().Trim();

            #endregion
            #region 校验查询参数是否符合规则
            //校验排序的字段只能是返回数据类含有的字段

            this.skip = GetSkipParam(skipString);
            this.top = GetTopParam(topString);
            this.orderbyParams = GetOrderbyParam(orderbyString,actionReturnType);
            #endregion
            

        }

        /// <summary>
        /// 获取orderby参数
        /// </summary>
        /// <param name="orderbyString"></param>
        /// <param name="actionReturnType"></param>
        /// <returns></returns>
        private string[] GetOrderbyParam(string orderbyString, Type actionReturnType)
        {
            string orderbyParam = orderbyString;
            //校验字符串是否匹配 desc asc后缀
            bool isOrderbyHasBehavior = Regex.IsMatch(orderbyString, @".*( desc| asc)$");
            if (isOrderbyHasBehavior)
            {
                //如果查询后带 desc,或者 asc则把这部分内容截掉
                string regex = @"( desc| asc)";
                Regex re = new Regex(regex);
                orderbyParam = Regex.Replace(orderbyString, regex, "");
                //同时获取orderbybehavior
                Regex reg = new Regex(regex);
                this.orderbybehavior = reg.Match(orderbyString).Groups[1].Value.Trim();
            }
            //以逗号分隔字符串
            string[] arrOrderby = orderbyParam.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] orderbyParams = new string[arrOrderby.Length];
            //校验输入的排序参数的值的范围必须在当前方法返回类型中的字段值中
            for (int i=0;i<arrOrderby.Length;i++)
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
                    orderbyParams[i] = arrOrderby[i].Trim();
                }
            }

            return orderbyParams;

        }

        /// <summary>
        /// 获取skip参数
        /// </summary>
        /// <param name="skipString"></param>
        private string GetSkipParam(string skipString)
        {
            var regex = @"^[1-9]([0-9]*)$|^[0-9]$";
            bool isSkipaAvailable = Regex.IsMatch(skipString, regex);
            string skipStr =  isSkipaAvailable ? skipString : null;
            return skipStr;
        }

        /// <summary>
        /// 获取top参数
        /// </summary>
        /// <param name="topString"></param>
        /// <returns></returns>
        private string GetTopParam(string topString)
        {
            var regex = @"^[1-9]([0-9]*)$|^[0-9]$";
            bool isSkipaAvailable = Regex.IsMatch(topString, regex);
            string topStr = isSkipaAvailable ? topString : null;
            return topStr;
        }

        /// <summary>
        /// 在controller action result执行之前调用 
        /// 获取返回结果后，对结果进行过滤，排序，搜索等操作
        /// </summary>
        /// <param name="context"></param>
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
                        IList resultData = QueryResult(genericList,actionReturnType);
                        //统一查询返回格式
                        var responseData = ResponseHandler.ConstructResponse<IList>(resultData);
                        //对返回结果重新赋值
                        result.Value = responseData;


                    }

                }
            }
            catch(Exception e)
            {
                //出现异常，则不做处理，返回原数据
            }
            
        }

        private IList QueryResult(IList listData, Type actionReturnType)
        {
            IList result = listData;
            if (listData.Count != 0 )
            {
                #region paging by skip and top
                if (!string.IsNullOrEmpty(this.skip) && string.IsNullOrEmpty(this.top))
                {
                    int skipNum = System.Convert.ToInt32(this.skip);
                    result = listData.Cast<object>().Skip(skipNum).ToList();

                } else if (string.IsNullOrEmpty(this.skip) && !string.IsNullOrEmpty(this.top))
                {
                    int topNum = System.Convert.ToInt32(this.top);
                    result = listData.Cast<object>().Take(topNum).ToList();

                } else if (!string.IsNullOrEmpty(this.skip) && !string.IsNullOrEmpty(this.top))
                {
                    int skipNum = System.Convert.ToInt32(this.skip);
                    int topNum = System.Convert.ToInt32(this.top);
                    result = listData.Cast<object>().Skip(skipNum).Take(topNum).ToList();
                }
                #endregion
                #region　orderby
                if (orderbyParams != null)
                {
                    if (!string.IsNullOrEmpty(orderbybehavior) && orderbybehavior.Trim().ToLower() =="desc")
                    {
                        var finalResult = listData.Cast<object>().OrderByDescending(v => v.GetType().GetProperty(orderbyParams[0]).GetValue(v, null));
                        for (int i = 1; i < orderbyParams.Length; i++)
                        {
                            finalResult = finalResult.OrderByDescending(v => v.GetType().GetProperty(orderbyParams[i]).GetValue(v, null));

                        }
                        return finalResult.ToList();
                    }
                    else
                    {
                        var finalResult = listData.Cast<object>().OrderBy(v => v.GetType().GetProperty(orderbyParams[0]).GetValue(v, null));
                        for (int i = 1; i < orderbyParams.Length; i++)
                        {
                            finalResult = finalResult.ThenBy(v => v.GetType().GetProperty(orderbyParams[i]).GetValue(v, null));

                        }
                        return finalResult.ToList();
                    }
                    
                }
                #endregion
            }
            return result;
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
    public class ItemWithProperty
    {
        public string Property { get; set; }
    }
}
