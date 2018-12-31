using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    public class FilterExpression : AbstractFilter
    {
        public FilterExpression(string tableName, string fieldName, SqlOperators sqlOperator, object value)
        {
            TableName = tableName;
            FieldName = fieldName;
            Operator = sqlOperator;
            Value = value;
        }

        [JsonConstructor]
        public FilterExpression(string tableName, string fieldName, SqlOperators sqlOperator, string methodName, object value)
        {
            TableName = tableName;
            FieldName = fieldName;
            Operator = sqlOperator;
            MethodName = methodName;
            Value = value;
        }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public string FieldName { get; set; }

        [DataMember]
        public string MethodName { get; set; }

        [DataMember]
        public object Value { get; set; }

        [DataMember]
        public SqlOperators Operator { get; set; }
    }
}
