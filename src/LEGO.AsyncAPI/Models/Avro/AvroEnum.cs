// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroEnum : AvroSchema
    {
        public override string Type { get; } = "enum";

        /// <summary>
        /// The name of the schema. Required for named types. See <a href="https://avro.apache.org/docs/1.9.0/spec.html#names">Avro Names</a>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The namespace of the schema. Useful for named types to avoid name conflicts.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Documentation for the schema.
        /// </summary>
        public string Doc { get; set; }

        /// <summary>
        /// Alternate names for this enum.
        /// </summary>
        public IList<string> Aliases { get; set; } = new List<string>();

        /// <summary>
        /// Listing symbols. All symbols in an enum must be unique.
        /// </summary>
        public IList<string> Symbols { get; set; } = new List<string>();

        /// <summary>
        /// A default value for this enumeration.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public override IDictionary<string, AsyncApiAny> Metadata { get; set; } = new Dictionary<string, AsyncApiAny>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteOptionalProperty("namespace", this.Namespace);
            writer.WriteOptionalCollection("aliases", this.Aliases, (w, s) => w.WriteValue(s));
            writer.WriteOptionalProperty("doc", this.Doc);
            writer.WriteRequiredCollection("symbols", this.Symbols, (w, s) => w.WriteValue(s));
            writer.WriteOptionalProperty("default", this.Default);
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