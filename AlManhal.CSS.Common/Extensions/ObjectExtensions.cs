using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        #region Contants

        private const string REQUIRED_MSG = "Field is required";
        private const string NOT_SUPPORTED_EXPRESSION = "Expression '{0}' not supported.";
        private const string FIELD = "Field";
        #endregion

        #region Methods

        public static string ToNullOrString(this object o)
        {
            return o == null ? null : o.ToString();
        }

        /// <summary>
        /// Builds a dictionary from the object's properties
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToPropertyDictionary(this object o)
        {
            if (o == null)
                return null;
            return o.GetType().GetProperties()
                .Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(o, null))).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Get value of the object property
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object o, string propertyName)
        {
            object propVal = null;

            try
            {
                propVal = o.GetType().GetProperty(propertyName).GetValue(o, null);
            }
            catch (Exception)
            {
                propVal = null;
            }

            return propVal;
        }

        /// <summary>
        /// Determines the member name of a field or property in the form of a string, given a
        /// strongly-typed expression.
        /// </summary>
        /// <typeparam name="T">The type of the class containing the field or property.</typeparam>
        /// <param name="obj">The object containing the field or property.</param>
        /// <param name="expression">The expression that identifies the field or property.</param>
        /// <returns>The member name in the form of a string.</returns>
        public static object GetPropertyValue<T>(this T obj, Expression<Func<T, object>> expression)
        {

            if (object.Equals(expression, null))
            {
                throw new NullReferenceException(REQUIRED_MSG);
            }

            MemberExpression expr = null;

            if (expression.Body is MemberExpression)
            {
                expr = (MemberExpression)expression.Body;
            }
            else if (expression.Body is UnaryExpression)
            {
                expr = (MemberExpression)((UnaryExpression)expression.Body).Operand;
            }
            else
            {
                const string Format = NOT_SUPPORTED_EXPRESSION;
                string message = string.Format(Format, expression);

                throw new ArgumentException(message, FIELD);
            }

            return obj.GetPropertyValue(expr.Member.Name);
        }

        public static object GetPropertyName<T>(this T obj, Expression<Func<T, object>> expression)
        {

            if (object.Equals(expression, null))
            {
                throw new NullReferenceException(REQUIRED_MSG);
            }

            MemberExpression expr = null;

            if (expression.Body is MemberExpression)
            {
                expr = (MemberExpression)expression.Body;
            }
            else if (expression.Body is UnaryExpression)
            {
                expr = (MemberExpression)((UnaryExpression)expression.Body).Operand;
            }
            else
            {
                const string Format = NOT_SUPPORTED_EXPRESSION;
                string message = string.Format(Format, expression);

                throw new ArgumentException(message, FIELD);
            }

            return expr.Member.Name;
        }

        /// <summary>
        /// Check if the object has property
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsExistingProperty(this Type type, string propertyName)
        {
            bool isExisting = false;

            try
            {
                isExisting = (type.GetProperty(propertyName) != null);
            }
            catch (Exception)
            {
                isExisting = false;
            }

            return isExisting;
        }

        /// <summary>
        /// Provide AddRange method to ICollection type
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static void AddRange<T>(this ICollection<T> destination,
                               IEnumerable<T> source)
        {
            List<T> list = destination as List<T>;

            if (list != null)
            {
                list.AddRange(source);
            }
            else
            {
                foreach (T item in source)
                {
                    destination.Add(item);
                }
            }
        }

        #endregion
    }
}
