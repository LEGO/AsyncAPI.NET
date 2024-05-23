// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AvroEnum : AvroFieldType
    {
        public string Type { get; set; } = "enum";

        public string Name { get; set; }

        public IList<string> Symbols { get; set; } = new List<string>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WritePropertyName("symbols");
            writer.WriteStartArray();
            foreach (var symbol in this.Symbols)
            {
                writer.WriteValue(symbol);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}