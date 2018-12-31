using DotFramework.DynamicQuery.Metadata;
using System;
using System.Linq.Expressions;

namespace DotFramework.DynamicQuery
{
    public abstract class AbstractQueryBuilder<TSource, TQueryBuilder, TQuery>
        where TQueryBuilder : AbstractQueryBuilder<TSource, TQueryBuilder, TQuery>, new()
        where TQuery : AbstractQuery, new()
    {
        #region Constructors

        public AbstractQueryBuilder()
        {
            Query = new TQuery();
        }

        public AbstractQueryBuilder(Type sourceType) : this()
        {
            Query = new TQuery
            {
                ObjectType = MetadataHelper.GetGeneralObject(sourceType)
            };
        }

        public AbstractQueryBuilder(TQuery query) : this()
        {
            SetQuery(query);
        }

        #endregion

        #region Properties

        public TQuery Query { get; private set; }

        #endregion

        #region Protected Properties

        protected FilterFactory FilterFactory => FilterFactory.Instance;

        #endregion

        #region Public Methods

        public static TQueryBuilder Initialize()
        {
            var queryBuilder = new TQueryBuilder();
            queryBuilder.Query.ObjectType = MetadataHelper.GetGeneralObject(typeof(TSource));

            return queryBuilder;
        }

        public static TQueryBuilder Initialize(TQuery query)
        {
            var queryBuilder = Initialize();

            if (queryBuilder.Query.ObjectType != null && query.ObjectType == null)
            {
                query.ObjectType = queryBuilder.Query.ObjectType;
            }

            queryBuilder.SetQuery(query);
            return queryBuilder;
        }

        public TQueryBuilder Where(Expression<Func<TSource, Boolean>> expression)
        {
            SetAndFilter(FilterFactory.CreateFilter(expression));
            return (TQueryBuilder)this;
        }

        public TQueryBuilder OrWhere(Expression<Func<TSource, Boolean>> expression)
        {
            SetOrFilter(FilterFactory.CreateFilter(expression));
            return (TQueryBuilder)this;
        }

        #endregion

        #region Public Methods

        internal void SetQuery(TQuery query)
        {
            Query = query;
        }

        #endregion

        #region Private Methods

        private void SetAndFilter(AbstractFilter filter)
        {
            if (Query.Filter != null)
            {
                Query.Filter = FilterFactory.CreateAndFilter(Query.Filter, filter);
            }
            else
            {
                Query.Filter = filter;
            }
        }

        private void SetOrFilter(AbstractFilter filter)
        {
            if (Query.Filter != null)
            {
                Query.Filter = FilterFactory.CreateOrFilter(Query.Filter, filter);
            }
            else
            {
                SetFilter(filter);
            }
        }

        private void SetFilter(AbstractFilter filter)
        {
            Query.Filter = filter;
        }

        #endregion
    }
}
