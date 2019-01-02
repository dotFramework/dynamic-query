using DotFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DotFramework.DynamicQuery
{
    public class FilterFactory : SingletonProvider<FilterFactory>
    {
        #region Public Methods

        public AbstractFilter CreateFilter<T1>(Expression<Func<T1, Boolean>> expression)
        {
            return CreateFilter(expression.Body);
        }

        public AbstractFilter CreateFilter<T1, T2>(Expression<Func<T1, T2, Boolean>> expression)
        {
            return CreateFilter(expression.Body);
        }

        public AbstractFilter CreateFilter<T1, T2, T3>(Expression<Func<T1, T2, T3, Boolean>> expression)
        {
            return CreateFilter(expression.Body);
        }

        public AbstractFilter CreateFilter<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, Boolean>> expression)
        {
            return CreateFilter(expression.Body);
        }

        public AbstractFilter CreateFilter<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, Boolean>> expression)
        {
            return CreateFilter(expression.Body);
        }

        public FilterExpression CreateFilterExpression(string tableName, string fieldName, SqlOperators sqlOperator, object value)
        {
            return new FilterExpression(tableName, fieldName, sqlOperator, value);
        }

        public InFilter CreateInFilter(string tableName, string fieldName, IList filterValues, InFilterCondition filterCondition)
        {
            return new InFilter(tableName, fieldName, filterValues, filterCondition);
        }

        public AbstractFilter CreateAndFilter(AbstractFilter filterExpressionLeft, AbstractFilter filterExpressionRight)
        {
            return filterExpressionLeft & filterExpressionRight;
        }

        public AbstractFilter CreateOrFilter(AbstractFilter filterExpressionLeft, AbstractFilter filterExpressionRight)
        {
            return filterExpressionLeft | filterExpressionRight;
        }

        #endregion

        #region Private Methods

        private AbstractFilter CreateFilter(Expression expression, bool hasNotOperator = false)
        {
            AbstractFilter filter = null;

            if (expression is BinaryExpression)
            {
                filter = CreateBinaryFilter(expression as BinaryExpression);
            }
            else if (expression is MethodCallExpression)
            {
                filter = CreateMethodCallFilter(expression as MethodCallExpression, hasNotOperator);
            }
            else if (expression is UnaryExpression)
            {
                filter = CreateUnaryFilter(expression as UnaryExpression);
            }
            else if (expression is MemberExpression)
            {
                filter = CreateMemberFilter(expression as MemberExpression, hasNotOperator);
            }
            else
            {
                throw new NotSupportedException("Not Supported Exception");
            }

            return filter;
        }

        private AbstractFilter CreateMemberFilter(MemberExpression expression, bool hasNotOperator = false)
        {
            if (expression.GetValue() != null || expression.Type != typeof(bool))
            {
                throw new ArgumentException("Unsupported Expression");
            }

            var propertyInfo = new LambdaPropertyInfo
            {
                Property = expression.Member as PropertyInfo,
                MemberType = expression.Expression.Type
            };

            string tableName = propertyInfo.MemberType.Name;
            string fieldName = propertyInfo.Property.Name;

            SqlOperators sqlOperator = SqlOperators.Equal;
            object filterValue = !hasNotOperator;

            return CreateFilterExpression(tableName, fieldName, sqlOperator, filterValue);
        }

        private AbstractFilter CreateUnaryFilter(UnaryExpression expression)
        {
            if (expression.NodeType != ExpressionType.Not)
            {
                throw new ArgumentException("Unsupported ExpressionType");
            }

            if (!(expression.Operand is MethodCallExpression || expression.Operand is MemberExpression))
            {
                throw new ArgumentException("Unsupported Expression");
            }

            return CreateFilter((expression as UnaryExpression).Operand, true);
        }

        private AbstractFilter CreateBinaryFilter(BinaryExpression expression)
        {
            AbstractFilter filter = null;

            if (expression.Left is MemberExpression || expression.Left is UnaryExpression)
            {
                MemberExpression leftExpression = null;

                if (expression.Left is UnaryExpression)
                {
                    leftExpression = (expression.Left as UnaryExpression).Operand as MemberExpression;
                }
                else
                {
                    leftExpression = (expression.Left as MemberExpression);
                }

                var propertyInfo = new LambdaPropertyInfo
                {
                    Property = leftExpression.Member as PropertyInfo,
                    MemberType = leftExpression.Expression.Type
                };

                string tableName = propertyInfo.MemberType.Name;
                string fieldName = propertyInfo.Property.Name;

                SqlOperators sqlOperator = SqlOperators.Equal;

                switch (expression.NodeType)
                {
                    case ExpressionType.Equal:
                        sqlOperator = SqlOperators.Equal;
                        break;
                    case ExpressionType.NotEqual:
                        sqlOperator = SqlOperators.NotEqual;
                        break;
                    case ExpressionType.GreaterThan:
                        sqlOperator = SqlOperators.Greater;
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        sqlOperator = SqlOperators.GreaterOrEqual;
                        break;
                    case ExpressionType.LessThan:
                        sqlOperator = SqlOperators.Less;
                        break;
                    case ExpressionType.LessThanOrEqual:
                        sqlOperator = SqlOperators.LessOrEqual;
                        break;
                    default:
                        throw new ArgumentException("Unsupported ExpressionType");
                }

                object filterValue = null;

                if (expression.Right is ConstantExpression)
                {
                    filterValue = (expression.Right as ConstantExpression).Value;
                }
                else if (expression.Right is MemberExpression)
                {
                    filterValue = (expression.Right as MemberExpression).GetValue();
                }
                else if (expression.Right is MethodCallExpression)
                {
                    filterValue = Expression.Lambda(expression.Right as MethodCallExpression).Compile().DynamicInvoke();
                }
                else if (expression.Right is UnaryExpression)
                {
                    filterValue = expression.Right.GetValue();
                }

                filter = CreateFilterExpression(tableName, fieldName, sqlOperator, filterValue);
            }
            else
            {
                AbstractFilter lefFilterBase = CreateFilter(expression.Left);
                AbstractFilter righFilterBase = CreateFilter(expression.Right);

                switch (expression.NodeType)
                {
                    case ExpressionType.AndAlso:
                    case ExpressionType.And:
                        filter = CreateAndFilter(lefFilterBase, righFilterBase);
                        break;
                    case ExpressionType.OrElse:
                    case ExpressionType.Or:
                        filter = CreateOrFilter(lefFilterBase, righFilterBase);
                        break;
                    default:
                        throw new ArgumentException("Unsupported ExpressionType");
                }
            }

            return filter;
        }

        private List<Tuple<String, String>> GetMembers(Expression expression, bool hasNotOperator = false)
        {
            List<Tuple<String, String>> members = null;

            if (expression is BinaryExpression)
            {
                members = GetBinaryMembers(expression as BinaryExpression);
            }

            return members;
        }

        private List<Tuple<String, String>> GetBinaryMembers(BinaryExpression expression)
        {
            List<Tuple<String, String>> members = new List<Tuple<String, String>>();

            if (expression.Left is MemberExpression || expression.Left is UnaryExpression)
            {
                MemberExpression leftExpression = null;

                if (expression.Left is UnaryExpression)
                {
                    leftExpression = (expression.Left as UnaryExpression).Operand as MemberExpression;
                }
                else
                {
                    leftExpression = (expression.Left as MemberExpression);
                }

                var propertyInfo = new LambdaPropertyInfo
                {
                    Property = leftExpression.Member as PropertyInfo,
                    MemberType = leftExpression.Expression.Type
                };
                members.Add(new Tuple<String, String>(propertyInfo.Property.Name, "1"));
            }
            else
            {
                List<Tuple<String, String>> lefFilterBase = GetMembers(expression.Left);
                List<Tuple<String, String>> righFilterBase = GetMembers(expression.Right);

                members.AddRange(lefFilterBase);
                members.AddRange(righFilterBase);
            }
            return members;
        }

        private AbstractFilter CreateMethodCallFilter(MethodCallExpression expression, bool hasNotOperator = false)
        {
            string tableName = String.Empty;
            string fieldName = String.Empty;

            object filterValue = null;

            MemberExpression objectExpression = null;
            Expression argumentsExpression = null;

            if (expression.Object != null)
            {
                objectExpression = expression.Object as MemberExpression;
                argumentsExpression = expression.Arguments[0];
            }
            else
            {
                objectExpression = expression.Arguments[0] as MemberExpression;
                argumentsExpression = expression.Arguments[1];
            }

            LambdaPropertyInfo propertyInfo = new LambdaPropertyInfo
            {
                Property = objectExpression.Member as PropertyInfo,
                MemberType = objectExpression.Expression.Type
            };

            tableName = propertyInfo.MemberType.Name;
            fieldName = propertyInfo.Property.Name;

            filterValue = argumentsExpression.GetValue();

            AbstractFilter filter = null;

            if (!hasNotOperator)
            {
                switch (expression.Method.Name)
                {
                    case "StartsWith":
                        filter = CreateFilterExpression(tableName, fieldName, SqlOperators.StartsWithLike, filterValue);
                        break;
                    case "EndsWith":
                        filter = CreateFilterExpression(tableName, fieldName, SqlOperators.EndsWithLike, filterValue);
                        break;
                    case "Contains":
                        filter = CreateFilterExpression(tableName, fieldName, SqlOperators.Like, filterValue);
                        break;
                    case "In":
                        filter = CreateInFilter(tableName, fieldName, filterValue as IList, InFilterCondition.In);
                        break;
                    default:
                        throw new ArgumentException("Unsupported Method");
                }
            }
            else
            {
                switch (expression.Method.Name)
                {
                    case "StartsWith":
                        filter = CreateFilterExpression(tableName, fieldName, SqlOperators.NotStartsWithLike, filterValue);
                        break;
                    case "EndsWith":
                        filter = CreateFilterExpression(tableName, fieldName, SqlOperators.NotEndsWithLike, filterValue);
                        break;
                    case "Contains":
                        filter = CreateFilterExpression(tableName, fieldName, SqlOperators.NotLike, filterValue);
                        break;
                    case "In":
                        filter = CreateInFilter(tableName, fieldName, filterValue as IList, InFilterCondition.NotIn);
                        break;
                    default:
                        throw new ArgumentException("Unsupported Method");
                }
            }

            return filter;
        }

        #endregion
    }
}
