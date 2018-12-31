using System;
using System.Linq.Expressions;

namespace DotFramework.DynamicQuery
{
    public abstract class BinaryUpdateValueEvaluator : AbstractEvaluator<BinaryUpdateValue>
    {
        public BinaryUpdateValueEvaluator(BinaryUpdateValue evaluationObject) : base(evaluationObject)
        {
        }

        public new string ToString()
        {
            return GetValueString();
        }

        protected virtual string GetValueString()
        {
            string left = ConvertValue(EvaluationObject.Left);
            string right = ConvertValue(EvaluationObject.Right);
            string nodeTypeString = ConvertNodeType(EvaluationObject.NodeType);

            return $"({left} {nodeTypeString} {right})";
        }

        protected virtual string ConvertNodeType(ExpressionType nodeType)
        {
            string nodeTypeString = String.Empty;

            switch (nodeType)
            {
                case ExpressionType.Add:
                    nodeTypeString = "+";
                    break;
                case ExpressionType.Subtract:
                    nodeTypeString = "-";
                    break;
                case ExpressionType.Multiply:
                    nodeTypeString = "*";
                    break;
                case ExpressionType.Divide:
                    nodeTypeString = "/";
                    break;
                default:
                    throw new NotSupportedException("Unsupported NodeType");
            }

            return nodeTypeString;
        }

        protected abstract string ConvertValue(object value);
    }
}
