using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class InFilter : AbstractFilter
    {
        public InFilter(string tableName, string fieldName, IList filterValues, InFilterCondition filterCondition = InFilterCondition.In)
        {
            TableName = tableName;
            FieldName = fieldName;
            FilterCondition = filterCondition;

            if (filterValues.Count != 0)
            {
                FilterValues = filterValues.Cast<Object>().ToList();
            }
        }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public string FieldName { get; set; }

        [DataMember]
        public InFilterCondition FilterCondition { get; set; }

        private List<Object> _FilterValues;
        [DataMember]
        public List<Object> FilterValues
        {
            get
            {
                if (_FilterValues == null)
                    _FilterValues = new List<Object>();
                return _FilterValues;
            }
            set
            {
                _FilterValues = value;
            }
        }

        public string GetFieldName()
        {
            if (String.IsNullOrWhiteSpace(FieldName))
            {
                throw new ArgumentNullException("FieldName");
            }

            if (String.IsNullOrWhiteSpace(TableName))
            {
                return String.Format("\"{0}\"", FieldName);
            }
            else
            {
                return String.Format("\"{0}\".\"{1}\"", TableName, FieldName);
            }
        }
    }
}
