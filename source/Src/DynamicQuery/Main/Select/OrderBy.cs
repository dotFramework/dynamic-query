using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class OrderBy
    {
        public OrderBy()
        {

        }

        public OrderBy(string tableName, string fieldName, OrderByType orderByType = OrderByType.Ascending)
        {
            TableName = tableName;
            FieldName = fieldName;
            OrderByType = orderByType;
        }

        [DataMember]
        public string TableName { get; internal set; }

        [DataMember]
        public string FieldName { get; internal set; }

        [DataMember]
        public OrderByType OrderByType { get; set; }
    }

    [CollectionDataContract]
    public class OrderByList : List<OrderBy>
    {
        public OrderByList()
        {

        }

        public OrderByList(OrderBy orderBy)
        {
            base.Add(orderBy);
        }

        public OrderByList(OrderByList orderByList)
        {
            base.AddRange(orderByList);
        }

        protected virtual string OrderByFormat
        {
            get
            {
                return " {0},";
            }
        }

        public new OrderByList Add(OrderBy ordeBy)
        {
            base.Add(ordeBy);
            return this;
        }

        public new OrderByList AddRange(IEnumerable<OrderBy> orderby)
        {
            base.AddRange(orderby);
            return this;
        }
    }
}
