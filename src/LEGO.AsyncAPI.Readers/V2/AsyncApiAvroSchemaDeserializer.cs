// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Globalization;
    using LEGO.AsyncAPI.Extensions;
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

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(schema, schemaFixedFields, null);
            }

            return schema;
        }

        private static AvroField LoadField(ParseNode node)
        {
            var mapNode = node.CheckMapNode("field");
            var field = new AvroField();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(field, fieldFixedFields, null);
            }

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

        private static AvroFieldType LoadFieldType(ParseNode node)
        {
            if (node is ValueNode valueNode)
            {
                return new AvroPrimitive(valueNode.GetScalarValue().GetEnumFromDisplayName<AvroPrimitiveType>());
            }

            if (node is MapNode mapNode)
            {
                //var typeNode = mapNode.GetValue("type");
                //var type = typeNode?.GetScalarValue();

                //switch (type)
                //{
                //    case "record":
                //        return new AvroRecord
                //        {
                //            Name = mapNode.GetValue("name")?.GetScalarValue(),
                //            Fields = mapNode.GetValue("fields").CreateList(LoadField)
                //        };
                //    case "enum":
                //        return new AvroEnum
                //        {
                //            Name = mapNode.GetValue("name")?.GetScalarValue(),
                //            Symbols = mapNode.GetValue("symbols").CreateSimpleList(n => n.GetScalarValue())
                //        };
                //    case "fixed":
                //        return new AvroFixed
                //        {
                //            Name = mapNode.GetValue("name")?.GetScalarValue(),
                //            Size = int.Parse(mapNode.GetValue("size").GetScalarValue())
                //        };
                //    case "array":
                //        return new AvroArray
                //        {
                //            Items = LoadFieldType(mapNode.GetValue("items"))
                //        };
                //    case "map":
                //        return new AvroMap
                //        {
                //            Values = LoadFieldType(mapNode.GetValue("values"))
                //        };
                //    case "union":
                //        return new AvroUnion
                //        {
                //            Types = mapNode.GetValue("types").CreateList(LoadFieldType)
                //        };
                //    default:
                //        throw new InvalidOperationException($"Unsupported type: {type}");
                //}
            }

            throw new AsyncApiReaderException("Invalid node type");
        }
    }
}