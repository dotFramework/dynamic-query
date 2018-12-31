using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class SelectField
    {
        public SelectField()
        {

        }

        public SelectField(string tableName, string fieldName)
        {
            TableName = tableName;
            FieldName = fieldName;
        }

        public SelectField(string tableName, string fieldName, string aggrigateMethod) : this(tableName, fieldName)
        {
            AggrigateMethod = aggrigateMethod;
        }

        public SelectField(string tableName, string fieldName, string aggrigateMethod, string alias) : this(tableName, fieldName, aggrigateMethod)
        {
            Alias = alias;
        }

        public SelectField(string tableName, string fieldName, AggregateMethodType aggregateMethodType) : this(tableName, fieldName)
        {
            AggregateMethodType = aggregateMethodType;
        }

        public SelectField(string tableName, string fieldName, AggregateMethodType aggregateMethodType, string alias) : this(tableName, fieldName, aggregateMethodType)
        {
            Alias = alias;
        }

        [DataMember]
        public string TableName { get; internal set; }

        [DataMember]
        public string FieldName { get; internal set; }

        [DataMember]
        public AggregateMethodType AggregateMethodType { get; internal set; }

        [DataMember]
        public string AggrigateMethod { get; private set; }

        [DataMember]
        public string Alias { get; internal set; }
    }

    [CollectionDataContract]
    public class SelectFieldList : List<SelectField>
    {
        public SelectFieldList()
        {

        }

        public SelectFieldList(SelectField selectField)
        {
            base.Add(selectField);
        }

        public SelectFieldList(SelectFieldList selectFieldList)
        {
            base.AddRange(selectFieldList);
        }

        protected virtual string Format
        {
            get
            {
                return " {0},";
            }
        }

        public new SelectFieldList Add(SelectField selectField)
        {
            base.Add(selectField);
            return this;
        }

        public new SelectFieldList AddRange(IEnumerable<SelectField> selectFields)
        {
            base.AddRange(selectFields);
            return this;
        }
    }
}
