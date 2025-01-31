// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroUnion : AsyncApiAvroSchema
    {
        public override string Type { get; } = "map";

        /// <summary>
        /// The types in this union.
        /// </summary>
        public IList<AsyncApiAvroSchema> Types { get; set; } = new List<AsyncApiAvroSchema>();

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public override IDictionary<string, AsyncApiAny> Metadata { get; set; } = new Dictionary<string, AsyncApiAny>();

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
                        writer.WriteAny(item.Value);
                    }
                }
            }

            writer.WriteEndArray();
        }
    }
}