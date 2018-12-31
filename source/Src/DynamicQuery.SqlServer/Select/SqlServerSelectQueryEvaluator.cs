using DotFramework.DynamicQuery.Metadata;
using System;
using System.Text;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerSelectQueryEvaluator : SelectQueryEvaluator
    {
        public SqlServerSelectQueryEvaluator(SelectQuery evaluationObject) : base(evaluationObject)
        {
        }

        protected override string TopRecordsFormat => "TOP {0} ";

        protected override string PaginationFormat => "OFFSET {0} ROWS\nFETCH NEXT {1} ROWS ONLY";

        protected override string GenerateQuery()
        {
            EvaluationObject.ValidateQuery();

            StringBuilder builder = new StringBuilder();

            builder.Append("SELECT ");

            if (EvaluationObject.Distinct)
            {
                builder.Append("DISTINCT ");
            }

            if (EvaluationObject.IsSelectWithTop())
            {
                builder.AppendFormat(TopRecordsFormat, EvaluationObject.RecordCount);
            }

            if (EvaluationObject.SelectFieldList.Count == 0)
            {
                builder.Append("*");
            }
            else
            {
                builder.Append(new SqlServerSelectFieldListEvaluator(EvaluationObject.SelectFieldList).ToString());
            }

            builder.AppendLine();
            builder.AppendFormat("FROM {0}", GetObjectName(EvaluationObject.ObjectType));

            if (EvaluationObject.JoinList.Count != 0)
            {
                builder.AppendFormat(" {0}", new SqlServerJoinListEvaluator(EvaluationObject.JoinList).ToString());
            }

            if (EvaluationObject.Filter != null)
            {
                string filterString = SqlServerFilterEvaluatorFactory.Instance.GetFilterEvaluator(EvaluationObject.Filter).ToString();

                if (!String.IsNullOrWhiteSpace(filterString))
                {
                    builder.AppendLine();
                    builder.AppendFormat("WHERE {0}", filterString);
                }
            }

            if (EvaluationObject.OrderByList != null)
            {
                string orderByString = new SqlServerOrderByListEvaluator(EvaluationObject.OrderByList).ToString();

                if (!String.IsNullOrWhiteSpace(orderByString))
                {
                    builder.AppendLine();
                    builder.AppendFormat("ORDER BY {0}", orderByString);
                }
            }

            if (EvaluationObject.IsSelectWithPaging())
            {
                builder.AppendLine();
                builder.AppendFormat(PaginationFormat, EvaluationObject.GetOffset(), EvaluationObject.RecordCount);
            }

            return builder.ToString();
        }

        private object GetObjectName(GeneralObject obj)
        {
            if (String.IsNullOrEmpty(obj.SchemaName))
            {
                return String.Format("[{0}]", obj.TableName);
            }
            else
            {
                return String.Format("[{0}].[{1}]", obj.SchemaName, obj.TableName);
            }
        }
    }
}
