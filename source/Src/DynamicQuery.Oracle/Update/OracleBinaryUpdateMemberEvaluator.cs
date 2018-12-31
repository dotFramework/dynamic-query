namespace DotFramework.DynamicQuery.Oracle
{
    public class OracleBinaryUpdateMemberEvaluator : BinaryUpdateMemberEvaluator
    {
        public OracleBinaryUpdateMemberEvaluator(BinaryUpdateMember evaluationObject) : base(evaluationObject)
        {
        }

        protected override string GetValueString()
        {
            return $"\"{EvaluationObject.MemberName}\"";
        }
    }
}
