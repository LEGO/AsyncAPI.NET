// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Attributes;

    public enum SchemaType
    {
        [Display("null")]
        Null,

        [Display("boolean")]
        Boolean,

        [Display("object")]
        Object,

        [Display("array")]
        Array,

        [Display("number")]
        Number,

        [Display("string")]
        String,

        [Display("integer")]
        Integer,
    }
}