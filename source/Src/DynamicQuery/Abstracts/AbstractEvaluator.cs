namespace DotFramework.DynamicQuery
{
    public abstract class AbstractEvaluator<TEvaluationObject>
    {
        public AbstractEvaluator()
        {

        }

        public AbstractEvaluator(TEvaluationObject evaluationObject)
        {
            EvaluationObject = evaluationObject;
        }

        public TEvaluationObject EvaluationObject { get; set; }
    }
}
