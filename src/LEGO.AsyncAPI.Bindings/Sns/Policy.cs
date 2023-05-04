using System;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings.Sns;

using System.Collections.Generic;

public class Policy : IAsyncApiElement
{
    /// <summary>
    /// An array of statement objects, each of which controls a permission for this topic.
    /// </summary>
    public List<Statement> Statements { get; set; }
    
    public void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        writer.WriteOptionalCollection(AsyncApiConstants.Statements, this.Statements, (w, t) => t.Serialize(w));
        writer.WriteEndObject();
    }
}