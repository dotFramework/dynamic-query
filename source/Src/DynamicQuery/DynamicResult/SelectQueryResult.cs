using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class SelectQueryResult : List<dynamic>
    {
        public SelectQueryResult(DbDataReader dataReader)
        {
            if (dataReader.HasRows)
            {
                foreach (IDataRecord record in dataReader)
                {
                    Add(new DataRecordDynamicWrapper(record));
                }
            }
        }

        //[DataMember]
        //public string SerializedObject { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}
