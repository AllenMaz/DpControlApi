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
using DpControl.Domain.Models;
using DpControl.Domain.Execptions;

namespace DpControl.Utility.Filters
{
    /// <summary>
    /// 查询过滤器
    /// 该类允许对Controller取得的结果集进行过滤，排序等查询操作
    /// </summary>
    public class EnableQueryAttribute : ActionFilterAttribute
    {
        private Type _actionReturnType { get; set; }

        public EnableQueryAttribute()
        {
            Query.EmptyQuery();
        }

        public EnableQueryAttribute(Type actionReturnType)
        {
            _actionReturnType = actionReturnType;
            Query.EmptyQuery();

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
            if(_actionReturnType == null)
                _actionReturnType = GetActionReturnType<ActionExecutingContext>(context);
           
            #region 获取查询参数
            var skipString = context.HttpContext.Request.Query["skip"].ToString().Trim();
            var topString = context.HttpContext.Request.Query["top"].ToString().Trim();
            var orderbyString = context.HttpContext.Request.Query["orderby"].ToString().Trim();
            var filterString = context.HttpContext.Request.Query["filter"].ToString().Trim();
            var selectString = context.HttpContext.Request.Query["select"].ToString().Trim();
            var expandString = context.HttpContext.Request.Query["expand"].ToString().Trim();

            Query.skip = GetSkipParam(skipString);
            Query.top = GetTopParam(topString);
            Query.orderby = GetOrderbyParam(orderbyString);
            Query.filter = GetFilterParam(filterString);
            Query.select = GetSelectParam(selectString);
            Query.expand = GetExpandParam(expandString);
            
            #endregion

            //给Action的查询参数赋值
            //string queryKey = GetActionQueryArgumentKey(context.ActionArguments);
            //context.ActionArguments[queryKey] = query;
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
            if (!string.IsNullOrEmpty(skipString))
            {
                var regex = @"^[1-9]([0-9]*)$|^[0-9]$";
                bool isSkipaAvailable = Regex.IsMatch(skipString, regex);
                if (!isSkipaAvailable)
                    throw new ExpectException("The query(paging) syntax errors: skip param format error");
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
            if (!string.IsNullOrEmpty(topString))
            {
                var regex = @"^[1-9]([0-9]*)$|^[0-9]$";
                bool isSkipaAvailable = Regex.IsMatch(topString, regex);
                if (!isSkipaAvailable)
                    throw new ExpectException("The query(paging) syntax errors: top param format error");
                topParam = System.Convert.ToInt32(topString);

            }

            return topParam;
        }

        /// <summary>
        /// 获取orderby参数
        /// </summary>
        /// <param name="orderbyString"></param>
        /// <returns></returns>
        private OrderBy GetOrderbyParam(string orderbyString)
        {
            if (string.IsNullOrEmpty(orderbyString)) return null;

            OrderBy orderbyParam = new OrderBy();
            string orderbyParamString = orderbyString;
            //校验字符串是否匹配 desc asc后缀
            bool isOrderbyHasBehavior = Regex.IsMatch(orderbyString, @".*( desc| asc)$", RegexOptions.IgnoreCase);
            if (isOrderbyHasBehavior)
            {
                //如果查询后带 desc,或者 asc则把这部分内容截掉
                string regex = @"( desc| asc)$";
                orderbyParamString = Regex.Replace(orderbyString, regex, "", RegexOptions.IgnoreCase);
                //同时获取orderbybehavior
                Regex reg = new Regex(regex,RegexOptions.IgnoreCase);
                orderbyParam.OrderbyBehavior = reg.Match(orderbyString).Groups[1].Value.Trim().ToLower();
            }
            //以逗号分隔字符串
            string[] arrOrderby = orderbyParamString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] OrderbyField = new string[arrOrderby.Length];
            //校验输入的排序参数的值的范围必须在当前方法返回类型中的字段值中
            for (int i = 0; i < arrOrderby.Length; i++)
            {
                var property = _actionReturnType.GetProperty(arrOrderby[i]);
                if (property == null)
                    //如果有任意一个值不属于方法返回值类型，则返回错误
                    throw new ExpectException("The query(orderby) syntax errors:Property '"+arrOrderby[i]+"' not exist");
                //判断属性类型是否可用于orderby
                if (!IsFilter_Select_OrderbyType(property))
                    throw new ExpectException("The query(orderby) type errors:Property '" + arrOrderby[i] + "' can't be orderby");


                //去除每个排序参数的前后空格
                OrderbyField[i] = arrOrderby[i].Trim();
            }

            orderbyParam.OrderbyField = OrderbyField;
            return orderbyParam;

        }

