using System.Linq;
using Lokad.Cloud.Storage;
using Xunit;

namespace WebApplication.Tests
{
    public class AzureStorageTests : InMemoryStorageFixture
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
}