using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DotFramework.DynamicQuery
{
    public static class ExpressionHelper
    {
        public static PropertyInfo GetPropertyInfo<TProperty>(this Expression<Func<TProperty>> propertyLambda)
        {
            MemberExpression member = propertyLambda.Body as MemberExpression;

            if (member == null)
            {
                throw new ArgumentException(String.Format("Expression '{0}' refers to a method, not a property.", propertyLambda.ToString()));
            }

            PropertyInfo propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
            {
                throw new ArgumentException(String.Format("Expression '{0}' refers to a field, not a property.", propertyLambda.ToString()));
            }

            return propInfo;
        }

        public static LambdaPropertyInfo GetPropertyInfo(this LambdaExpression propertyLambda)
        {
            MemberExpression member = propertyLambda.Body as MemberExpression;
            UnaryExpression unary = propertyLambda.Body as UnaryExpression;

            if (member == null && unary == null)
            {
                throw new ArgumentException(String.Format("Expression '{0}' refers to a method, not a property.", propertyLambda.ToString()));
            }

            PropertyInfo propInfo = null;
            Type memberType = null;

            if (member != null)
            {
                propInfo = member.Member as PropertyInfo;
                memberType = member.Expression.Type;
            }
            else if (unary != null)
            {
                propInfo = (unary.Operand as MemberExpression).Member as PropertyInfo;
                memberType = (unary.Operand as MemberExpression).Expression.Type;
            }

            if (propInfo == null)
            {
                throw new ArgumentException(String.Format("Expression '{0}' refers to a field, not a property.", propertyLambda.ToString()));
            }

            return new LambdaPropertyInfo
            {
                Property = propInfo,
                MemberType = memberType
            };
        }

        public static object GetValue(this Expression expression)
        {
            object value = null;

            if (expression is ConstantExpression)
            {
                value = GetConstantValue(expression as ConstantExpression);
            }
            else if (expression is MemberExpression)
            {
                value = GetMemberValue(expression as MemberExpression);
            }
            else if (expression is MethodCallExpression)
            {
                value = GetMethodCallValue(expression as MethodCallExpression);
            }
            else if (expression is UnaryExpression)
            {
                value = GetUnaryValue(expression as UnaryExpression);
            }
            else if (expression is ConditionalExpression)
            {
                value = GetConditionalValue(expression as ConditionalExpression);
            }
            else if (expression is ListInitExpression)
            {
                value = GetListInitValue(expression as ListInitExpression);
            }
            else if (expression is NewExpression)
            {
                value = GetNewValue(expression as NewExpression);
            }
            else
            {
                throw new NotSupportedException("Not Supported Expression");
            }

            return value;
        }

        private static object GetConstantValue(ConstantExpression expression)
        {
            return expression.Value;
        }

        private static object GetMemberValue(MemberExpression expression)
        {
            var objectMember = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            try
            {
                return getterLambda.Compile()();
            }
            catch
            {
                return null;
            }
        }

        private static object GetMethodCallValue(MethodCallExpression expression)
        {
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }

        private static object GetConditionalValue(ConditionalExpression conditionalExpression)
        {
            var ifResult = (bool)GetValue(conditionalExpression.Test);
            return ifResult ? GetValue(conditionalExpression.IfTrue) : GetValue(conditionalExpression.IfFalse);
        }

        private static object GetUnaryValue(UnaryExpression expression)
        {
            var operand = (expression as UnaryExpression).Operand;
            var value = GetValue(operand);

            if (expression.NodeType == ExpressionType.Convert)
            {
                Type underlyingT = Nullable.GetUnderlyingType(expression.Type);

                if (underlyingT == null)
                {
                    return Convert.ChangeType(value, expression.Type);
                }
                else
                {
                    return Convert.ChangeType(value, underlyingT);
                }
            }
            else
            {
                return value;
            }
        }

        private static object GetListInitValue(ListInitExpression expression)
        {
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }

        private static object GetNewValue(NewExpression expression)
        {
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }

        public static bool IsNumber(this object value)
        {
            return value is sbyte ||
                   value is byte ||
                   value is short ||
                   value is ushort ||
                   value is int ||
                   value is uint ||
                   value is long ||
                   value is ulong ||
                   value is float ||
                   value is double ||
                   value is decimal;
        }
    }

    public class LambdaPropertyInfo
    {
        public PropertyInfo Property { get; set; }
        public Type MemberType { get; set; }
    }

    public class LogicalLambdaPropertyInfo
    {
        public LambdaPropertyInfo PropertyInfo { get; set; }
        public SqlOperators Operator { get; set; }
        public object Value { get; set; }
    }
}
