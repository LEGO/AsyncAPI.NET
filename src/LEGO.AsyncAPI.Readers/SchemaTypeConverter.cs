// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using Json.Schema;

    public static class SchemaTypeConverter
    {
        public static SchemaValueType ConvertToSchemaValueType(string value)
        {
            return value.ToLowerInvariant() switch
            {
                "string" => SchemaValueType.String,
                "number" or "double" => SchemaValueType.Number,
                "integer" => SchemaValueType.Integer,
                "boolean" => SchemaValueType.Boolean,
                "array" => SchemaValueType.Array,
                "object" => SchemaValueType.Object,
                "null" => SchemaValueType.Null,
                _ => throw new NotSupportedException(),
            };
        }
    }
}
