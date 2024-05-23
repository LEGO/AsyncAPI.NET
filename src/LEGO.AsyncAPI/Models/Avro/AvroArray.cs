// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AvroArray : AvroFieldType
    {
        public string Type { get; set; } = "array";

        public AvroFieldType Items { get; set; }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WritePropertyName("items");
            this.Items.SerializeV2(writer);
            writer.WriteEndObject();
        }
    }
}