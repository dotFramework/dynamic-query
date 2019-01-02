using DotFramework.Core;
using System.Linq.Expressions;

namespace DotFramework.DynamicQuery
{
    public class OrderByFactory : SingletonProvider<OrderByFactory>
    {
        public OrderBy OrderBy(LambdaExpression expression, OrderByType orderByType)
        {
            var propInfo = expression.GetPropertyInfo();
            string tableName = propInfo.MemberType.Name;
            string fieldName = propInfo.Property.Name;

            OrderBy orderBy = new OrderBy
            {
                TableName = tableName,
                FieldName = fieldName,
                OrderByType = orderByType
            };

            return orderBy;
        }
    }
}
