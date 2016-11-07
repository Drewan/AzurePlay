using System.Runtime.Serialization;

namespace WebApplication.Tests
{
    [DataContract]
    public class Event
    {
        [DataMember]
        public string Content { get; set; }
    }
}
