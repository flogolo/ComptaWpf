using System;
using System.Reflection;
using System.Linq.Expressions;

namespace CommonLibrary.Tools
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Retourne le nom d'une propriété de l'objet cible.
        /// </summary>
        /// <typeparam name="TBr">Type de l'objet cible</typeparam>
        /// <param name="propertyExpression">Expression de type accès (get) à une propriété d'un objet du type cible.</param>
        /// <returns>Nom de la propriété référencé dans l'expression.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "magic lambda parameter.")]
        public static string GetPropertyName<TBr>(this Expression<Func<TBr, Object>> propertyExpression)
        {
            var lambda = propertyExpression as LambdaExpression;
            MemberExpression memberExpression = null;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                if (unaryExpression != null) memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo != null) return propertyInfo.Name;
            }
            return String.Empty;
        }

        /// <summary>
        /// Retourne le nom d'une propriété.
        /// </summary>
        /// <param name="propertyExpression">Expression de type accès (get) à une propriété d'un objet du type cible.</param>
        /// <returns>Nom de la propriété référencé dans l'expression.</returns>
        public static string GetPropertyName(this Expression<Predicate<object>> propertyExpression)
        {
            var lambda = propertyExpression as LambdaExpression;
            MemberExpression memberExpression = null;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                if (unaryExpression != null) memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo != null) return propertyInfo.Name;
            }
            return string.Empty;
        }

    }
}
