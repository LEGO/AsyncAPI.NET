namespace LEGO.AsyncAPI.Bindings.Sqs;

using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Writers;

public class PrincipalObject : Principal
{
    public KeyValuePair<string, StringOrStringList> Value { get; private set; }

    public PrincipalObject(KeyValuePair<string, StringOrStringList> value)
    {
        this.Value = value;
    }

    public override void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        writer.WriteRequiredObject(this.Value.Key, this.Value.Value, (w, t) => t.Value.Write(w));
        writer.WriteEndObject();
    }
}