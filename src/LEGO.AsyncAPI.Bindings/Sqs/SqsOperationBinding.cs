namespace LEGO.AsyncAPI.Bindings.Sqs
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    public class SqsOperationBinding : OperationBinding<SqsOperationBinding>
    {
        /// <summary>
        /// Queue objects that are either the endpoint for an SNS Operation Binding Object, or the deadLetterQueue of the SQS Operation Binding Object
        /// </summary>
        public List<Queue> Queues { get; set; }

        public override string BindingKey => "sqs";

        protected override FixedFieldMap<SqsOperationBinding> FixedFieldMap => new()
        {
            { "queues", (a, n) => { a.Queues = n.CreateList(s => s.ParseMapWithExtensions(this.queueFixedFields)); } },
        };

        private FixedFieldMap<Queue> queueFixedFields => new()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "fifoQueue", (a, n) => { a.FifoQueue = n.GetBooleanValue(); } },
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
            { "principal", (a, n) => { a.Principal = StringOrStringList.Parse(n); } },
            { "action", (a, n) => { a.Action = StringOrStringList.Parse(n); } },
        };

        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredCollection("queues", this.Queues, (w, t) => t.Serialize(w));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}