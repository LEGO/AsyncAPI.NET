namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using System.Collections.Generic;


    public class FilterPolicy : IAsyncApiExtensible
    {
        /// <summary>
        /// A map of a message attribute to an array of possible matches. The match may be a simple string for an exact match, but it may also be an object that represents a constraint and values for that constraint.
        /// </summary>
        public IAsyncApiAny Attributes { get; set; }
        
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredObject("attributes", this.Attributes, (w, a) => w.WriteAny(a));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}