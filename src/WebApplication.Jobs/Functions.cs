using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lokad.Cloud.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Serilog;

namespace WebApplication.Jobs
{
    public class Functions
    {
        // BLOB TRIGGERS

        /// Blob triggers are not designed to fire instantly.  Delays can be seconds or minutes.  
        /// Guidance from the WebJobs Team is to use QueueTriggers as they first instantly and retrieve the blob 

        //public static void CopyBlobToReplicationStorage(
        //    [BlobTrigger("events/{name}")] string input,
        //    [Blob("events/{name}"), StorageAccount("AzureWebJobsReplicationStorage")] out string output)
        //{
        //    output = input;

        //    Log.Information("Copied blob to replication storage");
        //}


        // QUEUE TRIGGERS

        //public static void ProcessQueueMessage(
        //    [QueueTrigger("events")] string blobName,
        //    [Blob("events/{name}")] TextReader input,
        //    [Blob("events/{name}"), StorageAccount("AzureWebJobsReplicationStorage")] out string output)
        //{
        //    output = input.ReadToEnd();

        //    Log.Information("Copied blob to replication storage");
        //}

        public static void CopyBlobToReplicationStorage(
            [QueueTrigger("events")] ReplicateEvent command,
            [Blob("events/{BlobName}")] string input,
            [Blob("events/{BlobName}"), StorageAccount("AzureWebJobsReplicationStorage")] out string output)
        {
            output = input;
        }

        public static void LogPoisonBlob(
            [QueueTrigger("webjobs-blobtrigger-poison")] BlobMessage message,
            TextWriter logger)
        {
            Log.Information("Poison blob message {@BlobMessage}", message);
        }
    }

    [DataContract]
    public abstract class Message
    {
        [DataMember]
        public Guid MessageId { get; set; }
    }

    [DataContract]
    public class Event : Message
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Author { get; set; }
    }

    [DataContract]
    public class PaymentTaken : Event
    {
        [DataMember]
        public decimal Amount { get; set; }
    }

    [DataContract]
    public class MessageId : BlobName<Message>
    {
        public override string ContainerName
        {
            get { return "events"; }
        }

        public static MessageId New
        {
            get { return new MessageId(Guid.NewGuid().ToString()); }
        }

        [Rank(0)]
        public string Name { get; set; }

        public MessageId() { }

        public MessageId(string name)
        {
            Name = name;
        }
    }

    [DataContract]
    public class ReplicateEvent
    {
        [DataMember]
        public string BlobName { get; set; }
    }

    public class BlobMessage
    {
        public string FunctionId { get; set; }
        public string BlobType { get; set; }
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public string ETag { get; set; }
    }

    public class EventEnvelope
    {
        public long SequenceId { get; set; }
        public string BlobName { get; set; }
        public Uri BlobUri { get; set; }
        public string Content { get; set; }
    }
}
