using Lokad.Cloud.Storage;

namespace WebApplication.Tests
{
    public class InMemoryStorageFixture
    {
        protected const string ContainerName = "events";

        public InMemoryStorageFixture()
        {
            Storage = CloudStorage
                .ForInMemoryStorage()
                .BuildBlobStorage();
            Storage.CreateContainerIfNotExist(ContainerName);
        }

        public IBlobStorageProvider Storage { get; set; }
    }
}