using Lokad.Cloud.Storage;

namespace WebApplication.Tests
{
    public class Message : BlobName<Event>
    {
        public override string ContainerName
        {
            get { return "events"; }
        }

        [Rank(1, true)]
        public long SequenceId { get; set; }

        [Rank(0)]
        public string RowKey { get; set; }
    }
}