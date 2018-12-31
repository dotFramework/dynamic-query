using System;

namespace DotFramework.DynamicQuery
{
    public abstract class AbstractQueryEvaluator<TQuery> : AbstractEvaluator<TQuery>
        where TQuery : AbstractQuery
    {
        #region Constructors

        public AbstractQueryEvaluator(TQuery evaluationObject) : base(evaluationObject)
        {
        }

        #endregion

        #region Abstract Methods

        protected abstract string GenerateQuery();

        #endregion

        #region Public Methods

        public new string ToString()
        {
            if (EvaluationObject.ValidateQuery())
            {
                return GenerateQuery();
            }
            else
            {
                throw new Exception("Query is not validated.");
            }
        }

        #endregion
    }
}
