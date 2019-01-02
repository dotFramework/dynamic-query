using DotFramework.Core;
using System;

namespace DotFramework.DynamicQuery
{
    public interface IFilterEvaluator
    {
        string ToString();
    }

    public abstract class AbstractFilterEvaluator<TFilter> : AbstractEvaluator<TFilter>, IFilterEvaluator
        where TFilter : AbstractFilter
    {
        public AbstractFilterEvaluator(TFilter evaluationObject) : base(evaluationObject)
        {
        }

        public new abstract string ToString();
    }

    public abstract class FilterEvaluatorFactory<TFilterEvaluator> : SingletonProvider<TFilterEvaluator>
        where TFilterEvaluator : class
    {
        public IFilterEvaluator GetFilterEvaluator(AbstractFilter filter)
        {
            if (filter is FilterExpression filterExpression)
            {
                return CreateFilterExpressionEvaluator(filterExpression);
            }
            else if (filter is InFilter inFilter)
            {
                return CreateInFilterEvaluator(inFilter);
            }
            else if (filter is AndFilter andFilter)
            {
                return CreateAndFilterEvaluator(andFilter);
            }
            else if (filter is OrFilter orFilter)
            {
                return CreateOrFilterEvaluator(orFilter);
            }
            else
            {
                throw new NotSupportedException("Not Supported Filter");
            }
        }

        protected abstract FilterExpressionEvaluator CreateFilterExpressionEvaluator(FilterExpression filterExpression);

        protected abstract InFilterEvaluator CreateInFilterEvaluator(InFilter inFilter);

        protected abstract AndFilterEvaluator CreateAndFilterEvaluator(AndFilter andFilter);

        protected abstract OrFilterEvaluator CreateOrFilterEvaluator(OrFilter orFilter);
    }
}
