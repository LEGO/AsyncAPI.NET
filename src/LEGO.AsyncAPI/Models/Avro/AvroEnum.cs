// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Writers;

    public class AvroEnum : AvroFieldType
    {
        public string Type { get; } = "enum";

        public string Name { get; set; }

        public string Doc { get; set; }

        public IList<string> Aliases { get; set; } = new List<string>();

        public IList<string> Symbols { get; set; } = new List<string>();

        public string Default { get; set; }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteOptionalCollection("aliases", this.Aliases, (w, s) => w.WriteValue(s));
            writer.WriteOptionalProperty("doc", this.Doc);
            writer.WriteRequiredCollection("symbols", this.Symbols, (w, s) => w.WriteValue(s));
            writer.WriteRequiredProperty("default", this.Default);
            writer.WriteEndObject();
        }
    }
}