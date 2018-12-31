using System;
using System.Text;

namespace DotFramework.DynamicQuery
{
    public abstract class JoinEvaluator : AbstractEvaluator<Join>
    {
        public JoinEvaluator()
        {

        }

        public JoinEvaluator(Join evaluationObject) : base(evaluationObject)
        {
        }

        public abstract new string ToString();
    }

    public abstract class JoinListEvaluator<TJoinEvaluator> : AbstractEvaluator<JoinList>
        where TJoinEvaluator : JoinEvaluator, new()
    {
        public JoinListEvaluator(JoinList evaluationObject) : base(evaluationObject)
        {
        }

        public virtual new string ToString()
        {
            if (EvaluationObject.Count != 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var join in EvaluationObject)
                {
                    builder.AppendFormat("{0} ", new TJoinEvaluator { EvaluationObject = join }.ToString());
                }

                return builder.ToString().TrimStart(' ');
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
