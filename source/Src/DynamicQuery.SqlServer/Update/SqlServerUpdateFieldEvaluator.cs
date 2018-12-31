using System;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerUpdateFieldEvaluator : UpdateFieldEvaluator
    {
        public SqlServerUpdateFieldEvaluator()
        {
        }

        public SqlServerUpdateFieldEvaluator(UpdateField evaluationObject) : base(evaluationObject)
        {
        }

        protected override string FieldFormat => "[{0}] = {1}";

        protected override string GetValueString()
        {
            string result = String.Empty;

            if (EvaluationObject.Value == null)
            {
                result = "NULL";
            }
            else if (EvaluationObject.Value is BinaryUpdateValue binaryUpdateValue)
            {
                result = new SqlServerBinaryUpdateValueEvaluator(binaryUpdateValue).ToString();
            }
            else if (EvaluationObject.Value.IsNumber())
            {
                result = EvaluationObject.Value.ToString();
            }
            else if (EvaluationObject.Value is String str)
            {
                result = str.Replace("'", "''");
                result = String.Format("N'{0}'", result);
            }
            else if (EvaluationObject.Value is bool)
            {
                result = Convert.ToByte(EvaluationObject.Value).ToString();
            }
            else if (EvaluationObject.Value is DateTime dateTimeValue)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException("Not Supported Value Type");
            }

            return result;
        }
    }

    public class SqlServerUpdateFieldListEvaluator : UpdateFieldListEvaluator<SqlServerUpdateFieldEvaluator>
    {
        public SqlServerUpdateFieldListEvaluator(UpdateFieldList evaluationObject) : base(evaluationObject)
        {
        }
    }
}
