// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiAvroSchemaPayload : IAsyncApiMessagePayload
    {
        private AvroSchema schema;

        public AsyncApiAvroSchemaPayload()
        {
            this.schema = new AvroSchema();
        }

        public AsyncApiAvroSchemaPayload(AvroSchema schema)
        {
            this.schema = schema;
        }

        /// <summary>
        /// The type of the schema. See <a href="https://avro.apache.org/docs/1.9.0/spec.html#schema_primitive">Avro Schema Types</a>.
        /// </summary>
        public AvroSchemaType Type { get => this.schema.Type; set => this.schema.Type = value; }

        /// <summary>
        /// The name of the schema. Required for named types (e.g., record, enum, fixed). See <a href="https://avro.apache.org/docs/1.9.0/spec.html#names">Avro Names</a>.
        /// </summary>
        public string Name { get => this.schema.Name; set => this.schema.Name = value; }

        /// <summary>
        /// The namespace of the schema. Useful for named types to avoid name conflicts.
        /// </summary>
        public string? Namespace { get => this.schema.Namespace; set => this.schema.Namespace = value; }

        /// <summary>
        /// Documentation for the schema.
        /// </summary>
        public string? Doc { get => this.schema.Doc; set => this.schema.Doc = value; }

        /// <summary>
        /// The list of fields in the schema.
        /// </summary>
        public IList<AvroField> Fields { get => this.schema.Fields; set => this.schema.Fields = value; }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            this.schema.SerializeV2(writer);
        }
    }
}
