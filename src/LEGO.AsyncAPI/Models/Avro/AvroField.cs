// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Represents a field within an Avro record schema.
    /// </summary>
    public class AvroField : IAsyncApiSerializable
    {
        /// <summary>
        /// The name of the field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the field. Can be a primitive type, a complex type, or a union.
        /// </summary>
        public AvroSchema Type { get; set; }

        /// <summary>
        /// The documentation for the field.
        /// </summary>
        public string Doc { get; set; }

        /// <summary>
        /// The default value for the field.
        /// </summary>
        public AsyncApiAny Default { get; set; }

        /// <summary>
        /// The order of the field, can be 'ascending', 'descending', or 'ignore'.
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Alternate names for this record (optional).
        /// </summary>
        public IList<string> Aliases { get; set; } = new List<string>();

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public IDictionary<string, AsyncApiAny> Metadata { get; set; } = new Dictionary<string, AsyncApiAny>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("name", this.Name);
            writer.WriteOptionalObject("type", this.Type, (w, s) => s.SerializeV2(w));
            writer.WriteOptionalProperty("doc", this.Doc);
            writer.WriteOptionalObject("default", this.Default, (w, s) => w.WriteAny(s));
            writer.WriteOptionalProperty("order", this.Order);
            writer.WriteOptionalCollection("aliases", this.Aliases, (w, s) => w.WriteValue(s));
            if (this.Metadata.Any())
            {
                foreach (var item in this.Metadata)
                {
                    writer.WritePropertyName(item.Key);
                    if (item.Value == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.WriteAny(item.Value);
                    }
                }
            }

            writer.WriteEndObject();
        }
    }
}