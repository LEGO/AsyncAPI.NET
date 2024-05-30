// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroUnion : AvroSchema
    {
        public override string Type { get; } = "map";

        public IList<AvroSchema> Types { get; set; } = new List<AvroSchema>();

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public IDictionary<string, AvroSchema> Metadata { get; set; } = new Dictionary<string, AvroSchema>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartArray();
            foreach (var type in this.Types)
            {
                type.SerializeV2(writer);
            }

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
            writer.WriteEndArray();
        }
    }
}