namespace DotFramework.DynamicQuery.Oracle
{
    public class OracleFilterEvaluatorFactory : FilterEvaluatorFactory<OracleFilterEvaluatorFactory>
    {
        protected override FilterExpressionEvaluator CreateFilterExpressionEvaluator(FilterExpression filterExpression)
        {
            return new OracleFilterExpressionEvaluator(filterExpression);
        }

        protected override InFilterEvaluator CreateInFilterEvaluator(InFilter inFilter)
        {
            return new OracleInFilterEvaluator(inFilter);
        }

        protected override AndFilterEvaluator CreateAndFilterEvaluator(AndFilter andFilter)
        {
            return new OracleAndFilterEvaluator(andFilter);
        }

        protected override OrFilterEvaluator CreateOrFilterEvaluator(OrFilter orFilter)
        {
            return new OracleOrFilterEvaluator(orFilter);
        }
    }
}
