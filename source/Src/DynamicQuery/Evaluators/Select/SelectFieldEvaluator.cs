using System;
using System.Text;

namespace DotFramework.DynamicQuery
{
    public abstract class SelectFieldEvaluator : AbstractEvaluator<SelectField>
    {
        public SelectFieldEvaluator()
        {

        }

        public SelectFieldEvaluator(SelectField evaluationObject) : base(evaluationObject)
        {
        }

        protected abstract string FieldFormat { get; }

        protected abstract string AliasFormat { get; }

        protected abstract string AggregateFieldFormat { get; }

        protected abstract string ConvertAggregateMethodTypeToString(AggregateMethodType aggregateMethodType);

        private string GetAggregateMethod()
        {
            if (String.IsNullOrEmpty(EvaluationObject.AggrigateMethod) && EvaluationObject.AggregateMethodType != AggregateMethodType.None)
            {
                return ConvertAggregateMethodTypeToString(EvaluationObject.AggregateMethodType);
            }

            return EvaluationObject.AggrigateMethod;
        }

        public virtual new string ToString()
        {
            if (String.IsNullOrWhiteSpace(FieldFormat))
            {
                throw new ArgumentNullException("FieldFormat");
            }

            if (String.IsNullOrWhiteSpace(AliasFormat))
            {
                throw new ArgumentNullException("AliasFormat");
            }

            if (String.IsNullOrWhiteSpace(AggregateFieldFormat))
            {
                throw new ArgumentNullException("AggregateFieldFormat");
            }

            string field = String.Empty;

            if (String.IsNullOrWhiteSpace(EvaluationObject.TableName))
            {
                if (EvaluationObject.FieldName == "*")
                {
                    field = EvaluationObject.FieldName;
                }
                else
                {
                    field = String.Format(FieldFormat, EvaluationObject.FieldName);
                }
            }
            else
            {
                field = String.Format("{0}.{1}", String.Format(FieldFormat, EvaluationObject.TableName), String.Format(FieldFormat, EvaluationObject.FieldName));
            }

            if (!String.IsNullOrWhiteSpace(GetAggregateMethod()))
            {
                field = String.Format(AggregateFieldFormat, GetAggregateMethod(), field);
            }

            if (!String.IsNullOrWhiteSpace(EvaluationObject.Alias))
            {
                field = String.Format(AliasFormat, field, EvaluationObject.Alias);
            }

            return field;
        }
    }

    public abstract class SelectFieldListEvaluator<TSelectFieldEvaluator> : AbstractEvaluator<SelectFieldList>
        where TSelectFieldEvaluator : SelectFieldEvaluator, new()
    {
        public SelectFieldListEvaluator(SelectFieldList evaluationObject) : base(evaluationObject)
        {
        }

        protected virtual string Format
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

                foreach (var field in EvaluationObject)
                {
                    builder.AppendFormat(Format, new TSelectFieldEvaluator { EvaluationObject = field }.ToString());
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
