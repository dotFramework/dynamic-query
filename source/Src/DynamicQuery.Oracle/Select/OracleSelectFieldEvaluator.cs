using System;

namespace DotFramework.DynamicQuery.Oracle
{
    public class OracleSelectFieldEvaluator : SelectFieldEvaluator
    {
        public OracleSelectFieldEvaluator()
        {
        }

        public OracleSelectFieldEvaluator(SelectField evaluationObject) : base(evaluationObject)
        {
        }

        protected override string FieldFormat => "\"{0}\"";

        protected override string AliasFormat => "{0} AS {1}";

        protected override string AggregateFieldFormat => "{0}({1})";

        protected override string ConvertAggregateMethodTypeToString(AggregateMethodType aggregateMethodType)
        {
            switch (aggregateMethodType)
            {
                case AggregateMethodType.Count:
                    return "COUNT";
                case AggregateMethodType.Sum:
                    return "SUM";
                case AggregateMethodType.Min:
                    return "MIN";
                case AggregateMethodType.Max:
                    return "MAX";
                case AggregateMethodType.Average:
                    return "AVG";
                default:
                    throw new NotSupportedException("Not Supported Aggregate Method");
            }
        }
    }

    public class OracleSelectFieldListEvaluator : SelectFieldListEvaluator<OracleSelectFieldEvaluator>
    {
        public OracleSelectFieldListEvaluator(SelectFieldList evaluationObject) : base(evaluationObject)
        {
        }
    }
}
