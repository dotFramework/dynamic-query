using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public enum AggregateMethodType
    {
        [EnumMember]
        None,
        [EnumMember]
        Count,
        [EnumMember]
        Sum,
        [EnumMember]
        Min,
        [EnumMember]
        Max,
        [EnumMember]
        Average,
    }
}
