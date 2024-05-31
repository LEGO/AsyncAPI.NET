// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroPrimitive : AvroSchema
    {
        public override string Type { get; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public override IDictionary<string, AsyncApiAny> Metadata { get; set; } = new Dictionary<string, AsyncApiAny>();

        public AvroPrimitive(AvroPrimitiveType type)
        {
            this.Type = type.GetDisplayName();
        }

        public AvroPrimitive()
        {
        }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteValue(this.Type);
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
        }
    }
}