// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AvroUnion : AvroFieldType
    {
        public IList<AvroFieldType> Types { get; set; } = new List<AvroFieldType>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartArray();
            foreach (var type in this.Types)
            {
                writer.WriteStartObject();
                type.SerializeV2(writer);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}