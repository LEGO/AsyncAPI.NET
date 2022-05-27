﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// The Schema Object allows the definition of input and output data types.
    /// </summary>
    public class Schema : IReferenceable
    {
        /// <summary>
        /// Gets or sets follow JSON Schema definition. Short text providing information about the data.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Value MUST be a string. Multiple types via an array are not supported.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public decimal? Maximum { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public bool? ExclusiveMaximum { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public decimal? Minimum { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public bool? ExclusiveMinimum { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// This string SHOULD be a valid regular expression, according to the ECMA 262 regular expression dialect.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public decimal? MultipleOf { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether relevant only for Schema "properties" definitions. Declares the property as "read only".
        /// This means that it MAY be sent as part of a response but SHOULD NOT be sent as part of the request.
        /// If the property is marked as readOnly being true and is in the required list,
        /// the required will take effect on the response only.
        /// A property MUST NOT be marked as both readOnly and writeOnly being true.
        /// Default value is false.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether relevant only for Schema "properties" definitions. Declares the property as "write only".
        /// Therefore, it MAY be sent as part of a request but SHOULD NOT be sent as part of the response.
        /// If the property is marked as writeOnly being true and is in the required list,
        /// the required will take effect on the request only.
        /// A property MUST NOT be marked as both readOnly and writeOnly being true.
        /// Default value is false.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool WriteOnly { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<Schema> AllOf { get; set; } = new List<Schema>();

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<Schema> OneOf { get; set; } = new List<Schema>();

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<Schema> AnyOf { get; set; } = new List<Schema>();

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public Schema Not { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public Schema Contains { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public Schema If { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public Schema Then { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public Schema Else { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public ISet<string> Required { get; set; } = new HashSet<string>();

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Value MUST be an object and not an array. Inline or referenced schema MUST be of a Schema Object
        /// and not a standard JSON Schema. items MUST be present if the type is array.
        /// </summary>
        public Schema Items { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MaxItems { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MinItems { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public bool? UniqueItems { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Property definitions MUST be a Schema Object and not a standard JSON Schema (inline or referenced).
        /// </summary>
        public IDictionary<string, Schema> Properties { get; set; } = new Dictionary<string, Schema>();

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MaxProperties { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public int? MinProperties { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01
        /// Value can be boolean or object. Inline or referenced schema
        /// MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public Schema AdditionalProperties { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public Schema PropertyNames { get; set; }

        /// <summary>
        /// Gets or sets adds support for polymorphism.
        /// The discriminator is the schema property name that is used to differentiate between other schema that inherit this schema.
        /// </summary>
        public string Discriminator { get; set; }

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public IList<string> Enum { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets follow JSON Schema definition: https://tools.ietf.org/html/draft-handrews-json-schema-validation-01.
        /// </summary>
        public IAsyncApiAny Const { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether allows sending a null value for the defined schema. Default value is false.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Nullable { get; set; }

        /// <summary>
        /// Gets or sets additional external documentation for this schema.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether specifies that a schema is deprecated and SHOULD be transitioned out of usage.
        /// Default value is false.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Deprecated { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public bool? UnresolvedReference { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc/>
        [JsonIgnore]
        public AsyncApiReference Reference { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}