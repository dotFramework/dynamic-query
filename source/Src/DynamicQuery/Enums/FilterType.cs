using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public enum FilterType
    {
        [EnumMember]
        Expression,
        [EnumMember]
        And,
        [EnumMember]
        Or,
        [EnumMember]
        In
    }
}
