namespace LEGO.AsyncAPI.Bindings.Sqs;

using System;
using LEGO.AsyncAPI.Writers;

public class PrincipalStar : Principal
{
    public string Value { get; private set; }

    public PrincipalStar()
    {
        this.Value = "*";
    }

    public override void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteValue(this.Value);
    }
}