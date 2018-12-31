using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public enum SqlOperators
    {
        [EnumMember]
        Greater,
        [EnumMember]
        Less,
        [EnumMember]
        Equal,
        [EnumMember]
        StartsWithLike,
        [EnumMember]
        NotStartsWithLike,
        [EnumMember]
        EndsWithLike,
        [EnumMember]
        NotEndsWithLike,
        [EnumMember]
        Like,
        [EnumMember]
        NotLike,
        [EnumMember]
        LessOrEqual,
        [EnumMember]
        GreaterOrEqual,
        [EnumMember]
        NotEqual
    }
}
