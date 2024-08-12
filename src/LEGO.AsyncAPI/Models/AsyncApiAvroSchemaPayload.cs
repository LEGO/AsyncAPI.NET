// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using System;

    public class AsyncApiAvroSchemaPayload : IAsyncApiMessagePayload
    {
        private readonly AvroSchema schema;

        public AsyncApiAvroSchemaPayload(AvroSchema schema)
        {
            this.schema = schema;
        }

        public AsyncApiAvroSchemaPayload()
        {
            this.schema = new AvroRecord();
        }

        public bool TryGetAs<T>(out T schema)
            where T : AvroSchema
        {
            schema = this.schema as T;
            return schema != null;
        }

        public bool Is<T>()
            where T : AvroSchema
        {
            return this.schema is T;
        }

        public Type GetSchemaType()
        {
            return this.schema.GetType();
        }

        public bool UnresolvedReference { get => this.schema.UnresolvedReference; set => this.schema.UnresolvedReference = value; }

        public AsyncApiReference Reference { get => this.schema.Reference; set => this.schema.Reference = value; }

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
            this.schema.SerializeV2(writer);
        }
    }
}
