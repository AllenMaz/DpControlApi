using DpControl.Domain.Execptions;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Utility
{
    public static class Common
    {
        /// <summary>
        /// Conver object value to Type
        /// if falied throw exception
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConverValueToType(Type type, object value)
        {
            try
            {
                bool isNullable = type.Name == "Nullable`1";
                if (isNullable)
                {
                    type = type.GetGenericArguments()[0];
                }
                else if (type.Name == "String")
                {
                    return value.ToString();
                }
                else if (type.Name == "Int16")
                {
                    return Convert.ToInt16(value);
                }
                else if (type.Name == "Int32")
                {
                    return Convert.ToInt32(value);
                }
                else if (type.Name == "Int64")
                {
                    return Convert.ToInt64(value);
                }
                else if (type.Name == "Boolean")
                {
                    return Convert.ToBoolean(value);
                }
                else if (type.Name == "DateTime")
                {
                    return Convert.ToDateTime(value);
                }
                else if (type.Name == "Guid")
                {
                    return new Guid(value.ToString());
                }
                else {
                    throw new ExpectException(type.Name+" is not supported");
                }
                

            }
            catch (Exception e)
            {
                throw new ExpectException("Conver value to Type Failed:"+e.Message);
            }

            return null;
        }


        public static bool PropertyExists<T>(string propertyName)
        {
            return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) != null;
        }

        
    }

    public static class Role
    {
        public const string Admin = "Admin";
        public const string Engineer = "Engineer";
        public const string CustomerService = "CustomerService";
        public const string NormalUser = "NormalUser";

        public static string ConstructAuthorizationRoles(string[] roleParams)
        {
            string authroizationRoles = string.Empty;
            for (int i=0;i<roleParams.Length;i++)
            {
                authroizationRoles += roleParams[i];
                if (i != roleParams.Length -1) 
                    authroizationRoles += ",";
            }

            return authroizationRoles;
        }
    }
}