        /// <summary>
        /// 获取select参数
        /// </summary>
        /// <param name="orderbyString"></param>
        /// <returns></returns>
        private string[] GetSelectParam(string selectString)
        {
            if (string.IsNullOrEmpty(selectString)) return null;

            //以逗号分隔字符串
            string[] arrSelect = selectString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] selectField = new string[arrSelect.Length];
            //校验输入的select参数的值的范围必须在当前方法返回类型中的字段值中
            for (int i = 0; i < arrSelect.Length; i++)
            {
                var property = _actionReturnType.GetProperty(arrSelect[i]);
                if (property == null)
                    throw new ExpectException("The query(select) syntax errors:Property '" + arrSelect[i] + "' not exist");

                //判断属性类型是否可用于select
                if (!IsFilter_Select_OrderbyType(property))
                    throw new ExpectException("The query(select) type errors:Property '" + arrSelect[i] + "' can't be select");


                //去除每个select参数的前后空格
                selectField[i] = arrSelect[i].Trim();
                
            }
            
            return selectField;

        }

        /// <summary>
        /// 获取expand参数
        /// </summary>
        /// <param name="expandString"></param>
        /// 
        /// <returns></returns>
        private string[] GetExpandParam(string expandString)
        {
            if (string.IsNullOrEmpty(expandString)) return null;

            //以逗号分隔字符串
            string[] arrExpand = expandString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] expandField = new string[arrExpand.Length];
            //校验输入的expand参数的值的范围必须在当前方法返回类型中的字段值中
            for (int i = 0; i < arrExpand.Length; i++)
            {

                var property = _actionReturnType.GetProperty(arrExpand[i]);
                
                if (property == null)
                    throw new ExpectException("The query(expand) syntax errors:Property '" + arrExpand[i] + "' not exist");
                //判断属性类型是否可用于expand
                if (IsFilter_Select_OrderbyType(property))
                    throw new ExpectException("The query(expand) type errors:Property '" + arrExpand[i] + "' can't be expand");
              
                //去除每个expand参数的前后空格
                expandField[i] = arrExpand[i].Trim();

            }

