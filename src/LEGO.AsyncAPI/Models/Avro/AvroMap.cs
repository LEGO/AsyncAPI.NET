// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Writers;

    public class AvroMap : AvroSchema
    {
        public string Type { get; } = "map";

        public IDictionary<string, string> Values { get; set; } = new Dictionary<string, string>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            Convert.ToBoolean()
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredObject("values", this.Values, (w, f) => f.SerializeV2(w));
            writer.WriteEndObject();
        }
    }
}