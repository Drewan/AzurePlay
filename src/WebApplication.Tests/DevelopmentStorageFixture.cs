using System.Linq;
using System.Runtime.Serialization;
using Lokad.Cloud.Storage;
using Xunit;

namespace WebApplication.Tests
{
    public class BlobStorageFixture
    {
        protected const string ContainerName = "events";

        public BlobStorageFixture()
        {
            Storage = CloudStorage
                .ForInMemoryStorage()
                .BuildBlobStorage();
            Storage.CreateContainerIfNotExist(ContainerName);
        }

        public IBlobStorageProvider Storage { get; set; }
    }


    public class AzureStorageTests : BlobStorageFixture
    {
        [Fact]
        public void CanWriteBlobToMemory()
        {
            var metadata = new Message
            {
                SequenceId = 2,
                RowKey = "126D1938-034C-449E-9E3B-1395BB106816"
            };
            var message = new Event { Content = "{}" };

            Storage.PutBlob(metadata, message);

            var blob = Storage.ListBlobs<Event>("events").Single();

            Assert.Equal(blob.Content, "{}");
        }
    }
    
    [DataContract]
    public class Event
    {
        [DataMember]
        public string Content { get; set; }
    }

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
