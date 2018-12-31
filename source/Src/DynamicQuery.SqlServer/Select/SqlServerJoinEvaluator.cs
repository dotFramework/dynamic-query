using System;
using System.Text;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerJoinEvaluator : JoinEvaluator
    {
        public SqlServerJoinEvaluator()
        {
        }

        public SqlServerJoinEvaluator(Join evaluationObject) : base(evaluationObject)
        {
        }

        public override string ToString()
        {
            if (String.IsNullOrWhiteSpace(EvaluationObject.OuterTable))
            {
                throw new ArgumentNullException("OuterTable");
            }

            if (String.IsNullOrWhiteSpace(EvaluationObject.OuterKey))
            {
                throw new ArgumentNullException("OuterKey");
            }

            if (String.IsNullOrWhiteSpace(EvaluationObject.InnerTable))
            {
                throw new ArgumentNullException("InnerTable");
            }

            if (String.IsNullOrWhiteSpace(EvaluationObject.InnerKey))
            {
                throw new ArgumentNullException("InnerKey");
            }

            string joinType = String.Empty;

            switch (EvaluationObject.JoinType)
            {
                case JoinType.InnerJoin:
                    joinType = "INNER JOIN";
                    break;
                case JoinType.LeftJoin:
                    joinType = "LEFT OUTER JOIN";
                    break;
                case JoinType.RightJoin:
                    joinType = "RIGHT OUTER JOIN";
                    break;
                case JoinType.FullOuterJoin:
                    joinType = "FULL OUTER JOIN";
                    break;
                default:
                    break;
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" {0} [{1}]", joinType, EvaluationObject.InnerTable);

            if (!String.IsNullOrWhiteSpace(EvaluationObject.InnerTableAlias))
            {
                builder.AppendFormat(" AS [{0}]", EvaluationObject.InnerTableAlias);
            }

            builder.AppendFormat(" ON [{0}].[{1}] = [{2}].[{3}]", String.IsNullOrWhiteSpace(EvaluationObject.OuterTableAlias) ? EvaluationObject.OuterTable : EvaluationObject.OuterTableAlias,
                                                                  EvaluationObject.OuterKey,
                                                                  String.IsNullOrWhiteSpace(EvaluationObject.InnerTableAlias) ? EvaluationObject.InnerTable : EvaluationObject.InnerTableAlias,
                                                                  EvaluationObject.InnerKey);

            return builder.ToString();
        }
    }

    public class SqlServerJoinListEvaluator : JoinListEvaluator<SqlServerJoinEvaluator>
    {
        public SqlServerJoinListEvaluator(JoinList evaluationObject) : base(evaluationObject)
        {
        }
    }
}
