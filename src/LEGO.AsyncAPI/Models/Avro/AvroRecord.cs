// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
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
            writer.WriteRequiredCollection("fields", this.Fields, (w, s) => s.SerializeV2(w));
            writer.WriteEndObject();
        }
    }
}