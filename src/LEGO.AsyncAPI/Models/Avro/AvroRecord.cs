// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroRecord : AvroFieldType
    {
        public string Type { get; } = "record";

        /// <summary>
        /// The name of the schema. Required for named types. See <a href="https://avro.apache.org/docs/1.9.0/spec.html#names">Avro Names</a>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The namespace of the schema. Useful for named types to avoid name conflicts.
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// Documentation for the schema.
        /// </summary>
        public string? Doc { get; set; }

        public IList<string> Aliases { get; set; } = new List<string>();

        public IList<AvroField> Fields { get; set; } = new List<AvroField>();

        public IDictionary<string, AvroFieldType> Metadata { get; set; } = new Dictionary<string, AvroFieldType>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteRequiredProperty("namespace", this.Namespace);
            writer.WriteOptionalProperty("doc", this.Doc);
            writer.WriteOptionalCollection("aliases", this.Aliases, (w, s) => w.WriteValue(s));
            writer.WriteRequiredCollection("fields", this.Fields, (w, s) => s.SerializeV2(w));
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
                        item.Value.SerializeV2(writer);
                    }
                }
            }
            writer.WriteEndObject();
        }
    }
}