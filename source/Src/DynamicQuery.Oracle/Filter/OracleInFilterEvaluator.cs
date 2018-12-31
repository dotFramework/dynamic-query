using System;

namespace DotFramework.DynamicQuery.Oracle
{
    public class OracleInFilterEvaluator : InFilterEvaluator
    {
        public OracleInFilterEvaluator(InFilter evaluationObject) : base(evaluationObject)
        {
        }

        public override string ToString()
        {
            string strFilter = "";

            if (EvaluationObject.FilterValues.Count != 0)
            {
                foreach (var filterValue in EvaluationObject.FilterValues)
                {
                    object value = filterValue;

                    if (value is String)
                    {
                        value = value.ToString().Replace("'", "''");
                    }

                    if (value.IsNumber())
                    {
                        strFilter += String.Format("{0}, ", value);
                    }
                    else if (value is bool)
                    {
                        strFilter += String.Format("{0}, ", Convert.ToByte(value));
                    }
                    else
                    {
                        strFilter += String.Format("N'{0}', ", value);
                    }
                }

                strFilter = String.Format("{0} {1} ({2})", EvaluationObject.GetFieldName(), EvaluationObject.FilterCondition.ToString().SplitCamelCase().ToUpper(), strFilter.TrimEnd(' ', ','));
            }
            else
            {
                throw new Exception("'Values' must contain at least one item.");
            }

            return strFilter;
        }
    }
}
