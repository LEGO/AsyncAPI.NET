// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroArray : AsyncApiAvroSchema
    {
        public override string Type { get; } = "array";

        /// <summary>
        /// The schema of the array's items.
        /// </summary>
        public AsyncApiAvroSchema Items { get; set; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public override IDictionary<string, AsyncApiAny> Metadata { get; set; } = new Dictionary<string, AsyncApiAny>();

        public override void SerializeV2Core(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredObject("items", this.Items, (w, f) => f.SerializeV2(w));
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