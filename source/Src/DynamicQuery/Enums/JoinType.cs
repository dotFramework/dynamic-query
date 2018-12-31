using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public enum JoinType
    {
        [EnumMember]
        InnerJoin,
        [EnumMember]
        LeftJoin,
        [EnumMember]
        RightJoin,
        [EnumMember]
        FullOuterJoin
    }
}
