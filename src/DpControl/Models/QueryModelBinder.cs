using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace DpControl.Models
{
    /// <summary>
    /// Define QueryModelBinder To Conver The Params from url
    /// To use QueryModelBinder you need Add Attribute [ModelBinder(BinderType = typeof(QueryModelBinder))] for Query class
    /// </summary>
    public class QueryModelBinder: IModelBinder
    {
        public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(Query))
            {
                //获取查询参数
                var queryStrinqg = bindingContext.OperationBindingContext.HttpContext.Request.Query["expand"];

                var queryString = bindingContext.OperationBindingContext.HttpContext.Request.QueryString.ToString() ;

                #region 绑定每一个值到model
                Query model;
                if (Query.ConverQuerystringToObject(queryString, out model))
                {
                    return Task.FromResult(ModelBindingResult.Success(bindingContext.ModelName, model));
                }
                #endregion

            }

            return null;



        }
    }
}
