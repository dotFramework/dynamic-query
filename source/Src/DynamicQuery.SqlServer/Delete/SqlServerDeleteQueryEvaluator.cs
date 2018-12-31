using DotFramework.DynamicQuery.Metadata;
using System;
using System.Text;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerDeleteQueryEvaluator : DeleteQueryEvaluator
    {
        public SqlServerDeleteQueryEvaluator(DeleteQuery evaluationObject) : base(evaluationObject)
        {
        }

        protected override string GenerateQuery()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("DELETE FROM {0}", GetObjectName(EvaluationObject.ObjectType));

            if (EvaluationObject.Filter != null)
            {
                string filterString = SqlServerFilterEvaluatorFactory.Instance.GetFilterEvaluator(EvaluationObject.Filter).ToString();

                if (!String.IsNullOrWhiteSpace(filterString))
                {
                    builder.AppendLine();
                    builder.AppendFormat("WHERE {0}", filterString);
                }
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
