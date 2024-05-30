// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroEnum : AvroSchema
    {
        public override string Type { get; } = "enum";

        public string Name { get; set; }

        public string Namespace { get; set; }

        public string Doc { get; set; }

        public IList<string> Aliases { get; set; } = new List<string>();

        public IList<string> Symbols { get; set; } = new List<string>();

        public string Default { get; set; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public IDictionary<string, AvroSchema> Metadata { get; set; } = new Dictionary<string, AvroSchema>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteOptionalProperty("namespace", this.Namespace);
            writer.WriteOptionalCollection("aliases", this.Aliases, (w, s) => w.WriteValue(s));
            writer.WriteOptionalProperty("doc", this.Doc);
            writer.WriteRequiredCollection("symbols", this.Symbols, (w, s) => w.WriteValue(s));
            writer.WriteRequiredProperty("default", this.Default);
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