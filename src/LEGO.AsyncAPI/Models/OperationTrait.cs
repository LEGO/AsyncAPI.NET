﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Describes a trait that MAY be applied to an Operation Object.
    /// </summary>
    public class OperationTrait : IExtensible, IReferenceable
    {
        /// <summary>
        /// Unique string used to identify the operation.
        /// </summary>
        /// <remarks>
        /// The id MUST be unique among all operations described in the API.
        /// </remarks>
        public string OperationId { get; set; }

        /// <summary>
        /// A short summary of what the operation is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A short summary of what the operation is about.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of operations.
        /// </summary>
        public IList<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Additional external documentation for this operation.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the operation.
        /// </summary>
        [JsonConverter(typeof(OperationBindingConverter))]
        public IDictionary<string, IOperationBinding> Bindings { get; set; } = new Dictionary<string, IOperationBinding>();

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; } = new Dictionary<string, IAny>();

        /// <inheritdoc/>
        public bool? UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public Reference Reference { get; set; }
    }
}