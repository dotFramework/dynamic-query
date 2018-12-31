using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotFramework.DynamicQuery
{
    public class UpdateQueryBuilder<TSource> : AbstractQueryBuilder<TSource, UpdateQueryBuilder<TSource>, UpdateQuery>
    {
        #region Constructors

        public UpdateQueryBuilder() : base()
        {

        }

        public UpdateQueryBuilder(Type sourceType) : base(sourceType)
        {

        }

        public UpdateQueryBuilder(UpdateQuery query) : base(query)
        {

        }

        #endregion

        #region Public Methods

        public UpdateQueryBuilder<TSource> Set(Expression<Func<TSource, TSource>> expression)
        {
            var type = expression.Body.GetType();

            if (expression.Body is MemberInitExpression)
            {
                var body = expression.Body as MemberInitExpression;

                foreach (var binding in body.Bindings.OfType<MemberAssignment>())
                {
                    UpdateField updateField = CreateUpdateField(binding);
                    AddUpdateField(updateField);
                }
            }
            else
            {
                throw new NotSupportedException("Unsupported Expression");
            }

            return this;
        }

        #endregion

        #region Private Methods

        private void AddUpdateField(IEnumerable<UpdateField> UpdateFields)
        {
            Query.UpdateFieldList.AddRange(UpdateFields);
        }

        private void AddUpdateField(UpdateField UpdateField)
        {
            Query.UpdateFieldList.Add(UpdateField);
        }

        private UpdateField CreateUpdateField(MemberAssignment binding)
        {
            string fieldName = binding.Member.Name;
            object value = GetValue(binding.Expression);

            UpdateField updateField = new UpdateField
            {
                FieldName = fieldName,
                Value = value
            };

            return updateField;
        }

        public object GetValue(Expression expression)
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
            else if (expression is BinaryExpression)
            {
                value = GetBinaryValue(expression as BinaryExpression);
            }
            else
            {
                throw new NotSupportedException("Not Supported Expression");
            }

            return value;
        }

        private object GetConstantValue(ConstantExpression expression)
        {
            return expression.Value;
        }

        private object GetMemberValue(MemberExpression expression)
        {
            if (expression.Member.DeclaringType == typeof(TSource) && expression.Expression.NodeType == ExpressionType.Parameter)
            {
                return new BinaryUpdateMember
                {
                    MemberName = expression.Member.Name
                };
            }
            else
            {
                return expression.GetValue();
            }
        }

        private object GetMethodCallValue(MethodCallExpression expression)
        {
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }

        private BinaryUpdateValue GetBinaryValue(BinaryExpression expression)
        {
            var left = GetValue(expression.Left);
            var right = GetValue(expression.Right);
            var nodeType = expression.NodeType;

            return new BinaryUpdateValue(left, right, nodeType);
        }

        private object GetConditionalValue(ConditionalExpression conditionalExpression)
        {
            var ifResult = (bool)GetValue(conditionalExpression.Test);
            return ifResult ? GetValue(conditionalExpression.IfTrue) : GetValue(conditionalExpression.IfFalse);
        }

        private object GetUnaryValue(UnaryExpression expression)
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

        #endregion
    }
}
