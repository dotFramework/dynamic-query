using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public enum ProviderType
    {
        [EnumMember]
        SqlServer,
        [EnumMember]
        Oracle,
        [EnumMember]
        MySql,
        [EnumMember]
        Sqlite
    }
}
