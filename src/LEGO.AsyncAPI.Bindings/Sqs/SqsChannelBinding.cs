// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Sqs
{
    using System;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// This object contains information about the channel representation in SQS.
    /// </summary>
    public class SqsChannelBinding : ChannelBinding<SqsChannelBinding>
    {
        /// <summary>
        /// A definition of the queue that will be used as the channel.
        /// </summary>
        public Queue Queue { get; set; }

        /// <summary>
        /// A definition of the queue that will be used for un-processable messages.
        /// </summary>
        public Queue DeadLetterQueue { get; set; }

        public override string BindingKey => "sqs";

        protected override FixedFieldMap<SqsChannelBinding> FixedFieldMap => new()
        {
            { "queue", (a, n) => { a.Queue = n.ParseMapWithExtensions(this.queueFixedFields); } },
            { "deadLetterQueue", (a, n) => { a.DeadLetterQueue = n.ParseMapWithExtensions(this.queueFixedFields); } },
        };

        private FixedFieldMap<Queue> queueFixedFields => new()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "fifoQueue", (a, n) => { a.FifoQueue = n.GetBooleanValue(); } },
            { "deduplicationScope", (a, n) => { a.DeduplicationScope = n.GetScalarValue().GetEnumFromDisplayName<DeduplicationScope>(); } },
            { "fifoThroughputLimit", (a, n) => { a.FifoThroughputLimit = n.GetScalarValue().GetEnumFromDisplayName<FifoThroughputLimit>(); } },
            { "deliveryDelay", (a, n) => { a.DeliveryDelay = n.GetIntegerValue(); } },
            { "visibilityTimeout", (a, n) => { a.VisibilityTimeout = n.GetIntegerValue(); } },
            { "receiveMessageWaitTime", (a, n) => { a.ReceiveMessageWaitTime = n.GetIntegerValue(); } },
            { "messageRetentionPeriod", (a, n) => { a.MessageRetentionPeriod = n.GetIntegerValue(); } },
            { "redrivePolicy", (a, n) => { a.RedrivePolicy = n.ParseMapWithExtensions(this.redrivePolicyFixedFields); } },
            { "policy", (a, n) => { a.Policy = n.ParseMapWithExtensions(this.policyFixedFields); } },
            { "tags", (a, n) => { a.Tags = n.CreateSimpleMap(s => s.GetScalarValue()); } },
        };

        private FixedFieldMap<RedrivePolicy> redrivePolicyFixedFields => new()
        {
            { "deadLetterQueue", (a, n) => { a.DeadLetterQueue = n.ParseMapWithExtensions(identifierFixFields); } },
            { "maxReceiveCount", (a, n) => { a.MaxReceiveCount = n.GetIntegerValue(); } },
        };

        private static FixedFieldMap<Identifier> identifierFixFields => new()
        {
            { "arn", (a, n) => { a.Arn = n.GetScalarValue(); } },
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
        };

        private FixedFieldMap<Policy> policyFixedFields = new()
        {
            { "statements", (a, n) => { a.Statements = n.CreateList(s => s.ParseMapWithExtensions(statementFixedFields)); } },
        };

        private static FixedFieldMap<Statement> statementFixedFields = new()
        {
            { "effect", (a, n) => { a.Effect = n.GetScalarValue().GetEnumFromDisplayName<Effect>(); } },
            { "principal", (a, n) => { a.Principal = Principal.Parse(n); } },
            { "action", (a, n) => { a.Action = StringOrStringList.Parse(n); } },
            { "resource", (a, n) => { a.Resource = StringOrStringList.Parse(n); } },
            { "condition", (a, n) => { a.Condition = n.CreateAny(); } },
        };

        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredObject("queue", this.Queue, (w, q) => q.Serialize(w));
            writer.WriteOptionalObject("deadLetterQueue", this.DeadLetterQueue, (w, q) => q.Serialize(w));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}