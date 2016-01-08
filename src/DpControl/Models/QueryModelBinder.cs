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
            if (bindingContext.ModelType == typeof(Query)
                && bindingContext.ValueProvider.GetValue(bindingContext.ModelName) != null)
            {
                Query model;
                var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue as string;

                if (Query.TryParse(val, out model))
                {
                    return Task.FromResult(ModelBindingResult.Success(bindingContext.ModelName, model));
                }
            }

            return null;



        }
    }
}
