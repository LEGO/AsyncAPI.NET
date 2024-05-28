﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using System.Collections.Generic;

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

        /// <summary>
        /// An array of strings, providing alternate names for this record (optional).
        /// </summary>
        public IList<string> Aliases { get; set; } = new List<string>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("name", this.Name);
            writer.WriteOptionalObject("type", this.Type, (w, s) => s.SerializeV2(w));
            writer.WriteOptionalProperty("doc", this.Doc);
            writer.WriteOptionalObject("default", this.Default, (w, s) => w.WriteAny(s));
            writer.WriteOptionalProperty("order", this.Order);
            writer.WriteOptionalCollection("aliases", this.Aliases, (w, s) => w.WriteValue(s));
            writer.WriteEndObject();
        }
    }
}