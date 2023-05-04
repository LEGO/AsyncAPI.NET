using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Models.Bindings.Sns;

using LEGO.AsyncAPI.Models.Interfaces;

/// <summary>
/// Binding class for SNS channel settings.
/// </summary>
public class SnsChannelBinding : IChannelBinding
{
    /// <summary>
    /// The name of the topic. Can be different from the channel name to allow flexibility around AWS resource naming limitations.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// By default, we assume an unordered SNS topic. This field allows configuration of a FIFO SNS Topic.
    /// </summary>
    public OrderingConfiguration Ordering { get; set; }

    /// <summary>
    /// The security policy for the SNS Topic.
    /// </summary>
    public Policy Policy { get; set; }

    /// <summary>
    /// Key-value pairs that represent AWS tags on the topic.
    /// </summary>
    public Dictionary<string, string> Tags { get; set; }

    /// <summary>
    /// The version of this binding.
    /// </summary>
    public string BindingVersion { get; set; }

    public BindingType Type => BindingType.Sns;

    public bool UnresolvedReference { get; set; }

    public AsyncApiReference Reference { get; set; }

    public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

    public void SerializeV2WithoutReference(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }
        
        writer.WriteStartObject();
        writer.WriteOptionalProperty(AsyncApiConstants.Name, this.Name);
        writer.WriteOptionalObject(AsyncApiConstants.Policy, this.Policy, (w, t) => t.Serialize(w));

        writer.WriteEndObject();
    }

    public void SerializeV2(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }
        
        if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
        {
            this.Reference.SerializeV2(writer);
            return;
        }

        this.SerializeV2WithoutReference(writer);
        
    }
}