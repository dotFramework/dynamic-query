using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class BinaryUpdateMember
    {
        public BinaryUpdateMember()
        {

        }

        public BinaryUpdateMember(string memberName) : this()
        {
            MemberName = memberName;
        }

        [DataMember]
        public string MemberName { get; internal set; }
    }
}
