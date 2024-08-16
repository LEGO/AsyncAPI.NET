namespace LEGO.AsyncAPI.Bindings.Sqs;

using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Writers;

public class PrincipalObject : Principal
{
    private KeyValuePair<string, StringOrStringList> PrincipalValue;

    public PrincipalObject(KeyValuePair<string, StringOrStringList> principalValue)
    {
        this.PrincipalValue = principalValue;
    }

    public override void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        writer.WriteRequiredObject(this.PrincipalValue.Key, this.PrincipalValue.Value, (w, t) => t.Value.Write(w));
        writer.WriteEndObject();
    }
}