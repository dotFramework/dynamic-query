using System;

namespace DotFramework.DynamicQuery
{
    public class DeleteQueryBuilder<TSource> : AbstractQueryBuilder<TSource, DeleteQueryBuilder<TSource>, DeleteQuery>
    {
        #region Constructors

        public DeleteQueryBuilder() : base()
        {
            
        }

        public DeleteQueryBuilder(Type sourceType) : base(sourceType)
        {

        }

        public DeleteQueryBuilder(DeleteQuery query) : base(query)
        {
            
        }

        #endregion
    }
}
