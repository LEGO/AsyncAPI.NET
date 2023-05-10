using System;
using LEGO.AsyncAPI.Attributes;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings.Sns;

public class Consumer : IAsyncApiElement
{
    /// <summary>
    /// What protocol will this endpoint receive messages by?
    /// </summary>
    public Protocol Protocol { get; set; }

    /// <summary>
    /// Where are messages being delivered to?
    /// </summary>
    public Identifier Endpoint { get; set; }

    /// <summary>
    /// Only receive a subset of messages from the channel, determined by this policy.
    /// </summary>
    public FilterPolicy FilterPolicy { get; set; }

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
    /// The display name to use with an SNS subscription
    /// </summary>
    public string DisplayName { get; set; }

    public void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        writer.WriteRequiredProperty("protocol", this.Protocol.GetDisplayName());
        writer.WriteRequiredObject("endpoint", this.Endpoint, (w, e) => e.Serialize(w));
        writer.WriteOptionalObject("filterPolicy", this.FilterPolicy, (w, f) => f.Serialize(w));
        writer.WriteRequiredProperty("rawMessageDelivery", this.RawMessageDelivery);
        writer.WriteOptionalObject("redrivePolicy", this.RedrivePolicy, (w, p) => p.Serialize(w));
        writer.WriteOptionalObject("deliveryPolicy", this.DeliveryPolicy, (w, p) => p.Serialize(w));
        writer.WriteOptionalProperty("displayName", this.DisplayName);
        writer.WriteEndObject();
    }
}

public enum Protocol
{
    [Display("http")]
    Http,
    [Display("https")]
    Https,
    [Display("email")]
    Email,
    [Display("email-json")]
    EmailJson,
    [Display("sms")]
    Sms,
    [Display("sqs")]
    Sqs,
    [Display("application")]
    Application,
    [Display("lambda")]
    Lambda,
    [Display("firehose")]
    Firehose,
}