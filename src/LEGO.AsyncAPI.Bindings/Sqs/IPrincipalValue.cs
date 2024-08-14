// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Sqs;

using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Writers;

public interface IPrincipalValue
{
    void Serialize(IAsyncApiWriter writer);
}

public struct PrincipalObject : IPrincipalValue
{
    private KeyValuePair<string, StringOrStringList> PrincipalValue;

    public PrincipalObject(KeyValuePair<string, StringOrStringList> principalValue)
    {
        this.PrincipalValue = principalValue;
    }

    public void Serialize(IAsyncApiWriter writer)
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

struct PrincipalStar : IPrincipalValue
{
    private string PrincipalValue;

    public PrincipalStar()
    {
        this.PrincipalValue = "*";
    }

    public void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteValue(this.PrincipalValue);
    }
}