using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings.Sns;

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

    protected override FixedFieldMap<SnsOperationBinding> FixedFieldMap { get; }
    
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
        writer.WriteEndObject();
    }
}