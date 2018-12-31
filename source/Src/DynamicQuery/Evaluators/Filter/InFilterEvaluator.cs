namespace DotFramework.DynamicQuery
{
    public abstract class InFilterEvaluator : AbstractFilterEvaluator<InFilter>
    {
        public InFilterEvaluator(InFilter evaluationObject) : base(evaluationObject)
        {
        }
    }
}
