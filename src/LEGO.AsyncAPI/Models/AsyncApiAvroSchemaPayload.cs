// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
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

        public bool TryGetAs<T>(out T schema)
            where T : AvroSchema
        {
            schema = this.Schema as T;
            return schema != null;
        }

        public bool UnresolvedReference { get => this.Schema.UnresolvedReference; set => this.Schema.UnresolvedReference = value; }

        public AsyncApiReference Reference { get => this.Schema.Reference; set => this.Schema.Reference = value; }

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
