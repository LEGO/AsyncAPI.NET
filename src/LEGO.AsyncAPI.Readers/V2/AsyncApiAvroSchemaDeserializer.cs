// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiAvroSchemaDeserializer
    {
        private static readonly FixedFieldMap<AvroSchema> schemaFixedFields = new()
        {
            { "type", (a, n) => a.Type = n.GetScalarValue().GetEnumFromDisplayName<AvroSchemaType>() },
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "namespace", (a, n) => a.Namespace = n.GetScalarValue() },
            { "doc", (a, n) => a.Doc = n.GetScalarValue() },
            { "fields", (a, n) => a.Fields = n.CreateList(LoadField) },
        };

        public static AvroSchema LoadSchema(ParseNode node)
        {
            var mapNode = node.CheckMapNode("schema");
            var schema = new AvroSchema();

            mapNode.ParseFields(ref schema, schemaFixedFields, null);

            return schema;
        }

        private static AvroField LoadField(ParseNode node)
        {
            var mapNode = node.CheckMapNode("field");
            var field = new AvroField();

            mapNode.ParseFields(ref field, fieldFixedFields, null);

            return field;
        }

        private static readonly FixedFieldMap<AvroField> fieldFixedFields = new()
        {
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "type", (a, n) => a.Type = LoadFieldType(n) },
            { "doc", (a, n) => a.Doc = n.GetScalarValue() },
            { "default", (a, n) => a.Default = n.CreateAny() },
            { "order", (a, n) => a.Order = n.GetScalarValue() },
        };

        private static readonly FixedFieldMap<AvroRecord> recordFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "fields", (a, n) => a.Fields = n.CreateList(LoadField) },
        };

        private static readonly FixedFieldMap<AvroEnum> enumFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "symbols", (a, n) => a.Symbols = n.CreateSimpleList(n2 => n2.GetScalarValue()) },
        };

        private static readonly FixedFieldMap<AvroFixed> fixedFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "size", (a, n) => a.Size = int.Parse(n.GetScalarValue(), n.Context.Settings.CultureInfo) },
        };

        private static readonly FixedFieldMap<AvroArray> arrayFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "items", (a, n) => a.Items = LoadFieldType(n) },
        };

        private static readonly FixedFieldMap<AvroMap> mapFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "values", (a, n) => a.Values = LoadFieldType(n) },
        };

        private static readonly FixedFieldMap<AvroUnion> unionFixedFields = new()
        {
            { "types", (a, n) => a.Types = n.CreateList(LoadFieldType) },
        };

        private static AvroFieldType LoadFieldType(ParseNode node)
        {
            if (node is ValueNode valueNode)
            {
                return new AvroPrimitive(valueNode.GetScalarValue().GetEnumFromDisplayName<AvroPrimitiveType>());
            }

            if (node is ListNode)
            {
                var union = new AvroUnion();
                foreach (var item in node as ListNode)
                {
                    union.Types.Add(LoadFieldType(item));
                }

                return union;
            }

            if (node is MapNode mapNode)
            {
                var type = mapNode["type"].Value?.GetScalarValue();

                switch (type)
                {
                    case "record":
                        var record = new AvroRecord();
                        mapNode.ParseFields(ref record, recordFixedFields, null);
                        return record;
                    case "enum":
                        var @enum = new AvroEnum();
                        mapNode.ParseFields(ref @enum, enumFixedFields, null);
                        return @enum;
                    case "fixed":
                        var @fixed = new AvroFixed();
                        mapNode.ParseFields(ref @fixed, fixedFixedFields, null);
                        return @fixed;
                    case "array":
                        var array = new AvroArray();
                        mapNode.ParseFields(ref array, arrayFixedFields, null);
                        return array;
                    case "map":
                        var map = new AvroMap();
                        mapNode.ParseFields(ref map, mapFixedFields, null);
                        return map;
                    case "union":
                        var union = new AvroUnion();
                        mapNode.ParseFields(ref union, unionFixedFields, null);
                        return union;
                    default:
                        throw new InvalidOperationException($"Unsupported type: {type}");
                }
            }

            throw new AsyncApiReaderException("Invalid node type");
        }
    }
}