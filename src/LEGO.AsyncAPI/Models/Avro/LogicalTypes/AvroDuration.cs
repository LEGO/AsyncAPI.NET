// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public class AvroDuration : AvroFixed
    {
        public LogicalType LogicalType { get; } = LogicalType.Duration;

        public new int Size { get; } = 12;

        public override void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteOptionalProperty("logicalType", this.LogicalType.GetDisplayName());
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteOptionalProperty("namespace", this.Namespace);
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
                        writer.WriteAny(item.Value);
                    }
                }
            }

            writer.WriteEndObject();
        }
    }
}
