// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AvroRecord : AvroFieldType
    {
        public string Type { get; set; } = "record";

        public string Name { get; set; }

        public IList<AvroField> Fields { get; set; } = new List<AvroField>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WritePropertyName("fields");
            writer.WriteStartArray();
            foreach (var field in this.Fields)
            {
                field.SerializeV2(writer);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}