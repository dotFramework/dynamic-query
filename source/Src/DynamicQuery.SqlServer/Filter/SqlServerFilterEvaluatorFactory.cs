namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerFilterEvaluatorFactory : FilterEvaluatorFactory<SqlServerFilterEvaluatorFactory>
    {
        protected override FilterExpressionEvaluator CreateFilterExpressionEvaluator(FilterExpression filterExpression)
        {
            return new SqlServerFilterExpressionEvaluator(filterExpression);
        }

        protected override InFilterEvaluator CreateInFilterEvaluator(InFilter inFilter)
        {
            return new SqlServerInFilterEvaluator(inFilter);
        }

        protected override AndFilterEvaluator CreateAndFilterEvaluator(AndFilter andFilter)
        {
            return new SqlServerAndFilterEvaluator(andFilter);
        }

        protected override OrFilterEvaluator CreateOrFilterEvaluator(OrFilter orFilter)
        {
            return new SqlServerOrFilterEvaluator(orFilter);
        }
    }
}
