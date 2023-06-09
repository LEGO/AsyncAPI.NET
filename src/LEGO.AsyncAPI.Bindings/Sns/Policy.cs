namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System.Collections.Generic;
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class Policy : IAsyncApiExtensible
    {
        /// <summary>
        /// An array of statement objects, each of which controls a permission for this topic.
        /// </summary>
        public List<Statement> Statements { get; set; }
        
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalCollection("statements", this.Statements, (w, t) => t.Serialize(w));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}