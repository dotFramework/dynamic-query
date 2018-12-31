using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public abstract class UpdateQueryEvaluator : AbstractQueryEvaluator<UpdateQuery>
    {
        public UpdateQueryEvaluator(UpdateQuery evaluationObject) : base(evaluationObject)
        {
        }
    }
}
