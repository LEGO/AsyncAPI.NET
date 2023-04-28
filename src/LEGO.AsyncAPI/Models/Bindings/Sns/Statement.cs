using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Models.Bindings.Sns;

public class Statement : IAsyncApiElement
{
    public Effect Effect { get; set; }

    /// <summary>
    /// The AWS account or resource ARN that this statement applies to.
    /// </summary>
    // public StringOrStringList Principal { get; set; }
    public string Principal { get; set; }

    /// <summary>
    /// The SNS permission being allowed or denied e.g. sns:Publish
    /// </summary>
    // public StringOrStringList Action { get; set; }

    public void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        // writer.WriteOptionalObject(AsyncApiConstants.Principal, this.Principal, (w, t) => t.Serialize(w));
        writer.WriteOptionalProperty(AsyncApiConstants.Principal, this.Principal);
        writer.WriteEndObject();
    }
}

public enum Effect
{
    Allow,
    Deny
}

public class StringOrStringList : IAsyncApiElement
{
    public string StringValue { get; set; }

    public List<string> StringList { get; set; }
}