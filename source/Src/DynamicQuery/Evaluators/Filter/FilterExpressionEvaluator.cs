namespace DotFramework.DynamicQuery
{
    public abstract class FilterExpressionEvaluator : AbstractFilterEvaluator<FilterExpression>
    {
        public FilterExpressionEvaluator(FilterExpression evaluationObject) : base(evaluationObject)
        {
        }
    }
}
