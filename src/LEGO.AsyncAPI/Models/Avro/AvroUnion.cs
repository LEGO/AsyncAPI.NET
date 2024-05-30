// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Writers;

    public class AvroUnion : AvroSchema
    {
        public IList<AvroSchema> Types { get; set; } = new List<AvroSchema>();

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartArray();
            foreach (var type in this.Types)
            {
                type.SerializeV2(writer);
            }
            writer.WriteEndArray();
        }
    }
}