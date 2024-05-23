// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using LEGO.AsyncAPI.Attributes;

    /// <summary>
    /// Enumeration of Avro schema types. See <a href="https://avro.apache.org/docs/1.9.0/spec.html#schemas">Avro Schemas</a>.
    /// </summary>
    [Flags]
    public enum AvroSchemaType
    {
        [Display("null")]
        Null = 1,

        [Display("boolean")]
        Boolean = 2,

        [Display("int")]
        Int = 4,

        [Display("long")]
        Long = 8,

        [Display("float")]
        Float = 16,

        [Display("double")]
        Double = 32,

        [Display("bytes")]
        Bytes = 64,

        [Display("string")]
        String = 128,

        [Display("record")]
        Record = 256,

        [Display("enum")]
        Enum = 512,

        [Display("array")]
        Array = 1024,

        [Display("map")]
        Map = 2048,

        [Display("fixed")]
        Fixed = 4096,

        [Display("logical")]
        Logical = 8192,
    }
}