using DotFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DotFramework.DynamicQuery
{
    public class SelectFieldFactory : SingletonProvider<SelectFieldFactory>
    {
        public SelectField CreateCount(string alias)
        {
            SelectField selectField = new SelectField
            {
                FieldName = "*",
                AggregateMethodType = AggregateMethodType.Count,
                Alias = alias
            };

            return selectField;
        }

        public IList<SelectField> Create(LambdaExpression expression)
        {
            IList<SelectField> selectFields = new List<SelectField>();

            if (expression.Body is UnaryExpression || expression.Body is MemberExpression)
            {
                SelectField selectField = Create(expression.GetPropertyInfo());
                selectFields.Add(selectField);
            }
            else if (expression.Body is NewExpression)
            {
                var body = expression.Body as NewExpression;

                for (int i = 0; i < body.Arguments.Count; i++)
                {
                    MemberExpression argument = body.Arguments[i] as MemberExpression;
                    MemberInfo member = body.Members[i];

                    SelectField selectField = Create(argument, member);
                    selectFields.Add(selectField);
                }
            }
            else
            {
                throw new NotSupportedException("Unsupported Expression");
            }

            return selectFields;
        }

        public SelectField CreateAggregate(LambdaExpression expression, AggregateMethodType aggregateMethodType)
        {
            SelectField selectField = default(SelectField);

            var type = expression.Body.GetType();

            if (expression.Body is UnaryExpression || expression.Body is MemberExpression)
            {
                selectField = Create(expression.GetPropertyInfo());
                selectField.AggregateMethodType = aggregateMethodType;
            }
            else
            {
                throw new NotSupportedException("Unsupported Expression");
            }

            return selectField;
        }

        private SelectField Create(LambdaPropertyInfo propInfo)
        {
            string tableName = propInfo.MemberType.Name;
            string fieldName = propInfo.Property.Name;

            SelectField selectField = new SelectField
            {
                TableName = tableName,
                FieldName = fieldName
            };

            return selectField;
        }

        private SelectField Create(MemberExpression argument, MemberInfo member)
        {
            PropertyInfo propInfo = argument.Member as PropertyInfo;
            Type memberType = argument.Expression.Type;

            string tableName = memberType.Name;
            string fieldName = propInfo.Name;
            string alias = member.Name;

            SelectField selectField = new SelectField
            {
                TableName = tableName,
                FieldName = fieldName
            };

            if (fieldName != alias)
            {
                selectField.Alias = alias;
            }

            return selectField;
        }
    }
}
