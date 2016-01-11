using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Models
{
    /// <summary>
    /// Define QueryConverter To Conver The Params from url
    /// To use QueryConverter you need Add Attribute [TypeConverter(typeof(QueryConverter))] for Query class
    /// </summary>
    public class QueryConverter: TypeConverter
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
                //Query query;
                //if (Query.ConverQuerystringToObject((string)value, out query))
                //{
                //    return query;
                //}
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
}
