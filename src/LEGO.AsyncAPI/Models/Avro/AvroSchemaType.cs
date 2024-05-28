// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Attributes;

    /// <summary>
    /// Enumeration of Avro schema types. See <a href="https://avro.apache.org/docs/1.9.0/spec.html#schemas">Avro Schemas</a>.
    /// </summary>
    public enum AvroSchemaType
    {
        [Display("record")]
        Record,

        [Display("enum")]
        Enum,

        [Display("fixed")]
        Fixed
    }
}