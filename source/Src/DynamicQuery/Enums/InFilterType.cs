using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public enum InFilterCondition
    {
        [EnumMember]
        In,
        [EnumMember]
        NotIn,
    }
}
