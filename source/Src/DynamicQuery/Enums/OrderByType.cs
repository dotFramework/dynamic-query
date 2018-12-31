using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public enum OrderByType
    {
        [EnumMember]
        Ascending,
        [EnumMember]
        Descending
    }
}
