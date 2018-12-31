using System;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerOrderByEvaluator : OrderByEvaluator
    {
        public SqlServerOrderByEvaluator()
        {
        }

        public SqlServerOrderByEvaluator(OrderBy evaluationObject) : base(evaluationObject)
        {
        }

        public override string ToString()
        {
            if (String.IsNullOrWhiteSpace(EvaluationObject.TableName))
            {
                throw new ArgumentNullException("TableName");
            }

            if (String.IsNullOrWhiteSpace(EvaluationObject.FieldName))
            {
                throw new ArgumentNullException("FieldName");
            }

            switch (EvaluationObject.OrderByType)
            {
                case OrderByType.Ascending:
                    return String.Format("[{0}].[{1}] ASC", EvaluationObject.TableName, EvaluationObject.FieldName);
                case OrderByType.Descending:
                    return String.Format("[{0}].[{1}] DESC", EvaluationObject.TableName, EvaluationObject.FieldName);
                default:
                    throw new Exception("This operator type is not supported");
            }
        }
    }

    public class SqlServerOrderByListEvaluator : OrderByListEvaluator<SqlServerOrderByEvaluator>
    {
        public SqlServerOrderByListEvaluator(OrderByList evaluationObject) : base(evaluationObject)
        {
        }
    }
}
