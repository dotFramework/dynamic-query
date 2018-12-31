using System;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerAndFilterEvaluator : AndFilterEvaluator
    {
        public SqlServerAndFilterEvaluator(AndFilter evaluationObject) : base(evaluationObject)
        {
        }

        public override string ToString()
        {
            string strFilter = "";

            if (EvaluationObject.FilterList != null && EvaluationObject.FilterList.Count != 0)
            {
                for (int i = 0; i < EvaluationObject.FilterList.Count - 1; i++)
                {
                    strFilter += SqlServerFilterEvaluatorFactory.Instance.GetFilterEvaluator(EvaluationObject.FilterList[i]).ToString() + " AND ";
                }

                strFilter += SqlServerFilterEvaluatorFactory.Instance.GetFilterEvaluator(EvaluationObject.FilterList[EvaluationObject.FilterList.Count - 1]).ToString();
                strFilter = String.Format("({0})", strFilter);
            }

            return strFilter;
        }
    }
}
