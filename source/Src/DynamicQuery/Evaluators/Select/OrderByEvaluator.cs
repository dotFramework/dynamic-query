using System;
using System.Text;

namespace DotFramework.DynamicQuery
{
    public abstract class OrderByEvaluator : AbstractEvaluator<OrderBy>
    {
        public OrderByEvaluator()
        {

        }

        public OrderByEvaluator(OrderBy evaluationObject) : base(evaluationObject)
        {
        }

        public abstract new string ToString();
    }

    public abstract class OrderByListEvaluator<TOrderByEvaluator> : AbstractEvaluator<OrderByList>
        where TOrderByEvaluator : OrderByEvaluator, new()
    {
        public OrderByListEvaluator(OrderByList evaluationObject) : base(evaluationObject)
        {
        }

        protected virtual string OrderByFormat
        {
            get
            {
                return " {0},";
            }
        }

        public virtual new string ToString()
        {
            if (EvaluationObject.Count != 0)
            {
                StringBuilder builder = new StringBuilder();

                foreach (var orderby in EvaluationObject)
                {
                    builder.AppendFormat(OrderByFormat, new TOrderByEvaluator { EvaluationObject = orderby }.EvaluationObject);
                }

                return builder.ToString().TrimStart(' ').TrimEnd(',');
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
