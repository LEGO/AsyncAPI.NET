using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Attributes;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings.Sns;

public class Statement : IAsyncApiElement
{
    public Effect Effect { get; set; }

    /// <summary>
    /// The AWS account or resource ARN that this statement applies to.
    /// </summary>
    // public StringOrStringList Principal { get; set; }
    public StringOrStringList Principal { get; set; }

    /// <summary>
    /// The SNS permission being allowed or denied e.g. sns:Publish
    /// </summary>
    public StringOrStringList Action { get; set; }

    public void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        writer.WriteOptionalProperty(AsyncApiConstants.Effect, this.Effect.GetDisplayName());
        writer.WriteOptionalObject(AsyncApiConstants.Principal, this.Principal, (w, t) => t.Serialize(w));
        writer.WriteOptionalObject(AsyncApiConstants.Action, this.Action, (w, t) => t.Serialize(w));
        writer.WriteEndObject();
    }
}

public enum Effect
{
    [Display("allow")]
    Allow,
    [Display("deny")]
    Deny,
}

public class StringOrStringList : IAsyncApiElement
{
    public string StringValue { get; set; }

    public List<string> StringList { get; set; }

    public void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        if (this.StringValue != null)
        {
            writer.WriteValue(this.StringValue);
        }
        else
        {
            writer.WriteStartArray();
            foreach (var v in this.StringList)
            {
                writer.WriteValue(v);
            }

            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}