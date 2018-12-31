using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class UpdateField
    {
        public UpdateField()
        {

        }

        public UpdateField(string fieldName, object value)
        {
            FieldName = fieldName;
            Value = value;
        }

        [DataMember]
        public string FieldName { get; internal set; }

        [DataMember]
        public object Value { get; internal set; }
    }

    [CollectionDataContract]
    public class UpdateFieldList : List<UpdateField>
    {
        public UpdateFieldList()
        {

        }

        public UpdateFieldList(UpdateField UpdateField)
        {
            base.Add(UpdateField);
        }

        public UpdateFieldList(UpdateFieldList UpdateFieldList)
        {
            base.AddRange(UpdateFieldList);
        }

        public new UpdateFieldList Add(UpdateField UpdateField)
        {
            base.Add(UpdateField);
            return this;
        }

        public new UpdateFieldList AddRange(IEnumerable<UpdateField> UpdateFields)
        {
            base.AddRange(UpdateFields);
            return this;
        }
    }
}
