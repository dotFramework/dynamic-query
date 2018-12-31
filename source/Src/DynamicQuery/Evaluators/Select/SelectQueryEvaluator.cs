namespace DotFramework.DynamicQuery
{
    public abstract class SelectQueryEvaluator : AbstractQueryEvaluator<SelectQuery>
    {
        public SelectQueryEvaluator(SelectQuery evaluationObject) : base(evaluationObject)
        {
        }

        protected abstract string TopRecordsFormat { get; }
        protected abstract string PaginationFormat { get; }
    }
}
