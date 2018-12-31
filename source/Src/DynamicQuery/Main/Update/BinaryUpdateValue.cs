using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class BinaryUpdateValue
    {
        public BinaryUpdateValue(object left, object right, ExpressionType nodeType)
        {
            Left = left;
            Right = right;
            NodeType = nodeType;
        }

        [DataMember]
        public object Left { get; internal set; }

        [DataMember]
        public object Right { get; internal set; }

        [DataMember]
        public ExpressionType NodeType { get; internal set; }
    }
}
