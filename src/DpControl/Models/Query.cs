using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace DpControl.Models
{
    [TypeConverter(typeof(QueryConverter))]
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

    public class QueryConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                Query query;
                if (Query.TryParse((string)value, out query))
                {
                    return query;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if ((destinationType == typeof(string)))
                return true;
            else
                return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            
            // call the base converter
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    //public class QueryModelBinder : IModelBinder
    //{
    //    public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        if (bindingContext.ModelType == typeof(CancellationToken))
    //        {
    //            var model = bindingContext.OperationBindingContext.HttpContext.RequestAborted;
    //            var validationNode =
    //                new ModelValidationNode(bindingContext.ModelName, bindingContext.ModelMetadata, model);
    //            return Task.FromResult(new ModelBindingResult(
    //                model,
    //                bindingContext.ModelName,
    //                isModelSet: true,
    //                validationNode: validationNode));
    //        }

    //        return Task.FromResult<ModelBindingResult>(null);
    //    }
    //}
}
