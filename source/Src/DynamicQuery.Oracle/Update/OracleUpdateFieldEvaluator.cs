using System;

namespace DotFramework.DynamicQuery.Oracle
{
    public class OracleUpdateFieldEvaluator : UpdateFieldEvaluator
    {
        public OracleUpdateFieldEvaluator()
        {
        }

        public OracleUpdateFieldEvaluator(UpdateField evaluationObject) : base(evaluationObject)
        {
        }

        protected override string FieldFormat => "\"{0}\" = {1}";

        protected override string GetValueString()
        {
            string result = String.Empty;

            if (EvaluationObject.Value == null)
            {
                result = "NULL";
            }
            else if (EvaluationObject.Value is BinaryUpdateValue binaryUpdateValue)
            {
                result = new OracleBinaryUpdateValueEvaluator(binaryUpdateValue).ToString();
            }
            else if (EvaluationObject.Value.IsNumber())
            {
                result = EvaluationObject.Value.ToString();
            }
            else if (EvaluationObject.Value is String str)
            {
                result = str.ToString().Replace("'", "''");
                result = String.Format("N'{0}'", result);
            }
            else if (EvaluationObject.Value is bool)
            {
                result = Convert.ToByte(EvaluationObject.Value).ToString();
            }
            else if (EvaluationObject.Value is DateTime dateTimeValue)
            {
                result = String.Format("TO_DATE('{0}', 'YYYY/MM/DD HH24:MI:SS')", dateTimeValue.ToString("yyyy/MM/dd HH:mm:ss"));
            }
            else
            {
                throw new NotSupportedException("Not Supported Value Type");
            }

            return result;
        }
    }

    public class OracleUpdateFieldListEvaluator : UpdateFieldListEvaluator<OracleUpdateFieldEvaluator>
    {
        public OracleUpdateFieldListEvaluator(UpdateFieldList evaluationObject) : base(evaluationObject)
        {
        }
    }
}
