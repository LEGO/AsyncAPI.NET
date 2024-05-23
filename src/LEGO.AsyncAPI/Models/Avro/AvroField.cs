// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Represents a field within an Avro record schema.
    /// </summary>
    public class AvroField : IAsyncApiSerializable
    {
        /// <summary>
        /// The name of the field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the field. Can be a primitive type, a complex type, or a union.
        /// </summary>
        public AvroFieldType Type { get; set; }

        /// <summary>
        /// The documentation for the field.
        /// </summary>
        public string Doc { get; set; }

        /// <summary>
        /// The default value for the field.
        /// </summary>
        public AsyncApiAny Default { get; set; }

        /// <summary>
        /// The order of the field, can be 'ascending', 'descending', or 'ignore'.
        /// </summary>
        public string Order { get; set; }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // name
            writer.WriteOptionalProperty("name", this.Name);

            // type
            writer.WriteOptionalObject("type", this.Type, (w, s) => s.SerializeV2(w));

            // doc
            writer.WriteOptionalProperty("doc", this.Doc);

            // default
            writer.WriteOptionalObject("default", this.Default, (w, s) => w.WriteAny(s));

            // order
            writer.WriteOptionalProperty("order", this.Order);

            writer.WriteEndObject();
        }
    }
}