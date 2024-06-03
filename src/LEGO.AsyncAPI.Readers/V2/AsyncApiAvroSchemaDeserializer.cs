// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiAvroSchemaDeserializer
    {
        private static readonly FixedFieldMap<AvroField> FieldFixedFields = new()
        {
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "type", (a, n) => a.Type = LoadSchema(n) },
            { "doc", (a, n) => a.Doc = n.GetScalarValue() },
            { "default", (a, n) => a.Default = n.CreateAny() },
            { "aliases", (a, n) => a.Aliases = n.CreateSimpleList(n2 => n2.GetScalarValue()) },
            { "order", (a, n) => a.Order = n.GetScalarValue().GetEnumFromDisplayName<AvroFieldOrder>() },
        };

        private static readonly FixedFieldMap<AvroRecord> RecordFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "doc", (a, n) => a.Doc = n.GetScalarValue() },
            { "namespace", (a, n) => a.Namespace = n.GetScalarValue() },
            { "aliases", (a, n) => a.Aliases = n.CreateSimpleList(n2 => n2.GetScalarValue()) },
            { "fields", (a, n) => a.Fields = n.CreateList(LoadField) },
        };

        private static readonly FixedFieldMap<AvroEnum> EnumFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "doc", (a, n) => a.Doc = n.GetScalarValue() },
            { "namespace", (a, n) => a.Namespace = n.GetScalarValue() },
            { "aliases", (a, n) => a.Aliases = n.CreateSimpleList(n2 => n2.GetScalarValue()) },
            { "symbols", (a, n) => a.Symbols = n.CreateSimpleList(n2 => n2.GetScalarValue()) },
            { "default", (a, n) => a.Default = n.GetScalarValue() },
        };

        private static readonly FixedFieldMap<AvroFixed> FixedFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "name", (a, n) => a.Name = n.GetScalarValue() },
            { "namespace", (a, n) => a.Namespace = n.GetScalarValue() },
            { "aliases", (a, n) => a.Aliases = n.CreateSimpleList(n2 => n2.GetScalarValue()) },
            { "size", (a, n) => a.Size = int.Parse(n.GetScalarValue(), n.Context.Settings.CultureInfo) },
        };

        private static readonly FixedFieldMap<AvroArray> ArrayFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "items", (a, n) => a.Items = LoadSchema(n) },
        };

        private static readonly FixedFieldMap<AvroMap> MapFixedFields = new()
        {
            { "type", (a, n) => { } },
            { "values", (a, n) => a.Values = n.GetScalarValue().GetEnumFromDisplayName<AvroPrimitiveType>() },
        };

        private static readonly FixedFieldMap<AvroUnion> UnionFixedFields = new()
        {
            { "types", (a, n) => a.Types = n.CreateList(LoadSchema) },
        };

        private static readonly PatternFieldMap<AvroRecord> RecordMetadataPatternFields =
        new()
        {
            { s => s.StartsWith(string.Empty), (a, p, n) => a.Metadata[p] = n.CreateAny() },
        };

        private static readonly PatternFieldMap<AvroField> FieldMetadataPatternFields =
        new()
        {
                { s => s.StartsWith(string.Empty), (a, p, n) => a.Metadata[p] = n.CreateAny() },
        };

        private static readonly PatternFieldMap<AvroEnum> EnumMetadataPatternFields =
        new()
        {
             { s => s.StartsWith(string.Empty), (a, p, n) => a.Metadata[p] = n.CreateAny() },
        };

        private static readonly PatternFieldMap<AvroFixed> FixedMetadataPatternFields =
        new()
        {
             { s => s.StartsWith(string.Empty), (a, p, n) => a.Metadata[p] = n.CreateAny() },
        };

        private static readonly PatternFieldMap<AvroArray> ArrayMetadataPatternFields =
        new()
        {
             { s => s.StartsWith(string.Empty), (a, p, n) => a.Metadata[p] = n.CreateAny() },
        };

        private static readonly PatternFieldMap<AvroMap> MapMetadataPatternFields =
        new()
        {
             { s => s.StartsWith(string.Empty), (a, p, n) => a.Metadata[p] = n.CreateAny() },
        };

        private static readonly PatternFieldMap<AvroUnion> UnionMetadataPatternFields =
        new()
        {
             { s => s.StartsWith(string.Empty), (a, p, n) => a.Metadata[p] = n.CreateAny() },
        };

        public static AvroSchema LoadSchema(ParseNode node)
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
                    union.Types.Add(LoadSchema(item));
                }

                return union;
            }

            if (node is MapNode mapNode)
            {
                var pointer = mapNode.GetReferencePointer();

                if (pointer != null)
                {
                    return new AvroRecord
                    {
                        UnresolvedReference = true,
                        Reference = node.Context.VersionService.ConvertToAsyncApiReference(pointer, ReferenceType.Schema),
                    };
                }

                var type = mapNode["type"]?.Value.GetScalarValue();

                switch (type)
                {
                    case "record":
                        var record = new AvroRecord();
                        mapNode.ParseFields(ref record, RecordFixedFields, RecordMetadataPatternFields);
                        return record;
                    case "enum":
                        var @enum = new AvroEnum();
                        mapNode.ParseFields(ref @enum, EnumFixedFields, EnumMetadataPatternFields);
                        return @enum;
                    case "fixed":
                        var @fixed = new AvroFixed();
                        mapNode.ParseFields(ref @fixed, FixedFixedFields, FixedMetadataPatternFields);
                        return @fixed;
                    case "array":
                        var array = new AvroArray();
                        mapNode.ParseFields(ref array, ArrayFixedFields, ArrayMetadataPatternFields);
                        return array;
                    case "map":
                        var map = new AvroMap();
                        mapNode.ParseFields(ref map, MapFixedFields, MapMetadataPatternFields);
                        return map;
                    case "union":
                        var union = new AvroUnion();
                        mapNode.ParseFields(ref union, UnionFixedFields, UnionMetadataPatternFields);
                        return union;
                    default:
                        throw new AsyncApiException($"Unsupported type: {type}");
                }
            }

            throw new AsyncApiReaderException("Invalid node type");
        }

        private static AvroField LoadField(ParseNode node)
        {
            var mapNode = node.CheckMapNode("field");
            var field = new AvroField();

            mapNode.ParseFields<AvroField>(ref field, FieldFixedFields, FieldMetadataPatternFields);

            return field;

        }
    }
}