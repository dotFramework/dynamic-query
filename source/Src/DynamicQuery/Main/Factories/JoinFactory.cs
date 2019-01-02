using DotFramework.Core;
using System;
using System.Linq.Expressions;

namespace DotFramework.DynamicQuery
{
    public class JoinFactory : SingletonProvider<JoinFactory>
    {
        public Join Join<TOuter, TInner, TKey>(Expression<Func<TOuter, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, JoinType joinType, string outerTableAlias)
        {
            var tableJoin = Join(outerExpression, innerExpression, joinType);
            tableJoin.OuterTableAlias = outerTableAlias;

            return tableJoin;
        }

        public Join Join<TOuter, TInner, TKey>(Expression<Func<TOuter, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, JoinType joinType, string outerTableAlias, string innerTableAlias)
        {
            var tableJoin = Join(outerExpression, innerExpression, joinType, outerTableAlias);
            tableJoin.InnerTableAlias = innerTableAlias;

            return tableJoin;
        }

        public Join Join<TOuter, TInner, TKey>(Expression<Func<TOuter, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, JoinType joinType)
        {
            var outerPropInfo = outerExpression.GetPropertyInfo();
            string outerTable = outerPropInfo.MemberType.Name;
            string outerKey = outerPropInfo.Property.Name;

            var innerPropInfo = innerExpression.GetPropertyInfo();
            string innerTable = innerPropInfo.MemberType.Name;
            string innerKey = innerPropInfo.Property.Name;

            return new Join
            {
                OuterTable = outerTable,
                OuterKey = outerKey,
                InnerTable = innerTable,
                InnerKey = innerKey,
                JoinType = joinType
            };
        }
    }
}
