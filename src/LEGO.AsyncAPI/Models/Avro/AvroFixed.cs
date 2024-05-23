// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Writers;

    public class AvroFixed : AvroFieldType
    {
        public string Type { get; } = "fixed";

        public string Name { get; set; }

        public int Size { get; set; }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteRequiredProperty("size", this.Size);
            writer.WriteEndObject();
        }
    }
}