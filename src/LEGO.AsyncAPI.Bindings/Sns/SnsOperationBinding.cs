namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// This object contains information about the operation representation in SNS.
    /// </summary>
    public class SnsOperationBinding : OperationBinding<SnsOperationBinding>
    {
        /// <summary>
        /// Often we can assume that the SNS Topic is the channel name-we provide this field in case the you need to supply the ARN, or the Topic name is not the channel name in the AsyncAPI document.
        /// </summary>
        public Identifier Topic { get; set; }

        /// <summary>
        /// The protocols that listen to this topic and their endpoints.
        /// </summary>
        public List<Consumer> Consumers { get; set; }

        /// <summary>
        /// Policy for retries to HTTP. The field is the default for HTTP receivers of the SNS Topic which may be overridden by a specific consumer.
        /// </summary>
        public DeliveryPolicy DeliveryPolicy { get; set; }

        public override string BindingKey => "sns";

        protected override FixedFieldMap<SnsOperationBinding> FixedFieldMap => new()
        {
            { "topic", (a, n) => { a.Topic = n.ParseMapWithExtensions(this.identifierFixFields); } },
            { "consumers", (a, n) => { a.Consumers = n.CreateList(s => s.ParseMapWithExtensions(this.consumerFixedFields)); } },
            { "deliveryPolicy", (a, n) => { a.DeliveryPolicy = n.ParseMapWithExtensions(this.deliveryPolicyFixedFields); } },
        };

        private FixedFieldMap<Identifier> identifierFixFields => new()
        {
            { "url", (a, n) => { a.Url = n.GetScalarValue(); } },
            { "email", (a, n) => { a.Email = n.GetScalarValue(); } },
            { "phone", (a, n) => { a.Phone = n.GetScalarValue(); } },
            { "arn", (a, n) => { a.Arn = n.GetScalarValue(); } },
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
        };

        private FixedFieldMap<Consumer> consumerFixedFields => new()
        {
            { "protocol", (a, n) => { a.Protocol = n.GetScalarValue().GetEnumFromDisplayName<Protocol>(); } },
            { "endpoint", (a, n) => { a.Endpoint = n.ParseMapWithExtensions(this.identifierFixFields); } },
            { "filterPolicy", (a, n) => { a.FilterPolicy = n.CreateAny(); } },
            { "filterPolicyScope", (a, n) => { a.FilterPolicyScope = n.GetScalarValue().GetEnumFromDisplayName<FilterPolicyScope>(); } },
            { "rawMessageDelivery", (a, n) => { a.RawMessageDelivery = n.GetBooleanValue(); } },
            { "redrivePolicy", (a, n) => { a.RedrivePolicy = n.ParseMapWithExtensions(this.redrivePolicyFixedFields); } },
            { "deliveryPolicy", (a, n) => { a.DeliveryPolicy = n.ParseMapWithExtensions(this.deliveryPolicyFixedFields); } },
            { "displayName", (a, n) => { a.DisplayName = n.GetScalarValue(); } },
        };

        private FixedFieldMap<RedrivePolicy> redrivePolicyFixedFields => new()
        {
            { "deadLetterQueue", (a, n) => { a.DeadLetterQueue = n.ParseMapWithExtensions(identifierFixFields); } },
            { "maxReceiveCount", (a, n) => { a.MaxReceiveCount = n.GetIntegerValue(); } },
        };

        private FixedFieldMap<DeliveryPolicy> deliveryPolicyFixedFields => new()
        {
            { "minDelayTarget", (a, n) => { a.MinDelayTarget = n.GetIntegerValue(); } },
            { "maxDelayTarget", (a, n) => { a.MaxDelayTarget = n.GetIntegerValue(); } },
            { "numRetries", (a, n) => { a.NumRetries = n.GetIntegerValue(); } },
            { "numNoDelayRetries", (a, n) => { a.NumNoDelayRetries = n.GetIntegerValue(); } },
            { "numMinDelayRetries", (a, n) => { a.NumMinDelayRetries = n.GetIntegerValue(); } },
            { "numMaxDelayRetries", (a, n) => { a.NumMaxDelayRetries = n.GetIntegerValue(); } },
            { "backoffFunction", (a, n) => { a.BackoffFunction = n.GetScalarValue().GetEnumFromDisplayName<BackoffFunction>(); } },
            { "maxReceivesPerSecond", (a, n) => { a.MaxReceivesPerSecond = n.GetIntegerValue(); } },
        };

        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalObject("topic", this.Topic, (w, t) => t.Serialize(w));
            writer.WriteOptionalCollection("consumers", this.Consumers, (w, c) => c.Serialize(w));
            writer.WriteOptionalObject("deliveryPolicy", this.DeliveryPolicy, (w, p) => p.Serialize(w));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}