using System;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerFilterExpressionEvaluator : FilterExpressionEvaluator
    {
        public SqlServerFilterExpressionEvaluator(FilterExpression evaluationObject) : base(evaluationObject)
        {
        }

        public override string ToString()
        {
            string strFilter = "";

            if (EvaluationObject.Value is String strVale)
            {
                EvaluationObject.Value = strVale.Replace("'", "''");
            }

            string value = String.Format("N'{0}'", EvaluationObject.Value);

            if (EvaluationObject.Value.IsNumber())
            {
                value = EvaluationObject.Value.ToString();
            }

            if (EvaluationObject.Value is bool)
            {
                value = Convert.ToByte(EvaluationObject.Value).ToString();
            }

            switch (EvaluationObject.Operator)
            {
                case SqlOperators.Greater:
                    strFilter = String.Format("{0} > {1}", GetFieldName(), value);
                    break;
                case SqlOperators.Less:
                    strFilter = String.Format("{0} < {1}", GetFieldName(), value);
                    break;
                case SqlOperators.Equal:
                    if (EvaluationObject.Value != null)
                    {
                        strFilter = String.Format("{0} = {1}", GetFieldName(), value);
                    }
                    else
                    {
                        strFilter = String.Format("{0} IS NULL", GetFieldName());
                    }
                    break;
                case SqlOperators.StartsWithLike:
                    strFilter = String.Format("{0} LIKE N'{1}%'", GetFieldName(), EvaluationObject.Value);
                    break;
                case SqlOperators.NotStartsWithLike:
                    strFilter = String.Format("{0} NOT LIKE N'{1}%'", GetFieldName(), EvaluationObject.Value);
                    break;
                case SqlOperators.EndsWithLike:
                    strFilter = String.Format("{0} LIKE N'%{1}'", GetFieldName(), EvaluationObject.Value);
                    break;
                case SqlOperators.NotEndsWithLike:
                    strFilter = String.Format("{0} NOT LIKE N'%{1}'", GetFieldName(), EvaluationObject.Value);
                    break;
                case SqlOperators.Like:
                    strFilter = String.Format("{0} LIKE N'%{1}%'", GetFieldName(), EvaluationObject.Value);
                    break;
                case SqlOperators.NotLike:
                    strFilter = String.Format("{0} NOT LIKE N'%{1}%'", GetFieldName(), EvaluationObject.Value);
                    break;
                case SqlOperators.LessOrEqual:
                    strFilter = String.Format("{0} <= {1}", GetFieldName(), value);
                    break;
                case SqlOperators.GreaterOrEqual:
                    strFilter = String.Format("{0} >= {1}", GetFieldName(), value);
                    break;
                case SqlOperators.NotEqual:
                    if (EvaluationObject.Value != null)
                    {
                        strFilter = String.Format("{0} != {1}", GetFieldName(), value);
                    }
                    else
                    {
                        strFilter = String.Format("{0} IS NOT NULL", GetFieldName());
                    }
                    break;
                default:
                    throw new Exception("This operator type is not supported");
            }

            return strFilter;
        }

        private string GetFieldName()
        {
            if (String.IsNullOrWhiteSpace(EvaluationObject.FieldName))
            {
                throw new ArgumentNullException("FieldName");
            }

            string fieldName = String.Empty;

            if (String.IsNullOrWhiteSpace(EvaluationObject.TableName))
            {
                fieldName = String.Format("[{0}]", EvaluationObject.FieldName);
            }
            else
            {
                fieldName = String.Format("[{0}].[{1}]", EvaluationObject.TableName, EvaluationObject.FieldName);
            }

            if (!String.IsNullOrWhiteSpace(EvaluationObject.MethodName))
            {
                fieldName = String.Format("{0}({1})", EvaluationObject.MethodName, fieldName);
            }

            return fieldName;
        }
    }
}
