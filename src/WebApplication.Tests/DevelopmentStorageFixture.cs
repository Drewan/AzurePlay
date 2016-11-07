using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lokad.Cloud.Storage;

namespace WebApplication.Tests
{
    public class DevelopmentStorageFixture
    {
        protected const string ContainerName = "events-container";
        protected const string BlobName = "myprefix/myblob";

        public DevelopmentStorageFixture()
        {
            Storage = CloudStorage
                .ForDevelopmentStorage()
                .BuildBlobStorage();

            Storage.CreateContainerIfNotExist(ContainerName);
            Storage.DeleteBlobIfExist(ContainerName, BlobName);
        }

        public IBlobStorageProvider Storage { get; set; }
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
            get { return "events-container"; }
        }

        [Rank(1, true)]
        public long SequenceId { get; set; }

        [Rank(0)]
        public string RowKey { get; set; }
    }


}
