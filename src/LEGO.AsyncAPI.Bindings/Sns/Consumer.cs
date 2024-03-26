namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Attributes;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class Consumer : IAsyncApiExtensible
    {
        /// <summary>
        /// The protocol that this endpoint will receive messages by.
        /// </summary>
        public Protocol Protocol { get; set; }

        /// <summary>
        /// The endpoint messages are delivered to.
        /// </summary>
        public Identifier Endpoint { get; set; }

        /// <summary>
        /// Only receive a subset of messages from the channel, determined by this policy.
        /// Depending on the FilterPolicyScope, a map of either a message attribute or message body to an array of possible matches. The match may be a simple string for an exact match, but it may also be an object that represents a constraint and values for that constraint.
        /// </summary>
        public AsyncApiAny FilterPolicy { get; set; }

        /// <summary>
        /// Determines whether the FilterPolicy applies to MessageAttributes or MessageBody.
        /// </summary>
        public FilterPolicyScope FilterPolicyScope { get; set; }

        /// <summary>
        /// If true AWS SNS attributes are removed from the body, and for SQS, SNS message attributes are copied to SQS message attributes. If false the SNS attributes are included in the body.
        /// </summary>
        public bool RawMessageDelivery { get; set; }

        /// <summary>
        /// Prevent poison pill messages by moving un-processable messages to an SQS dead letter queue.
        /// </summary>
        public RedrivePolicy RedrivePolicy { get; set; }

        /// <summary>
        /// Policy for retries to HTTP. The parameter is for that SNS Subscription and overrides any policy on the SNS Topic.
        /// </summary>
        public DeliveryPolicy DeliveryPolicy { get; set; }

        /// <summary>
        /// The display name to use with an SNS subscription.
        /// </summary>
        public string DisplayName { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredProperty("protocol", this.Protocol.GetDisplayName());
            writer.WriteRequiredObject("endpoint", this.Endpoint, (w, e) => e.Serialize(w));
            writer.WriteOptionalObject("filterPolicy", this.FilterPolicy, (w, f) => f.Write(w));
            writer.WriteOptionalProperty("filterPolicyScope", this.FilterPolicyScope.GetDisplayName());
            writer.WriteRequiredProperty("rawMessageDelivery", this.RawMessageDelivery);
            writer.WriteOptionalObject("redrivePolicy", this.RedrivePolicy, (w, p) => p.Serialize(w));
            writer.WriteOptionalObject("deliveryPolicy", this.DeliveryPolicy, (w, p) => p.Serialize(w));
            writer.WriteOptionalProperty("displayName", this.DisplayName);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }

    public enum Protocol
    {
        [Display("http")] Http,
        [Display("https")] Https,
        [Display("email")] Email,
        [Display("email-json")] EmailJson,
        [Display("sms")] Sms,
        [Display("sqs")] Sqs,
        [Display("application")] Application,
        [Display("lambda")] Lambda,
        [Display("firehose")] Firehose,
    }

    public enum FilterPolicyScope
    {
        [Display("MessageAttributes")] MessageAttributes,
        [Display("MessageBody")] MessageBody,
    }
}