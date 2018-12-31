using DotFramework.DynamicQuery.Metadata;
using System;
using System.Text;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerUpdateQueryEvaluator : UpdateQueryEvaluator
    {
        public SqlServerUpdateQueryEvaluator(UpdateQuery evaluationObject) : base(evaluationObject)
        {
        }

        protected override string GenerateQuery()
        {
            EvaluationObject.ValidateQuery();

            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("UPDATE [{0}]", GetObjectName(EvaluationObject.ObjectType));
            builder.AppendLine();
            builder.AppendFormat("SET {0}", new SqlServerUpdateFieldListEvaluator(EvaluationObject.UpdateFieldList).ToString());

            string filterString = SqlServerFilterEvaluatorFactory.Instance.GetFilterEvaluator(EvaluationObject.Filter).ToString();

            if (EvaluationObject.Filter != null && !String.IsNullOrWhiteSpace(filterString))
            {
                builder.AppendLine();
                builder.AppendFormat("WHERE {0}", filterString);
            }

            return builder.ToString();
        }

        private object GetObjectName(GeneralObject obj)
        {
            if (String.IsNullOrEmpty(obj.SchemaName))
            {
                return String.Format("\"{0}\"", obj.TableName);
            }
            else
            {
                return String.Format("\"{0}\".\"{1}\"", obj.SchemaName, obj.TableName);
            }
        }
    }
}
