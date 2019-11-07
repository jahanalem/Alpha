using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Alpha.Infrastructure
{
    public static class ExpressionUtility
    {
        public static PropertyInfo Property<T>(Expression<Func<T, object>> expr)
        {
            var member = ExpressionUtility.Member(expr);
            var prop = member as PropertyInfo;

            if (prop == null)
            {
                throw new InvalidOperationException("Specified member is not a property.");
            }

            return prop;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static MemberInfo Member<T>(Expression<Func<T, object>> expr)
        {
            var memberExpr = expr.Body as MemberExpression;

            if (memberExpr == null)
            {
                var unaryExpr = (UnaryExpression)expr.Body;

                memberExpr = (MemberExpression)unaryExpr.Operand;
            }

            return memberExpr.Member;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static List<PropertyInfo> ConvertToPropertyInfo<TModel>(this Expression<Func<TModel, object>>[] properties)
        {
            List<PropertyInfo> pList = new List<PropertyInfo>();
            foreach (var prop in properties)
            {
                MemberExpression memberExpr = prop.Body as MemberExpression;
                if (memberExpr == null)
                {
                    UnaryExpression unaryExpr = (UnaryExpression)prop.Body;
                    memberExpr = (MemberExpression)unaryExpr.Operand;
                }
                PropertyInfo pi = (PropertyInfo)memberExpr.Member;
                pList.Add(pi);
            }
            return pList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="properties"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool Exist<TModel>(this Expression<Func<TModel, object>>[] properties, string propertyName)
        {
            bool exist = false;
            foreach (var prop in properties)
            {
                MemberExpression memberExpr = prop.Body as MemberExpression;
                if (memberExpr == null)
                {
                    UnaryExpression unaryExpr = (UnaryExpression)prop.Body;
                    memberExpr = (MemberExpression)unaryExpr.Operand;
                }
                var propName = memberExpr.Member.Name;
                if (propName == propertyName)
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(this Expression<Func<T, object>> property)
        {
            string result = null;
            MemberExpression memberExpr = property.Body as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = (UnaryExpression)property.Body;
                memberExpr = (MemberExpression)unaryExpr.Operand;
            }
            result = memberExpr.Member.Name;
            return result;
        }
    }
}
