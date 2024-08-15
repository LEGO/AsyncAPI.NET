namespace LEGO.AsyncAPI.Bindings.Sns;

using System;
using LEGO.AsyncAPI.Writers;

public class PrincipalStar : Principal
{
    private string PrincipalValue;

    public PrincipalStar()
    {
        this.PrincipalValue = "*";
    }

    public override void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteValue(this.PrincipalValue);
    }
}