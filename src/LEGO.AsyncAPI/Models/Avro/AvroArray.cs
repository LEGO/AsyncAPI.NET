// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Writers;

    public class AvroArray : AvroSchema
    {
        public string Type { get; } = "array";

        public AvroSchema Items { get; set; }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredObject("items", this.Items, (w, f) => f.SerializeV2(w));
            writer.WriteEndObject();
        }
    }
}