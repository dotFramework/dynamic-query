using System;
using System.Text;

namespace DotFramework.DynamicQuery
{
    public abstract class UpdateFieldEvaluator : AbstractEvaluator<UpdateField>
    {
        public UpdateFieldEvaluator()
        {

        }

        public UpdateFieldEvaluator(UpdateField evaluationObject) : base(evaluationObject)
        {
        }

        protected abstract string FieldFormat { get; }

        public virtual new string ToString()
        {
            if (String.IsNullOrWhiteSpace(FieldFormat))
            {
                throw new ArgumentNullException("FieldFormat");
            }

            if (String.IsNullOrWhiteSpace(EvaluationObject.FieldName))
            {
                throw new ArgumentNullException("FieldName");
            }

            return String.Format(FieldFormat, EvaluationObject.FieldName, GetValueString());
        }

        protected abstract string GetValueString();
    }

    public abstract class UpdateFieldListEvaluator<TUpdateFieldEvaluator> : AbstractEvaluator<UpdateFieldList>
        where TUpdateFieldEvaluator : UpdateFieldEvaluator, new()
    {
        public UpdateFieldListEvaluator(UpdateFieldList evaluationObject) : base(evaluationObject)
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
                    builder.AppendFormat(Format, new TUpdateFieldEvaluator { EvaluationObject = field }.ToString());
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
