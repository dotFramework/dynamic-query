using DotFramework.DynamicQuery.Metadata;
using System;
using System.Linq.Expressions;

namespace DotFramework.DynamicQuery
{
    public class SelectQueryBuilderBase
    {
        #region Constructors

        public SelectQueryBuilderBase()
        {
            Query = new SelectQuery();
        }

        public SelectQueryBuilderBase(Type sourceType)
        {
            Query = new SelectQuery(sourceType);
        }

        public SelectQueryBuilderBase(SelectQuery query)
        {
            SetQuery(query);
        }

        #endregion

        #region Properties

        public SelectQuery Query { get; private set; }

        #endregion

        #region Protected Properties

        protected FilterFactory FilterFactory => FilterFactory.Instance;

        #endregion

        #region Public Methods

        public void SetQuery(SelectQuery query)
        {
            Query = query;
        }

        #endregion

        #region Protected Methods

        protected void SetAndFilter(AbstractFilter filter)
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

        protected void SetOrFilter(AbstractFilter filter)
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

        protected void SetFilter(AbstractFilter filter)
        {
            Query.Filter = filter;
        }

        #endregion
    }

    public class SelectQueryBuilder : SelectQueryBuilderBase
    {
        #region Constructors

        public SelectQueryBuilder() : base()
        {

        }

        public SelectQueryBuilder(Type sourceType) : base(sourceType)
        {

        }

        public SelectQueryBuilder(SelectQuery query) : base(query)
        {

        }

        #endregion

        #region Public Methods

        public SelectQueryBuilder From(string tableName)
        {
            return From(null, tableName);
        }

        public SelectQueryBuilder From(string schemaName, string tableName)
        {
            Query.ObjectType = new GeneralObject(schemaName, tableName);
            return this;
        }

        public SelectQueryBuilder<TSource> From<TSource>()
        {
            return InitializeNextLevelBuilder<TSource>();
        }

        #endregion

        #region Private Methods

        private SelectQueryBuilder<T1> InitializeNextLevelBuilder<T1>()
        {
            Query.ObjectType = MetadataHelper.GetGeneralObject(typeof(T1));

            var builder = CreateNextLevelBuilder<T1>();
            builder.SetQuery(Query);

            return builder;
        }

        #endregion

        #region Protected Methods

        protected SelectQueryBuilder<T1> CreateNextLevelBuilder<T1>()
        {
            return new SelectQueryBuilder<T1>();
        }

        #endregion

        #region Static Methods

        public static SelectQueryBuilder Initialize()
        {
            return new SelectQueryBuilder();
        }

        public static SelectQueryBuilder Initialize(SelectQuery query)
        {
            var queryBuilder = Initialize();
            queryBuilder.SetQuery(query);

            return queryBuilder;
        }

        #endregion
    }

    public class SelectQueryBuilder<T1> : SelectQueryBuilderBase
    {
        #region Constructors

        public SelectQueryBuilder() : base()
        {

        }

        public SelectQueryBuilder(Type sourceType) : base(sourceType)
        {

        }

        public SelectQueryBuilder(SelectQuery query) : base(query)
        {

        }

        #endregion

        #region Private Properties

        private SelectFieldFactory SelectFieldFactory => SelectFieldFactory.Instance;
        private JoinFactory JoinFactory => JoinFactory.Instance;
        private OrderByFactory OrderByFactory => OrderByFactory.Instance;

        #endregion

        #region Public Methods

        public SelectQueryBuilder<T1> Select(Expression<Func<T1, Object>> expression)
        {
            var selectFields = SelectFieldFactory.Create(expression);
            Query.SelectFieldList.AddRange(selectFields);

            return this;
        }

        public SelectQueryBuilder<T1> Distinct()
        {
            Query.Distinct = true;
            return this;
        }

        public SelectQueryBuilder<T1> Count()
        {
            return Count("Count");
        }

