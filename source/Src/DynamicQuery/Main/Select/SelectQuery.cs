using DotFramework.DynamicQuery.Metadata;
using System;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class SelectQuery : AbstractQuery
    {
        #region Constructors

        public SelectQuery()
        {
            SelectFieldList = new SelectFieldList();
            JoinList = new JoinList();
            OrderByList = new OrderByList();
        }

        public SelectQuery(Type sourceType)
        {
            ObjectType = MetadataHelper.GetGeneralObject(sourceType);
        }

        #endregion

        #region Properties

        [DataMember]
        public bool Distinct { get; set; }

        [DataMember]
        public UInt64 PageNumber { get; set; }

        [DataMember]
        public UInt64 RecordCount { get; set; }

        [DataMember]
        public UInt64 Offset { get; set; }

        [DataMember]
        public SelectFieldList SelectFieldList { get; set; }

        [DataMember]
        public JoinList JoinList { get; set; }

        [DataMember]
        public OrderByList OrderByList { get; set; }

        #endregion

        #region Abstract Properties

        

        #endregion

        #region Protected Method

        public bool IsSelectWithTop()
        {
            if (RecordCount != 0)
            {
                if (PageNumber <= 1 && Offset == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsSelectWithPaging()
        {
            if (RecordCount != 0)
            {
                if (PageNumber <= 1 && Offset == 0)
                {
                    return false;
                }
                else if (PageNumber > 1 || Offset != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public ulong GetOffset()
        {
            if (RecordCount != 0)
            {
                if (PageNumber <= 1 && Offset == 0)
                {
                    return 0;
                }
                else if (PageNumber > 1)
                {
                    return (PageNumber - 1) * RecordCount;
                }
                else if (PageNumber <= 1 && Offset != 0)
                {
                    return Offset;
                }
            }

            return 0;
        }

        #endregion
    }
}
