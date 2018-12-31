using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class Join
    {
        #region Constructors

        public Join()
        {

        }

        public Join(string outerTable, string outerKey, string innerTable, string innerKey, JoinType joinType)
        {
            OuterTable = outerTable;
            OuterKey = outerKey;

            InnerTable = innerTable;
            InnerKey = innerKey;

            JoinType = joinType;
        }

        public Join(string outerTable, string outerKey, string innerTable, string innerKey, JoinType joinType, string outerTableAlias) : this(outerTable, outerKey, innerTable, innerKey, joinType)
        {
            OuterTableAlias = outerTableAlias;
        }

        public Join(string outerTable, string outerKey, string innerTable, string innerKey, JoinType joinType, string outerTableAlias, string innerTableAlias) : this(outerTable, outerKey, innerTable, innerKey, joinType, outerTableAlias)
        {
            InnerTableAlias = innerTableAlias;
        }

        #endregion

        #region Properties

        [DataMember]
        public string OuterTable { get; internal set; }

        [DataMember]
        public string OuterKey { get; internal set; }

        [DataMember]
        public string OuterTableAlias { get; internal set; }

        [DataMember]
        public string InnerTable { get; internal set; }

        [DataMember]
        public string InnerKey { get; internal set; }

        [DataMember]
        public string InnerTableAlias { get; internal set; }

        [DataMember]
        public JoinType JoinType { get; internal set; }

        #endregion
    }

    [CollectionDataContract]
    public class JoinList : List<Join>
    {
        public JoinList()
        {

        }

        public JoinList(Join join)
        {
            base.Add(join);
        }

        public JoinList(JoinList joinList)
        {
            base.AddRange(joinList);
        }

        public new JoinList Add(Join join)
        {
            base.Add(join);
            return this;
        }

        public new JoinList AddRange(IEnumerable<Join> joins)
        {
            base.AddRange(joins);
            return this;
        }
    }
}
