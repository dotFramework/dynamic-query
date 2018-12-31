using System;

namespace DotFramework.DynamicQuery.SqlServer
{
    public class SqlServerOrFilterEvaluator : OrFilterEvaluator
    {
        public SqlServerOrFilterEvaluator(OrFilter evaluationObject) : base(evaluationObject)
        {
        }

        public override string ToString()
        {
            string strFilter = "";

            if (EvaluationObject.FilterList != null && EvaluationObject.FilterList.Count != 0)
            {
                for (int i = 0; i < EvaluationObject.FilterList.Count - 1; i++)
                {
                    strFilter += SqlServerFilterEvaluatorFactory.Instance.GetFilterEvaluator(EvaluationObject.FilterList[i]).ToString() + " OR ";
                }

                strFilter += SqlServerFilterEvaluatorFactory.Instance.GetFilterEvaluator(EvaluationObject.FilterList[EvaluationObject.FilterList.Count - 1]).ToString();
                strFilter = String.Format("({0})", strFilter);
            }

            return strFilter;
        }
    }
}
