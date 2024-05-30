// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Writers;
    using System.Collections.Generic;
    using System.Linq;

    public class AvroFixed : AvroSchema
    {
        public override string Type { get; } = "fixed";

        public string Name { get; set; }

        public string? Namespaace { get; set; }

        public IList<string> Aliases { get; set; } = new List<string>();

        public int Size { get; set; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public IDictionary<string, AvroSchema> Metadata { get; set; } = new Dictionary<string, AvroSchema>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteOptionalProperty("namespace", this.Namespaace);
            writer.WriteOptionalCollection("aliases", this.Aliases, (w, s) => w.WriteValue(s));
            writer.WriteRequiredProperty("size", this.Size);
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