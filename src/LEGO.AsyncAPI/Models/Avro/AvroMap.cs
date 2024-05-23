// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Writers;

    public class AvroMap : AvroFieldType
    {
        public string Type { get; } = "map";

        public AvroFieldType Values { get; set; }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredObject("values", this.Values, (w, f) => f.SerializeV2(w));
            writer.WriteEndObject();
        }
    }
}