            return expandField;

        }

        /// <summary>
        /// 获取filter参数
        /// </summary>
        /// <param name="filterString"></param>
        /// 
        /// <returns></returns>
        private Filter[] GetFilterParam(string filterString)
        {
            if (string.IsNullOrEmpty(filterString)) return null;

            //split by "and" logical operator
            //Conver filter operator to lower(case sensitive)
            string lowerFilterString = Regex.Replace(filterString, FilterOperators.And, FilterOperators.And, RegexOptions.IgnoreCase);
            lowerFilterString = Regex.Replace(lowerFilterString, FilterOperators.Or, FilterOperators.Or, RegexOptions.IgnoreCase);

            lowerFilterString = Regex.Replace(lowerFilterString, FilterOperators.LessThan, FilterOperators.LessThan, RegexOptions.IgnoreCase);
            lowerFilterString = Regex.Replace(lowerFilterString, FilterOperators.MoreThan, FilterOperators.MoreThan, RegexOptions.IgnoreCase);
            lowerFilterString = Regex.Replace(lowerFilterString, FilterOperators.Equal, FilterOperators.Equal, RegexOptions.IgnoreCase);
            
            //split by FilterOperators.Add
            string[] arrFilter = lowerFilterString.Split(new string[] { FilterOperators.And }, StringSplitOptions.RemoveEmptyEntries);
            //split by FilterOperators.Add and OR ,to count total condition number
            int totalFilterConditionNum = lowerFilterString
                .Split(new string[] { FilterOperators.And, FilterOperators.Or }, StringSplitOptions.RemoveEmptyEntries)
                .Count();


            Filter[] filterParams = new Filter[totalFilterConditionNum];
            int filterParamsIndex = 0;
            for (int i = 0; i < arrFilter.Length; i++)
            {
                //判断每个被AND条件分割的条件中，是否含有OR条件
                string[] arrSubFilter = arrFilter[i].Split(new string[] { FilterOperators.Or }, StringSplitOptions.RemoveEmptyEntries);
                for (int j=0;j<arrSubFilter.Length;j++)
                {
                    #region
                    string compareOperator = string.Empty;
                    if (arrSubFilter[j].Contains(FilterOperators.LessThan))
                    {
                        compareOperator = FilterOperators.LessThan;
                    }
                    else if (arrSubFilter[j].Contains(FilterOperators.MoreThan))
                    {
                        compareOperator = FilterOperators.MoreThan;
                    }
                    else if (arrSubFilter[j].Contains(FilterOperators.Equal))
                    {
                        compareOperator = FilterOperators.Equal;
                    }
                    string[] arrFieldAndValue = arrSubFilter[j].Split(new string[] { compareOperator }, StringSplitOptions.RemoveEmptyEntries);
                    //去掉数组中所有元素的首位空格
                    arrFieldAndValue = arrFieldAndValue.Select(v => v.Trim()).ToArray();
                    if (arrFieldAndValue.Length != 2)
                        throw new ExpectException("The query(filter) syntax errors:'" + arrSubFilter[j] + "' syntax error");
                    //校验输入的filter参数的属性值的范围必须在当前方法返回类型中的字段值中
                    var property = _actionReturnType.GetProperty(arrFieldAndValue[0]);
                    if (property == null)
                        throw new ExpectException("The query(filter) syntax errors:Property '" + arrFieldAndValue[0] + "' not exist");

                    //判断属性类型是否可用于filter
                    if (!IsFilter_Select_OrderbyType(property))
                        throw new ExpectException("The query(filter) type errors:Property '" + arrFieldAndValue[1] + "' can't be filter");


                    try
                    {
                        //Check if the value match the type of property
                        Common.ConverValueToType(property.PropertyType, arrFieldAndValue[1]);
                    }
                    catch (Exception e)
                    {
                        throw new ExpectException("The query(filter) syntax errors:Property '" + e.Message);
                    }

                    Filter filterParam = new Filter();
                    Dictionary<string, string> propertyValue = new Dictionary<string, string>();
                    propertyValue[arrFieldAndValue[0]] = arrFieldAndValue[1];
                    filterParam.FilterPropertyValue = propertyValue;
                    filterParam.CompareOperator = compareOperator;
                    //如果有OR条件，则被OR分割的数组中，除了第一个条件是AND，其它都是OR
                    //如果没有OR条件，则被OR分割的数组中只有一个元素，第个条件是AND
                    filterParam.LogicalOperator = (j == 0) ? FilterOperators.And : FilterOperators.Or;
                    filterParams[filterParamsIndex++] = filterParam;
                    #endregion
                }

            }

            return filterParams;

        }

        private bool CheckPropertyAndValueType(string propertyName, string value)
        {
            bool success = false;
            var property = _actionReturnType.GetProperty(propertyName);
            if (property != null)
            {
                //值类型要与属性类型一致,如果转换失败则会抛出异常
                try
                {
                    Common.ConverValueToType(property.PropertyType, value);
                    success = true;
                }
                catch
                {
                    success = false;
                }
                
                

            }
            return success;
        }

        /// <summary>
        /// 判断满足Filter,Select,OrderBy的数据类型
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsFilter_Select_OrderbyType(PropertyInfo property)
        {
            bool isFilter_Select_OrderByType = false;
            Type propertyType = property.PropertyType;
            string propertyTypeName = propertyType.Name;

            bool isNullable = propertyTypeName == "Nullable`1";
            if (isNullable)
            {
                propertyTypeName = propertyType.GetGenericArguments()[0].Name;
            }

            List<string> allowTypeName = new List<string>();
            allowTypeName.Add("String");
            allowTypeName.Add("Int16");
            allowTypeName.Add("Int32");
            allowTypeName.Add("Int64");
            allowTypeName.Add("Single");
            allowTypeName.Add("Double");
            allowTypeName.Add("Boolean");
            allowTypeName.Add("DateTime");

            if (allowTypeName.Any(v=>propertyTypeName.StartsWith(v)))
            {
                isFilter_Select_OrderByType = true;
            }
            return isFilter_Select_OrderByType;
        }

        /// <summary>
        /// 在controller action result执行之前调用 
        /// 获取返回结果后，重新组织查询结果格式
        /// </summary>
        /// <param name="context"></param>
        /// 
        public override void OnResultExecuting(ResultExecutingContext context)
        {
           
            var result = context.Result as ObjectResult;

            if (result != null)
            {
                var selectParams = Query.select;
                var expandParams = Query.expand;
                var returnValue = ReturnExpandAndSelectResult(_actionReturnType, result.Value, selectParams, expandParams);

                var responseData = returnValue;
                if (!IsSingleResult(result.Value))
                    responseData = ResponseHandler.ListResponse<object>(returnValue);

                result.Value = responseData;
            } 
            
            
            #region 
            /*
            try
            {
                //获取方法的返回值类型

                if (context.Result is ObjectResult)
                {
                    //获取返回的结果
                    var result = context.Result as ObjectResult;

                    //如果返回结果类型为list，则进行如下操作
                    if (result.Value.GetType().Name == "List`1")
                    {
                        //根据方法返回值类型，生成List<返回值类型>实例
                        IList genericList = CreateList(typeof(List<>), _actionReturnType, result.Value);
                        //对实例集合进行查询操作
                        IList resultData = QueryResult(genericList, _actionReturnType);
                        //对返回结果重新赋值
                        result.Value = resultData;

                    }

                }
            }
            catch (Exception e)
            {
                //出现异常，则不做处理，返回原数据
            }
            */
            #endregion
        }

        /// <summary>
        /// 判断result是单个结果，而不是list
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool IsSingleResult(object result)
        {
            bool isSingleResult = false;
            var isConstructedGenericType = result.GetType().IsConstructedGenericType;
            if (!isConstructedGenericType)
            {
                isSingleResult = true;
            }
            return isSingleResult;
        }

        /// <summary>
        /// 返回expand 和select之后的结果
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private dynamic ReturnExpandAndSelectResult(Type resultType, object result,string[] selectParams, string[] expandParams)
        {
            if (result == null) return result;

            var isConstructedGenericType = result.GetType().IsConstructedGenericType;
            dynamic returnResult ;
            if (IsSingleResult(result))
            {
                //
                returnResult = DynamicExpandAndSelectResult(resultType, result, selectParams,expandParams);
            }
            else
            {
                //if return result is not singleResult ,conver result to IEnumerable
                var results = result as IEnumerable<object>;
                returnResult = DynamicExpandAndSelectResult(resultType, results, selectParams, expandParams);
            }
            return returnResult;
        }

        /// <summary>
        /// 根据expand 和select条件动态构造返回的结果
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        private dynamic DynamicExpandAndSelectResult(Type resultType, IEnumerable<object> results, string[] selectParams, string[] expandParams)
        {
            List<object> returnResults = new List<object>();
            //循环Action返回的结果集
            foreach (var result in results)
            {
                var returnResult = this.DynamicExpandAndSelectResult(resultType, result, selectParams, expandParams);
                returnResults.Add(returnResult);
            }
            return returnResults;
        }

        /// <summary>
        /// 根据expand 和select条件动态构造返回的结果
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private dynamic DynamicExpandAndSelectResult(Type resultType, object result,string[] selectParams,string[] expandParams)
        {
            //新建一个动态词典，用于存放返回的结果
            var returnResult = new ExpandoObject() as IDictionary<string, object>;
            //循环Action返回类型的所有属性
            foreach (var property in resultType.GetProperties())
            {
                var val = property.GetValue(result);

                bool isSelectType = IsFilter_Select_OrderbyType(property);
                if (isSelectType)
                {
                    #region select
                    //if selectParams is null or empey ,add all propertys
                    //if selectParams not null , add propertys which in selectParams
                    if (selectParams == null || selectParams.Length ==0 || selectParams.Contains(property.Name))
                    {
                       returnResult.Add(property.Name, val);
                    }
                    #endregion
                }
                else
                {
                    #region expand逻辑处理
                    //if have expand confition
                    if (expandParams != null && expandParams.Length > 0
                        && expandParams.Contains(property.Name))
                    {
                        var expandResultType = GetClassType(property.PropertyType);
                        var expandResult = ReturnExpandAndSelectResult(expandResultType, val,null,null);
                        returnResult.Add(property.Name, expandResult);
                        
                    }
                    #endregion

                }
                
            }
            return returnResult;
        }
        


        private IList QueryResult(IList listData)
        {
            IList result = listData;
            if (listData.Count != 0 )
            {
                result = OrderByResult(listData);
                result = PageResult(result);
                
            }
            return result;
        }

        /// <summary>
        /// 对查询结果进行排序
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        private IList OrderByResult(IList listData)
        {
            var orderbyParam = Query.orderby;
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
        private IList PageResult(IList listData)
        {
            int? skipParam = Query.skip;
            int? topParam = Query.top;

            var result = listData;
            #region paging by skip and top
            if (skipParam != null && topParam == null)
            {
                int skipNum = System.Convert.ToInt32(skipParam);
                result = listData.Cast<object>().Skip(skipNum).ToList();

            }
            else if (skipParam == null && topParam != null)
            {
                int topNum = System.Convert.ToInt32(topParam);
                result = listData.Cast<object>().Take(topNum).ToList();

            }
            else if (skipParam != null && topParam != null)
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
