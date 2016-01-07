using DpControl.Controllers.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DpControl.Utility
{
    public static class ModelHandler
    {
        /// <summary>
        /// Conver Entity To Model
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static List<M> ConverEntityToModel<M, E>(List<E> entitys) where M : new()
        {
            List<M> models = new List<M>();
            foreach (E entity in entitys)
            {
                M model = new M();
                model = ConverEntityToModel<M, E>(entity);
                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// Conver Entity To Model
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static M ConverEntityToModel<M, E>(E entity) where M : new()
        {
            try
            {
                Type entityType = typeof(E);
                Type modelType = typeof(M);

                M model = new M();
                var entityProperties = entityType.GetProperties();
                var modelProperties = modelType.GetProperties();

                foreach (var ePropertie in entityProperties)
                {
                    foreach (var mPropertie in modelProperties)
                    {
                        if (ePropertie.Name == mPropertie.Name)
                        {
                            object value = SwitchPropertyValue(mPropertie.PropertyType, ePropertie.GetValue(entity, null));
                            mPropertie.SetValue(model, value);

                            //if (mPropertie.PropertyType == typeof(string))
                            //{

                            //    var ePropertieValue = ePropertie.GetValue(entity, null).ToString();
                            //    mPropertie.SetValue(model, ePropertieValue);
                            //}
                        }
                    }
                }

                return model;

            }
            catch (Exception e)
            {
                throw new ProcedureException("实体类转换成领域模型失败。错误：" + e.Message);
            }

        }

        /// <summary>
        /// Conver Model To Entity
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        public static List<E> ConverModelToEntity<M, E>(List<M> models) where E : new()
        {
            List<E> entitys = new List<E>();
            foreach (M model in models)
            {
                E entity = new E();
                entity = ConverModelToEntity<M, E>(model);
                entitys.Add(entity);
            }

            return entitys;
        }

        /// <summary>
        /// Conver Model To Entity
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static E ConverModelToEntity<M, E>(M model) where E : new()
        {
            try
            {
                Type entityType = typeof(E);
                Type modelType = typeof(M);

                E entity = new E();
                var entityProperties = entityType.GetProperties();
                var modelProperties = modelType.GetProperties();

                foreach (var ePropertie in entityProperties)
                {
                    foreach (var mPropertie in modelProperties)
                    {
                        if (ePropertie.Name == mPropertie.Name)
                        {

                            object value = SwitchPropertyValue(ePropertie.PropertyType, mPropertie.GetValue(model, null));
                            ePropertie.SetValue(entity, value, null);
                        }
                    }
                }

                return entity;

            }
            catch (Exception e)
            {
                throw new ProcedureException("领域模型转换成实体类失败。错误：" + e.Message);
            }
        }
        

        public static object SwitchPropertyValue(Type type, object value)
        {
            //if (value == null)
            //{
            //    return type.IsValueType ? Activator.CreateInstance(type) : null;
            //}

            bool isNullable = type.Name == "Nullable`1";
            if (isNullable)
            {
                type = type.GetGenericArguments()[0];
            }
            if (type.Name == "Int16")
            {
                return Convert.ToInt16(value);
            }
            if (type.Name == "Int32")
            {
                return Convert.ToInt32(value);
            }
            if (type.Name == "Int64")
            {
                return Convert.ToInt64(value);
            }
            if (type.Name == "String")
            {
                return value.ToString().Trim();
            }
            if (type.Name == "Boolean")
            {
                return Convert.ToBoolean(value);
            }
            if (type.Name == "DateTime")
            {
                return Convert.ToDateTime(value);
            }
            if (type.Name == "Guid")
            {
                return new Guid(value.ToString());
            }

            throw new NotSupportedException("数据类型不支持");
        }
    }
}
