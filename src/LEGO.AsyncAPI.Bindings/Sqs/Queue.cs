using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Bindings.Sqs
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using Extensions;

    public class Queue : IAsyncApiExtensible
    {
        /// <summary>
        /// Allows for an external definition of a queue. The referenced structure MUST be in the format of a Queue. If there are conflicts between the referenced definition and this Queue's definition, the behavior is undefined.
        /// </summary>
        public string DollarRef { get; set; }

        /// <summary>
        /// The name of the queue. When an SNS Operation Binding Object references an SQS queue by name, the identifier should be the one in this field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is this a FIFO queue?
        /// </summary>
        public bool FifoQueue { get; set; }

        /// <summary>
        /// The number of seconds to delay before a message sent to the queue can be received. used to create a delay queue.
        /// </summary>
        public int? DeliveryDelay { get; set; }

        /// <summary>
        /// The length of time, in seconds, that a consumer locks a message - hiding it from reads - before it is unlocked and can be read again.
        /// </summary>
        public int? VisibilityTimeout { get; set; }

        /// <summary>
        /// Determines if the queue uses short polling or long polling. Set to zero the queue reads available messages and returns immediately. Set to a non-zero integer, long polling waits the specified number of seconds for messages to arrive before returning.
        /// </summary>
        public int? ReceiveMessageWaitTime { get; set; }

        /// <summary>
        /// How long to retain a message on the queue in seconds, unless deleted.
        /// </summary>
        public int? MessageRetentionPeriod { get; set; }

        /// <summary>
        /// Prevent poison pill messages by moving un-processable messages to an SQS dead letter queue.
        /// </summary>
        public RedrivePolicy RedrivePolicy { get; set; }

        /// <summary>
        /// The security policy for the SQS Queue
        /// </summary>
        public Policy Policy { get; set; }

        /// <summary>
        /// Key-value pairs that represent AWS tags on the topic.
        /// </summary>
        public Dictionary<string, string> Tags { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public static FixedFieldMap<Queue> fixedFieldMap => new()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "fifoQueue", (a, n) => { a.FifoQueue = n.GetBooleanValue(); } },
            { "deliveryDelay", (a, n) => { a.DeliveryDelay = n.GetIntegerValue(); } },
            { "visibilityTimeout", (a, n) => { a.VisibilityTimeout = n.GetIntegerValue(); } },
            { "receiveMessageWaitTime", (a, n) => { a.ReceiveMessageWaitTime = n.GetIntegerValue(); } },
            { "messageRetentionPeriod", (a, n) => { a.MessageRetentionPeriod = n.GetIntegerValue(); } },
            // temporarily removed for testing extensibility
            // { "redrivePolicy", (a, n) => { a.RedrivePolicy = n.ParseMap(this.redrivePolicyFixedFields); } },
            // { "policy", (a, n) => { a.Policy = n.ParseMap(this.policyFixedFields); } },
            { "tags", (a, n) => { a.Tags = n.CreateSimpleMap(s => s.GetScalarValue()); } },
        };
        
        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalProperty(AsyncApiConstants.DollarRef, this.DollarRef);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteOptionalProperty("fifoQueue", this.FifoQueue);
            writer.WriteOptionalProperty("deliveryDelay", this.DeliveryDelay);
            writer.WriteOptionalProperty("visibilityTimeout", this.VisibilityTimeout);
            writer.WriteOptionalProperty("receiveMessageWaitTime", this.ReceiveMessageWaitTime);
            writer.WriteOptionalProperty("messageRetentionPeriod", this.MessageRetentionPeriod);
            writer.WriteOptionalObject("redrivePolicy", this.RedrivePolicy, (w, p) => p.Serialize(w));
            writer.WriteOptionalObject("policy", this.Policy, (w, p) => p.Serialize(w));
            writer.WriteOptionalMap("tags", this.Tags, (w, t) => w.WriteValue(t));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }

        public static Queue LoadQueue(ParseNode node)
        {
            var mapNode = node.CheckMapNode("queue");

            var queue = new Queue();
            foreach (var property in mapNode)
            {
                property.ParseField(queue, fixedFieldMap, ExtensionHelpers.GetExtensionsFieldMap<Queue>());
            }

            return queue;
        }
    }
}