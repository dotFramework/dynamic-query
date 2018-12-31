namespace DotFramework.DynamicQuery
{
    public abstract class BinaryUpdateMemberEvaluator : AbstractEvaluator<BinaryUpdateMember>
    {
        public BinaryUpdateMemberEvaluator(BinaryUpdateMember evaluationObject) : base(evaluationObject)
        {
        }

        public new string ToString()
        {
            return GetValueString();
        }

        protected abstract string GetValueString();
    }
}
