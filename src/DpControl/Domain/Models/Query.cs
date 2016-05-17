using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using DpControl.Domain.Execptions;
using Microsoft.AspNet.Mvc.ViewFeatures;
using System.Linq.Expressions;
using DpControl.Domain.Entities;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DpControl.Domain.Models
{
    
    public static class Query
    {
        /// <summary>
        /// 排序
        /// 格式：orderby=name,price desc/asc
        ///       orderby=name 
        /// </summary>
        public static OrderBy orderby { get; set; }


        /// <summary>
        /// 跳过前N条
        /// 格式：skip=10
        /// @"^[1-9]([0-9]*)$|^[0-9]$" 只能是0或正整数
        /// </summary>
        public static int? skip { get; set; }

        /// <summary>
        /// 返回前N条
        /// 格式：top=20
        /// @"^[1-9]([0-9]*)$|^[0-9]$" 只能是0或正整数
        /// </summary>
        public static int? top { get; set; }

        /// <summary>
        /// 选中要返回的属性结果
        /// 格式：select = name,price
        /// </summary>
        public static string[] select { get; set; }

        /// <summary>
        /// Expand
        /// </summary>
        public static string[] expand { get; set; }

        /// <summary>
        /// Filter
        /// </summary>
        public static Filter[] filter { get; set; }

        public static void EmptyQuery()
        {
            skip = null;
            top = null;
            orderby = null;
            filter = null;
            select = null;
            expand = null;
        }

        /// <summary>
        /// Add extra And Confition
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public static void And(string propertyName, string propertyValue)
        {
            Filter filter = new Filter();
            var filterPropertyValue = new Dictionary<string, string>();
            filterPropertyValue.Add(propertyName, propertyValue);
            filter.FilterPropertyValue = filterPropertyValue;
            filter.CompareOperator = FilterOperators.Equal;
            filter.LogicalOperator = FilterOperators.And;
            Query.AddFilterCondition(filter);
        }


        /// <summary>
        /// Add extra filter condition
        /// </summary>
        private static void AddFilterCondition(List<Filter> filters)
        {
            foreach(var filter in filters)
            {
                AddFilterCondition(filter);
            }
        }

        /// <summary>
        /// Add extra filter condition
        /// </summary>
        private static void AddFilterCondition(Filter filter)
        {
            //if Query.filter is not null,then conver to list
            var queryFilter = Query.filter == null ? new List<Filter>() : Query.filter.ToList();
            //add filter param to queryFilter
            queryFilter.Add(filter);
            //reset value form Gobal Query filter
            Query.filter = queryFilter.ToArray();
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
        
        /// <summary>
        /// this operator will be: eq、lt 、gt
        /// </summary>
        public string CompareOperator { get; set; }

        /// <summary>
        /// this operator will be : and 、or
        /// </summary>
        public string LogicalOperator { get; set; }

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
            And,
            Or
        };
    }

    public static class ExpandOperator
    {
        /// <summary>
        /// Expand Related Entities
        /// </summary>
        /// <param name="result"></param>
        /// 
        /// <returns></returns>
        public static IQueryable<T> ExpandRelatedEntities<T>(IQueryable<T> result)
        {
            if (typeof(T) == typeof(Customer))
            {
                var returnResult = result as IQueryable<Customer>;
                var needExpandProjects = NeedExpand("Projects");
                if (needExpandProjects)
                    returnResult = returnResult.Include(c => c.Projects);
                    

                return (IQueryable<T>)returnResult;

            }else if (typeof(T) == typeof(Project))
            {
                #region Project
                var returnResult = result as IQueryable<Project>;
                var needExpandScenes = ExpandOperator.NeedExpand("Scenes");
                var needExpandGroups = ExpandOperator.NeedExpand("Groups");
                var needExpandLocations = ExpandOperator.NeedExpand("Locations");
                var needExpandHolidays = ExpandOperator.NeedExpand("Holidays");
                var needExpandCustomer = ExpandOperator.NeedExpand("Customer");

                if (needExpandScenes)
                    returnResult = returnResult.Include(p => p.Scenes);
                if (needExpandGroups)
                    returnResult = returnResult.Include(p => p.Groups);
                if (needExpandLocations)
                    returnResult = returnResult.Include(p => p.Locations);
                if (needExpandHolidays)
                    returnResult = returnResult.Include(p => p.Holidays);
                if (needExpandCustomer)
                    returnResult = returnResult.Include(p => p.Customer);

                return (IQueryable<T>)returnResult;
                #endregion
            }else if (typeof(T) == typeof(Scene))
            {
                #region 
                var returnResult = result as IQueryable<Scene>;
                var needExpandSceneSegments = ExpandOperator.NeedExpand("SceneSegments");
                var needExpandGroups = ExpandOperator.NeedExpand("Groups");
                var needExpandProject = ExpandOperator.NeedExpand("Project");

                if (needExpandSceneSegments)
                    returnResult = returnResult.Include(s => s.SceneSegments);
                if (needExpandGroups)
                    returnResult = returnResult.Include(s => s.Groups);
                if (needExpandProject)
                    returnResult = returnResult.Include(s => s.Project);

                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(SceneSegment))
            {
                #region 
                var returnResult = result as IQueryable<SceneSegment>;
                var needExpandScene = ExpandOperator.NeedExpand("Scene");
                if (needExpandScene)
                    returnResult = returnResult.Include(s => s.Scene);
                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(Group))
            {
                #region 
                var returnResult = result as IQueryable<Group>;
                var needExpandLocations = ExpandOperator.NeedExpand("Locations");
                var needExpandProject = ExpandOperator.NeedExpand("Project");
                var needExpandScene = ExpandOperator.NeedExpand("Scene");
                var needExpandUsers = ExpandOperator.NeedExpand("Users");

                if (needExpandUsers)
                    returnResult = returnResult.Include(g => g.UserGroups).ThenInclude(gl => gl.User);
                if (needExpandLocations)
                    returnResult = returnResult.Include(g => g.GroupLocations).ThenInclude(gl => gl.Location);
                if (needExpandProject)
                    returnResult = returnResult.Include(g => g.Project);
                if (needExpandScene)
                    returnResult = returnResult.Include(g => g.Scene);
                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(Location))
            {
                #region 
                var returnResult = result as IQueryable<Location>;
                var needExpandGroups = ExpandOperator.NeedExpand("Groups");
                var needExpandAlarms = ExpandOperator.NeedExpand("Alarms");
                var needExpandLogs = ExpandOperator.NeedExpand("Logs");
                var needExpandProject = ExpandOperator.NeedExpand("Project");
                var needExpandDevice = ExpandOperator.NeedExpand("Device");
                var needExpandUsers = ExpandOperator.NeedExpand("Users");

                if (needExpandUsers)
                    returnResult = returnResult.Include(g => g.UserLocations).ThenInclude(gl => gl.User);
                if (needExpandGroups)
                    returnResult = returnResult.Include(l => l.GroupLocations).ThenInclude(gl => gl.Group);
                if (needExpandAlarms)
                    returnResult = returnResult.Include(l => l.Alarms);
                if (needExpandLogs)
                    returnResult = returnResult.Include(l => l.Logs);
                if (needExpandProject)
                    returnResult = returnResult.Include(l => l.Project);
                if (needExpandDevice)
                    returnResult = returnResult.Include(l => l.Device);

                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(Device))
            {
                #region 
                var returnResult = result as IQueryable<Device>;
                var needExpandLocations = ExpandOperator.NeedExpand("Locations");
                if (needExpandLocations)
                    returnResult = returnResult.Include(d => d.Locations);
                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(AlarmMessage))
            {
                #region 
                var returnResult = result as IQueryable<AlarmMessage>;
                var needExpandAlarms = ExpandOperator.NeedExpand("Alarms");
                if (needExpandAlarms)
                    returnResult = returnResult.Include(a => a.Alarms);
                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(Alarm))
            {
                #region 
                var returnResult = result as IQueryable<Alarm>;
                var needExpandLocation = ExpandOperator.NeedExpand("Location");
                var needExpandAlarmMessage = ExpandOperator.NeedExpand("AlarmMessage");
                if (needExpandLocation)
                    returnResult = returnResult.Include(a => a.Location);
                if (needExpandAlarmMessage)
                    returnResult = returnResult.Include(a => a.AlarmMessage);
                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(LogDescription))
            {
                #region 
                var returnResult = result as IQueryable<LogDescription>;
                var needExpandLogs = ExpandOperator.NeedExpand("Logs");
                if (needExpandLogs)
                    returnResult = returnResult.Include(l => l.Logs);
                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(Log))
            {
                #region 
                var returnResult = result as IQueryable<Log>;
                var needExpandLogDescription = ExpandOperator.NeedExpand("LogDescription");
                var needExpandLocation = ExpandOperator.NeedExpand("Location");

                if (needExpandLogDescription)
                    returnResult = returnResult.Include(l => l.LogDescription);
                if (needExpandLocation)
                    returnResult = returnResult.Include(l => l.Location);
                return (IQueryable<T>)returnResult;
                #endregion
            }
            else if (typeof(T) == typeof(Holiday))
            {
                #region 
                var returnResult = result as IQueryable<Holiday>;
                var needExpandProject = ExpandOperator.NeedExpand("Project");
                if (needExpandProject)
                    returnResult = returnResult.Include(h=>h.Project);
                return (IQueryable<T>)returnResult;
                #endregion
            }else if(typeof(T) == typeof(ApplicationUser))
            {
                //用户下的角色
                var returnResult = result as IQueryable<ApplicationUser>;
                var needExpandRoles = ExpandOperator.NeedExpand("Roles");
                var needExpandGroups = ExpandOperator.NeedExpand("Groups");
                var needExpandLocations = ExpandOperator.NeedExpand("Locations");

                if (needExpandRoles)
                    returnResult = returnResult.Include(l => l.Roles);
                if (needExpandGroups)
                    returnResult = returnResult.Include(l => l.UserGroups).ThenInclude(gl => gl.Group);
                if (needExpandLocations)
                    returnResult = returnResult.Include(l => l.UserLocations).ThenInclude(gl => gl.Location);

                return (IQueryable<T>)returnResult;

            } else if (typeof(T) == typeof(IdentityRole))
            {
                //角色下的用户
                var returnResult = result as IQueryable<IdentityRole>;
                var needExpandUsers = ExpandOperator.NeedExpand("Users");

                if (needExpandUsers)
                    returnResult = returnResult.Include(l => l.Users);
                
                return (IQueryable<T>)returnResult;
            }


            return result;
           
        }

        /// <summary>
        /// Judge if need expand
        /// </summary>
        /// <param name="relatedEntityName"></param>
        /// <returns></returns>
        private static bool NeedExpand(string relatedEntityName)
        {
            var expandParams = Query.expand;

            bool needExpand = false;
            if (expandParams != null && expandParams.Length > 0
                        && expandParams.Contains(relatedEntityName))
            {
                needExpand = true;
            }

            return needExpand;
        }
    }

    public static class QueryOperate<T> where T :class //T 必须是引用类型
    {
        public static IQueryable<T> Execute(IQueryable<T> queryData)
        {
            
            int? skip = Query.skip;
            int? top = Query.top;
            OrderBy orderbyParam = Query.orderby;
            Filter[] filterParams = Query.filter;

            //Filter operate
            queryData = Filter(queryData,filterParams);
            //paging operate
            queryData = Paging(queryData, skip, top);
            //orderby operate
            queryData = OrderBy(queryData,orderbyParam);
            
            return queryData;
        }
        #region Filter
        private static IQueryable<T> Filter(IQueryable<T> query, Filter[] filterParams)
        {
            
            var allFilterExpresson = ConstructMulitFilterLambadExpression(filterParams);
            if (allFilterExpresson !=null)
              query = query.Where(allFilterExpresson);
            return query;
        }

        /// <summary>
        /// Construct mulit filter conditions
        /// </summary>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> ConstructMulitFilterLambadExpression(Filter[] filterParams)
        {
            Expression<Func<T, bool>> mulitFilterExpression = null;
            if (filterParams != null)
            {
                for (int i = 0; i < filterParams.Length; i++)
                {
                    if (i == 0)
                    {
                        mulitFilterExpression = ConstructFilterLambadExpression(filterParams[i]);

                    }
                    else
                    {
                        var filterExpression = ConstructFilterLambadExpression(filterParams[i]);
                        if (filterParams[i].LogicalOperator == FilterOperators.Or)
                        {
                            mulitFilterExpression = mulitFilterExpression.Or(filterExpression);
                        }
                        else
                        {
                            mulitFilterExpression = mulitFilterExpression.And(filterExpression);
                        }
                    }
                    

                }
            }
            return mulitFilterExpression;
        }

        /// <summary>
        /// Construct single filter condition
        /// </summary>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> ConstructFilterLambadExpression(Filter filterParam)
        {
            Expression<Func<T, bool>> queryExpression = null ;
            if (filterParam != null)
            {
                string propertyName = filterParam.FilterPropertyValue.First().Key;
                string propertyValue = filterParam.FilterPropertyValue.First().Value;
                string filterOperator = filterParam.CompareOperator;

                var valueTypr = typeof(T).GetProperty(propertyName).PropertyType;
                ParameterExpression param = Expression.Parameter(typeof(T), "c");
                Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
                //Conver value to propertyType
                var converValue = Convert.ChangeType(propertyValue, valueTypr);
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

                queryExpression = Expression.Lambda<Func<T, bool>>(filter, param);
                return queryExpression;
                
            }
            return queryExpression;
            
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
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (_map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
    public static class Utility
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}