        public SelectQueryBuilder<T1> Count(string alias)
        {
            SelectField selectField = SelectFieldFactory.CreateCount(alias);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1> Sum(Expression<Func<T1, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Sum);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1> Min(Expression<Func<T1, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Min);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1> Max(Expression<Func<T1, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Max);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1> Average(Expression<Func<T1, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Average);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, TInner> InnerJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> InnerJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> InnerJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> LeftJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> LeftJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> LeftJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> RightJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> RightJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> RightJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T1, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1> Where(Expression<Func<T1, Boolean>> expression)
        {
            SetAndFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1> OrWhere(Expression<Func<T1, Boolean>> expression)
        {
            SetOrFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1> OrderBy(Expression<Func<T1, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Ascending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1> OrderByDescending(Expression<Func<T1, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Descending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1> Paginate(UInt64 pageNumber, UInt64 recordCount)
        {
            if (pageNumber == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "page number should be greater than zero.");
            }

            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.PageNumber = pageNumber;
            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1> Take(UInt64 recordCount)
        {
            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1> Skip(UInt64 offset)
        {
            Query.Offset = offset;

            return this;
        }

        #endregion

        #region Private Methods

        private SelectQueryBuilder<T1, T2> InitializeNextLevelBuilder<T2>()
        {
            var builder = CreateNextLevelBuilder<T2>();
            builder.SetQuery(Query);

            return builder;
        }

        #endregion

        #region Protected Methods

        protected SelectQueryBuilder<T1, T2> CreateNextLevelBuilder<T2>()
        {
            return new SelectQueryBuilder<T1, T2>();
        }

        #endregion

        #region Static Methods

        public static SelectQueryBuilder<T1> Initialize()
        {
            return new SelectQueryBuilder<T1>(typeof(T1));
        }

        public static SelectQueryBuilder<T1> Initialize(SelectQuery query)
        {
            var queryBuilder = Initialize();

            if (queryBuilder.Query.ObjectType != null && query.ObjectType == null)
            {
                query.ObjectType = queryBuilder.Query.ObjectType;
            }

            queryBuilder.SetQuery(query);
            return queryBuilder;
        }

        #endregion
    }

    public class SelectQueryBuilder<T1, T2> : SelectQueryBuilderBase
    {
        #region Constructors

        public SelectQueryBuilder() : base()
        {

        }

        public SelectQueryBuilder(Type sourceType) : base(sourceType)
        {

        }

        public SelectQueryBuilder(SelectQuery query) : base(query)
        {

        }

        #endregion

        #region Private Properties

        private SelectFieldFactory SelectFieldFactory => SelectFieldFactory.Instance;
        private JoinFactory JoinFactory => JoinFactory.Instance;
        private OrderByFactory OrderByFactory => OrderByFactory.Instance;

        #endregion

        #region Public Methods

        public SelectQueryBuilder<T1, T2> Select(Expression<Func<T1, T2, Object>> expression)
        {
            var selectFields = SelectFieldFactory.Create(expression);
            Query.SelectFieldList.AddRange(selectFields);

            return this;
        }

        public SelectQueryBuilder<T1, T2> Distinct()
        {
            Query.Distinct = true;
            return this;
        }

        public SelectQueryBuilder<T1, T2> Count()
        {
            return Count("Count");
        }

        public SelectQueryBuilder<T1, T2> Count(string alias)
        {
            SelectField selectField = SelectFieldFactory.CreateCount(alias);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2> Sum(Expression<Func<T1, T2, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Sum);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2> Min(Expression<Func<T1, T2, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Min);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2> Max(Expression<Func<T1, T2, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Max);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2> Average(Expression<Func<T1, T2, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Average);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, TInner> InnerJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> InnerJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> InnerJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> LeftJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> LeftJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> LeftJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> RightJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> RightJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> RightJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T2, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2> Where(Expression<Func<T1, T2, Boolean>> expression)
        {
            SetAndFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1, T2> OrWhere(Expression<Func<T1, T2, Boolean>> expression)
        {
            SetOrFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1, T2> OrderBy(Expression<Func<T1, T2, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Ascending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1, T2> OrderByDescending(Expression<Func<T1, T2, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Descending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1, T2> Paginate(UInt64 pageNumber, UInt64 recordCount)
        {
            if (pageNumber == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "page number should be greater than zero.");
            }

            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.PageNumber = pageNumber;
            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1, T2> Take(UInt64 recordCount)
        {
            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1, T2> Skip(UInt64 offset)
        {
            Query.Offset = offset;

            return this;
        }

        #endregion

        #region Private Methods

        private SelectQueryBuilder<T1, T2, T3> InitializeNextLevelBuilder<T3>()
        {
            var builder = CreateNextLevelBuilder<T3>();
            builder.SetQuery(Query);

            return builder;
        }

        #endregion

        #region Protected Methods

        protected SelectQueryBuilder<T1, T2, T3> CreateNextLevelBuilder<T3>()
        {
            return new SelectQueryBuilder<T1, T2, T3>();
        }

        #endregion

        #region Static Methods

        public static SelectQueryBuilder<T1, T2> Initialize()
        {
            return new SelectQueryBuilder<T1, T2>(typeof(T1));
        }

        public static SelectQueryBuilder<T1, T2> Initialize(SelectQuery query)
        {
            var queryBuilder = Initialize();

            if (queryBuilder.Query.ObjectType != null && query.ObjectType == null)
            {
                query.ObjectType = queryBuilder.Query.ObjectType;
            }

            queryBuilder.SetQuery(query);
            return queryBuilder;
        }

        #endregion
    }

    public class SelectQueryBuilder<T1, T2, T3> : SelectQueryBuilderBase
    {
        #region Constructors

        public SelectQueryBuilder() : base()
        {

        }

        public SelectQueryBuilder(Type sourceType) : base(sourceType)
        {

        }

        public SelectQueryBuilder(SelectQuery query) : base(query)
        {

        }

        #endregion

        #region Private Properties

        private SelectFieldFactory SelectFieldFactory => SelectFieldFactory.Instance;
        private JoinFactory JoinFactory => JoinFactory.Instance;
        private OrderByFactory OrderByFactory => OrderByFactory.Instance;

        #endregion

        #region Public Methods

        public SelectQueryBuilder<T1, T2, T3> Select(Expression<Func<T1, T2, T3, Object>> expression)
        {
            var selectFields = SelectFieldFactory.Create(expression);
            Query.SelectFieldList.AddRange(selectFields);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Distinct()
        {
            Query.Distinct = true;
            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Count()
        {
            return Count("Count");
        }

        public SelectQueryBuilder<T1, T2, T3> Count(string alias)
        {
            SelectField selectField = SelectFieldFactory.CreateCount(alias);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Sum(Expression<Func<T1, T2, T3, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Sum);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Min(Expression<Func<T1, T2, T3, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Min);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Max(Expression<Func<T1, T2, T3, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Max);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Average(Expression<Func<T1, T2, T3, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Average);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> InnerJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> InnerJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> InnerJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> LeftJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> LeftJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> LeftJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> RightJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> RightJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> RightJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T3, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3> Where(Expression<Func<T1, T2, T3, Boolean>> expression)
        {
            SetAndFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> OrWhere(Expression<Func<T1, T2, T3, Boolean>> expression)
        {
            SetOrFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Ascending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> OrderByDescending(Expression<Func<T1, T2, T3, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Descending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Paginate(UInt64 pageNumber, UInt64 recordCount)
        {
            if (pageNumber == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "page number should be greater than zero.");
            }

            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.PageNumber = pageNumber;
            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Take(UInt64 recordCount)
        {
            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3> Skip(UInt64 offset)
        {
            Query.Offset = offset;

            return this;
        }

        #endregion

        #region Private Methods

        private SelectQueryBuilder<T1, T2, T3, T4> InitializeNextLevelBuilder<T4>()
        {
            var builder = CreateNextLevelBuilder<T4>();
            builder.SetQuery(Query);

            return builder;
        }

        #endregion

        #region Protected Methods

        protected SelectQueryBuilder<T1, T2, T3, T4> CreateNextLevelBuilder<T4>()
        {
            return new SelectQueryBuilder<T1, T2, T3, T4>();
        }

        #endregion

        #region Static Methods

        public static SelectQueryBuilder<T1, T2, T3> Initialize()
        {
            return new SelectQueryBuilder<T1, T2, T3>(typeof(T1));
        }

        public static SelectQueryBuilder<T1, T2, T3> Initialize(SelectQuery query)
        {
            var queryBuilder = Initialize();

            if (queryBuilder.Query.ObjectType != null && query.ObjectType == null)
            {
                query.ObjectType = queryBuilder.Query.ObjectType;
            }

            queryBuilder.SetQuery(query);
            return queryBuilder;
        }

        #endregion
    }

    public class SelectQueryBuilder<T1, T2, T3, T4> : SelectQueryBuilderBase
    {
        #region Constructors

        public SelectQueryBuilder() : base()
        {

        }

        public SelectQueryBuilder(Type sourceType) : base(sourceType)
        {

        }

        public SelectQueryBuilder(SelectQuery query) : base(query)
        {

        }

        #endregion

        #region Private Properties

        private SelectFieldFactory SelectFieldFactory => SelectFieldFactory.Instance;
        private JoinFactory JoinFactory => JoinFactory.Instance;
        private OrderByFactory OrderByFactory => OrderByFactory.Instance;

        #endregion

        #region Public Methods

        public SelectQueryBuilder<T1, T2, T3, T4> Select(Expression<Func<T1, T2, T3, T4, Object>> expression)
        {
            var selectFields = SelectFieldFactory.Create(expression);
            Query.SelectFieldList.AddRange(selectFields);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Distinct()
        {
            Query.Distinct = true;
            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Count()
        {
            return Count("Count");
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Count(string alias)
        {
            SelectField selectField = SelectFieldFactory.CreateCount(alias);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Sum(Expression<Func<T1, T2, T3, T4, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Sum);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Min(Expression<Func<T1, T2, T3, T4, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Min);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Max(Expression<Func<T1, T2, T3, T4, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Max);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Average(Expression<Func<T1, T2, T3, T4, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Average);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> InnerJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> InnerJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> InnerJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.InnerJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> LeftJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> LeftJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> LeftJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.LeftJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> RightJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> RightJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> RightJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.RightJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin, outerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4, TInner> FullOuterJoin<TInner, TKey>(Expression<Func<T4, TKey>> outerExpression, Expression<Func<TInner, TKey>> innerExpression, string outerTableAlias, string innerTableAlias)
        {
            var join = JoinFactory.Join(outerExpression, innerExpression, JoinType.FullOuterJoin, outerTableAlias, innerTableAlias);
            Query.JoinList.Add(join);

            return InitializeNextLevelBuilder<TInner>();
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, Boolean>> expression)
        {
            SetAndFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> OrWhere(Expression<Func<T1, T2, T3, T4, Boolean>> expression)
        {
            SetOrFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> OrderBy(Expression<Func<T1, T2, T3, T4, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Ascending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> OrderByDescending(Expression<Func<T1, T2, T3, T4, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Descending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Paginate(UInt64 pageNumber, UInt64 recordCount)
        {
            if (pageNumber == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "page number should be greater than zero.");
            }

            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.PageNumber = pageNumber;
            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Take(UInt64 recordCount)
        {
            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4> Skip(UInt64 offset)
        {
            Query.Offset = offset;

            return this;
        }

        #endregion

        #region Private Methods

        private SelectQueryBuilder<T1, T2, T3, T4, T5> InitializeNextLevelBuilder<T5>()
        {
            var builder = CreateNextLevelBuilder<T5>();
            builder.SetQuery(Query);

            return builder;
        }

        #endregion

        #region Protected Methods

        protected SelectQueryBuilder<T1, T2, T3, T4, T5> CreateNextLevelBuilder<T5>()
        {
            return new SelectQueryBuilder<T1, T2, T3, T4, T5>();
        }

        #endregion

        #region Static Methods

        public static SelectQueryBuilder<T1, T2, T3, T4> Initialize()
        {
            return new SelectQueryBuilder<T1, T2, T3, T4>(typeof(T1));
        }

        public static SelectQueryBuilder<T1, T2, T3, T4> Initialize(SelectQuery query)
        {
            var queryBuilder = Initialize();

            if (queryBuilder.Query.ObjectType != null && query.ObjectType == null)
            {
                query.ObjectType = queryBuilder.Query.ObjectType;
            }

            queryBuilder.SetQuery(query);
            return queryBuilder;
        }

        #endregion
    }

    public class SelectQueryBuilder<T1, T2, T3, T4, T5> : SelectQueryBuilderBase
    {
        #region Constructors

        public SelectQueryBuilder() : base()
        {

        }

        public SelectQueryBuilder(Type sourceType) : base(sourceType)
        {

        }

        public SelectQueryBuilder(SelectQuery query) : base(query)
        {

        }

        #endregion

        #region Private Properties

        private SelectFieldFactory SelectFieldFactory => SelectFieldFactory.Instance;
        private JoinFactory JoinFactory => JoinFactory.Instance;
        private OrderByFactory OrderByFactory => OrderByFactory.Instance;

        #endregion

        #region Public Methods

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Select(Expression<Func<T1, T2, T3, T4, T5, Object>> expression)
        {
            var selectFields = SelectFieldFactory.Create(expression);
            Query.SelectFieldList.AddRange(selectFields);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Distinct()
        {
            Query.Distinct = true;
            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Count()
        {
            return Count("Count");
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Count(string alias)
        {
            SelectField selectField = SelectFieldFactory.CreateCount(alias);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Sum(Expression<Func<T1, T2, T3, T4, T5, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Sum);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Min(Expression<Func<T1, T2, T3, T4, T5, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Min);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Max(Expression<Func<T1, T2, T3, T4, T5, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Max);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Average(Expression<Func<T1, T2, T3, T4, T5, Object>> expression)
        {
            SelectField selectField = SelectFieldFactory.CreateAggregate(expression, AggregateMethodType.Average);
            Query.SelectFieldList.Add(selectField);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, Boolean>> expression)
        {
            SetFilter(FilterFactory.CreateFilter(expression));
            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> OrderBy(Expression<Func<T1, T2, T3, T4, T5, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Ascending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> OrderByDescending(Expression<Func<T1, T2, T3, T4, T5, Object>> expression)
        {
            var orderBy = OrderByFactory.OrderBy(expression, OrderByType.Descending);
            Query.OrderByList.Add(orderBy);

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Paginate(UInt64 pageNumber, UInt64 recordCount)
        {
            if (pageNumber == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "page number should be greater than zero.");
            }

            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.PageNumber = pageNumber;
            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Take(UInt64 recordCount)
        {
            if (recordCount == 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "record count should be greater than zero.");
            }

            Query.RecordCount = recordCount;

            return this;
        }

        public SelectQueryBuilder<T1, T2, T3, T4, T5> Skip(UInt64 offset)
        {
            Query.Offset = offset;

            return this;
        }

        #endregion

        #region Static Methods

        public static SelectQueryBuilder<T1, T2, T3, T4, T5> Initialize()
        {
            return new SelectQueryBuilder<T1, T2, T3, T4, T5>(typeof(T1));
        }

        public static SelectQueryBuilder<T1, T2, T3, T4, T5> Initialize(SelectQuery query)
        {
            var queryBuilder = Initialize();

            if (queryBuilder.Query.ObjectType != null && query.ObjectType == null)
            {
                query.ObjectType = queryBuilder.Query.ObjectType;
            }

            queryBuilder.SetQuery(query);
            return queryBuilder;
        }

        #endregion
    }
}
