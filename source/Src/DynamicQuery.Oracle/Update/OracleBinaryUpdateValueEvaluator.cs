using System;

namespace DotFramework.DynamicQuery.Oracle
{
    public class OracleBinaryUpdateValueEvaluator : BinaryUpdateValueEvaluator
    {
        public OracleBinaryUpdateValueEvaluator(BinaryUpdateValue evaluationObject) : base(evaluationObject)
        {
        }

        protected override string ConvertValue(object value)
        {
            string strValue = String.Empty;

            if (value is BinaryUpdateValue binaryUpdateValue)
            {
                strValue = new OracleBinaryUpdateValueEvaluator(binaryUpdateValue).ToString();
            }
            else if (value is BinaryUpdateMember binaryUpdateMember)
            {
                strValue = new OracleBinaryUpdateMemberEvaluator(binaryUpdateMember).ToString();
            }
            else if (value.IsNumber())
            {
                strValue = value.ToString();
            }
            else if (value is String str)
            {
                strValue = str.Replace("'", "''");
                strValue = String.Format("'{0}'", str);
            }
            else if (value is bool)
            {
                strValue = Convert.ToByte(value).ToString();
            }
            else if (value is DateTime)
            {
                throw new NotImplementedException();
            }

            return strValue;
        }
    }
}
