// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// This is the root document object for the API specification. It combines resource listing and API declaration together into one document.
    /// </summary>
    public class AsyncApiDocument : IAsyncApiExtensible, IAsyncApiSerializable
    {
        /// <summary>
        /// Gets or sets rEQUIRED. Specifies the AsyncAPI Specification version being used.
        /// </summary>
        public string Asyncapi { get; set; }

        /// <summary>
        /// Gets or sets identifier of the application the AsyncAPI document is defining.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets rEQUIRED. Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        public Info Info { get; set; }

        /// <summary>
        /// Gets or sets provides connection details of servers. Field pattern ^[A-Za-z0-9_\-]+$.
        /// </summary>
        public IDictionary<string, Server> Servers { get; set; } = new Dictionary<string, Server>();

        /// <summary>
        /// Gets or sets default content type to use when encoding/decoding a message's payload.
        /// </summary>
        /// <remarks>
        /// A string representing the default content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json). This value MUST be used by schema parsers when the contentType property is omitted.
        /// </remarks>
        public string DefaultContentType { get; set; }

        /// <summary>
        /// Gets or sets rEQUIRED. The available channels and messages for the API.
        /// </summary>
        public IDictionary<string, Channel> Channels { get; set; } = new Dictionary<string, Channel>();

        /// <summary>
        /// Gets or sets an element to hold various schemas for the specification.
        /// </summary>
        public Components Components { get; set; }

        /// <summary>
        /// Gets or sets a list of tags used by the specification with additional metadata. Each tag name in the list MUST be unique.
        /// </summary>
        public IList<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Gets or sets additional external documentation.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            // openapi
            writer.WriteProperty(AsyncApiConstants.AsyncApi, "3.0.1");

            // info
            writer.WriteRequiredObject(OpenApiConstants.Info, Info, (w, i) => i.SerializeAsV3(w));

            // servers
            writer.WriteOptionalCollection(OpenApiConstants.Servers, Servers, (w, s) => s.SerializeAsV3(w));

            // paths
            writer.WriteRequiredObject(OpenApiConstants.Paths, Paths, (w, p) => p.SerializeAsV3(w));

            // components
            writer.WriteOptionalObject(OpenApiConstants.Components, Components, (w, c) => c.SerializeAsV3(w));

            // security
            writer.WriteOptionalCollection(
                OpenApiConstants.Security,
                SecurityRequirements,
                (w, s) => s.SerializeAsV3(w));

            // tags
            writer.WriteOptionalCollection(OpenApiConstants.Tags, Tags, (w, t) => t.SerializeAsV3WithoutReference(w));

            // external docs
            writer.WriteOptionalObject(OpenApiConstants.ExternalDocs, ExternalDocs, (w, e) => e.SerializeAsV3(w));

            // extensions
            writer.WriteExtensions(Extensions, OpenApiSpecVersion.OpenApi3_0);

            writer.WriteEndObject();
        }
    }
}
