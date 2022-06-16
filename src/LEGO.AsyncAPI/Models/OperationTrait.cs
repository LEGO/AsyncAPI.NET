// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Describes a trait that MAY be applied to an Operation Object.
    /// </summary>
    public class OperationTrait : IAsyncApiExtensible, IReferenceable
    {
        /// <summary>
        /// Gets or sets unique string used to identify the operation.
        /// </summary>
        /// <remarks>
        /// The id MUST be unique among all operations described in the API.
        /// </remarks>
        public string OperationId { get; set; }

        /// <summary>
        /// Gets or sets a short summary of what the operation is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets a short summary of what the operation is about.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a list of tags for API documentation control. Tags can be used for logical grouping of operations.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// Gets or sets additional external documentation for this operation.
        /// </summary>
        public AsyncApiExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the operation.
        /// </summary>
        [JsonConverter(typeof(OperationBindingConverter))]
        public IDictionary<string, IOperationBinding> Bindings { get; set; } = new Dictionary<string, IOperationBinding>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();

        /// <inheritdoc/>
        public bool? UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public AsyncApiReference Reference { get; set; }
    }
}