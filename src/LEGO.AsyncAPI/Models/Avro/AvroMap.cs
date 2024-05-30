// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroMap : AvroSchema
    {
        public override string Type { get; } = "map";

        public AvroPrimitiveType Values { get; set; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public IDictionary<string, AvroSchema> Metadata { get; set; } = new Dictionary<string, AvroSchema>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("values", this.Values.GetDisplayName());
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