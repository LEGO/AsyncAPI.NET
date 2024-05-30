// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiAvroSchemaPayload : IAsyncApiMessagePayload
    {
        public AvroSchema Schema { get; set; }

        public AsyncApiAvroSchemaPayload(AvroSchema schema)
        {
            this.Schema = schema;
        }

        public AsyncApiAvroSchemaPayload()
        {
        }

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            var settings = writer.GetSettings();

            if (this.Reference != null)
            {
                if (!settings.ShouldInlineReference(this.Reference))
                {
                    this.Reference.SerializeV2(writer);
                    return;
                }
            }

            this.SerializeV2WithoutReference(writer);
        }

        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            this.Schema.SerializeV2(writer);
        }
    }
}
