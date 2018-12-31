namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerBinaryUpdateMemberEvaluator : BinaryUpdateMemberEvaluator
    {
        public SqlServerBinaryUpdateMemberEvaluator(BinaryUpdateMember evaluationObject) : base(evaluationObject)
        {
        }

        protected override string GetValueString()
        {
            return $"[{EvaluationObject.MemberName}]";
        }
    }
}
