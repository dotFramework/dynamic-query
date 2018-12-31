using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public abstract class AbstractFilter
    {
        public static AbstractFilter operator &(AbstractFilter filterExpressionLeft, AbstractFilter filterExpressionRight)
        {
            return new AndFilter(filterExpressionLeft, filterExpressionRight);
        }

        public static AbstractFilter operator |(AbstractFilter filterExpressionLeft, AbstractFilter filterExpressionRight)
        {
            return new OrFilter(filterExpressionLeft, filterExpressionRight);
        }
    }
}
