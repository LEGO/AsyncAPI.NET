// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using LEGO.AsyncAPI.Attributes;

    [Flags]
    public enum SchemaType
    {
        [Display("null")]
        Null = 1,

        [Display("boolean")]
        Boolean = 2,

        [Display("object")]
        Object = 4,

        [Display("array")]
        Array = 8,

        [Display("number")]
        Number = 16,

        [Display("string")]
        String = 32,

        [Display("integer")]
        Integer = 64,
    }
}
