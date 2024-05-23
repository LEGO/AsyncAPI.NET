// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AvroMap : AvroFieldType
    {
        public string Type { get; set; } = "map";

        public AvroFieldType Values { get; set; }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WritePropertyName("values");
            this.Values.SerializeV2(writer);
            writer.WriteEndObject();
        }
    }
}