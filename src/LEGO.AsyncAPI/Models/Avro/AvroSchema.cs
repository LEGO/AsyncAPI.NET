// // Copyright (c) The LEGO Group. All rights reserved.
//
// namespace LEGO.AsyncAPI.Models
// {
//     using System;
//     using System.Collections.Generic;
//     using LEGO.AsyncAPI.Models.Interfaces;
//     using LEGO.AsyncAPI.Writers;
//
//     /// <summary>
//     /// Represents an Avro schema model (compatible with Avro 1.9.0).
//     /// </summary>
//     // #ToFix: Any type can be the main type so avroschema should be removed 
//     /*
//     Record
//
//     Fields:
//     name (string)
//     namespace (optional string)
//     doc (optional string)
//     aliases (optional array of strings)
//     fields (array of field objects, each with name, type, default, doc, order, and aliases)
//     
//     Enum
//     Fields:
//     name (string)
//     namespace (optional string)
//     aliases (optional array of strings)
//     symbols (array of strings)
//     default (optional string)
//     doc (optional string)
//     
//     Array
//     Fields:
//     items (type of the array elements)
//     
//     Map
//     Fields:
//     values (type of the map values)
//     
//     Union
//     Fields:
//     A list of potential types (each element is a valid Avro schema)
//     
//     Fixed
//     Fields:
//     name (string)
//     namespace (optional string)
//     aliases (optional array of strings)
//     size (integer specifying the number of bytes per value)
//     doc (optional string)
//     */
//
//     public class AvroSchema : IAsyncApiSerializable, IAsyncApiReferenceable
// {
//     /// <summary>
//     /// The type of the schema. See <a href="https://avro.apache.org/docs/1.9.0/spec.html#schema_primitive">Avro Schema Types</a>.
//     /// </summary>
//     public AvroSchemaType Type { get; set; }
//
//     /// <summary>
//     /// The name of the schema. Required for named types (e.g., record, enum, fixed). See <a href="https://avro.apache.org/docs/1.9.0/spec.html#names">Avro Names</a>.
//     /// </summary>
//     public string Name { get; set; }
//
//     /// <summary>
//     /// The namespace of the schema. Useful for named types to avoid name conflicts.
//     /// </summary>
//     public string? Namespace { get; set; }
//
//     /// <summary>
//     /// Documentation for the schema.
//     /// </summary>
//     public string? Doc { get; set; }
//
//     /// <summary>
//     /// The list of fields in the schema.
//     /// </summary>
//     public IList<AvroField> Fields { get; set; } = new List<AvroField>();
//
//     public bool UnresolvedReference { get; set; }
//
//     public AsyncApiReference Reference { get; set; }
//
//     public void SerializeV2(IAsyncApiWriter writer)
//     {
//         if (writer is null)
//         {
//             throw new ArgumentNullException(nameof(writer));
//         }
//
//         if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
//         {
//             this.Reference.SerializeV2(writer);
//             return;
//         }
//
//         this.SerializeV2WithoutReference(writer);
//     }
//
//     public void SerializeV2WithoutReference(IAsyncApiWriter writer)
//     {
//         writer.WriteStartObject();
//
//         writer.WriteOptionalProperty(AsyncApiConstants.Type, this.Type.GetDisplayName());
//         writer.WriteOptionalProperty("name", this.Name);
//         writer.WriteOptionalProperty("namespace", this.Namespace);
//         writer.WriteOptionalProperty("doc", this.Doc);
//         writer.WriteOptionalCollection("fields", this.Fields, (w, f) => f.SerializeV2(w));
//
//         writer.WriteEndObject();
//     }
// }
// }