using System;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerBinaryUpdateValueEvaluator : BinaryUpdateValueEvaluator
    {
        public SqlServerBinaryUpdateValueEvaluator(BinaryUpdateValue evaluationObject) : base(evaluationObject)
        {
        }

        protected override string ConvertValue(object value)
        {
            string strValue = String.Empty;

            if (value is BinaryUpdateValue binaryUpdateValue)
            {
                strValue = new SqlServerBinaryUpdateValueEvaluator(binaryUpdateValue).ToString();
            }
            else if (value is BinaryUpdateMember binaryUpdateMember)
            {
                strValue = new SqlServerBinaryUpdateMemberEvaluator(binaryUpdateMember).ToString();
            }
            else if (value.IsNumber())
            {
                strValue = value.ToString();
            }
            else if (value is String str)
            {
                strValue = str.Replace("'", "''");
                strValue = String.Format("N'{0}'", strValue);
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
