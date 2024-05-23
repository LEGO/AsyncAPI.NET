// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Represents an Avro schema model (compatible with Avro 1.9.0).
    /// </summary>
    public class AvroSchema : IAsyncApiSerializable
    {
        /// <summary>
        /// The type of the schema. See <a href="https://avro.apache.org/docs/1.9.0/spec.html#schema_primitive">Avro Schema Types</a>.
        /// </summary>
        public AvroSchemaType Type { get; set; }

        /// <summary>
        /// The name of the schema. Required for named types (e.g., record, enum, fixed). See <a href="https://avro.apache.org/docs/1.9.0/spec.html#names">Avro Names</a>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The namespace of the schema. Useful for named types to avoid name conflicts.
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// Documentation for the schema.
        /// </summary>
        public string? Doc { get; set; }

        /// <summary>
        /// The list of fields in the schema.
        /// </summary>
        public IList<AvroField> Fields { get; set; } = new List<AvroField>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // type
            writer.WriteOptionalProperty(AsyncApiConstants.Type, this.Type.GetDisplayName());

            // name
            writer.WriteOptionalProperty("name", this.Name);

            // namespace
            writer.WriteOptionalProperty("namespace", this.Namespace);

            // doc
            writer.WriteOptionalProperty("doc", this.Doc);

            // fields
            writer.WriteOptionalCollection("fields", this.Fields, (w, f) => f.SerializeV2(w));

            writer.WriteEndObject();
        }
    }
}