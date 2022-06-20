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
        /// Gets or sets REQUIRED. Specifies the AsyncAPI Specification version being used.
        /// </summary>
        public string Asyncapi { get; set; }

        /// <summary>
        /// Gets or sets identifier of the application the AsyncAPI document is defining.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets REQUIRED. Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        public AsyncApiInfo Info { get; set; }

        /// <summary>
        /// Gets or sets provides connection details of servers. Field pattern ^[A-Za-z0-9_\-]+$.
        /// </summary>
        public IDictionary<string, AsyncApiServer> Servers { get; set; } = new Dictionary<string, AsyncApiServer>();

        /// <summary>
        /// Gets or sets default content type to use when encoding/decoding a message's payload.
        /// </summary>
        /// <remarks>
        /// A string representing the default content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json). This value MUST be used by schema parsers when the contentType property is omitted.
        /// </remarks>
        public string DefaultContentType { get; set; }

        /// <summary>
        /// Gets or sets REQUIRED. The available channels and messages for the API.
        /// </summary>
        public IList<AsyncApiChannel> Channels { get; set; } = new List<AsyncApiChannel>();

        /// <summary>
        /// Gets or sets an element to hold various schemas for the specification.
        /// </summary>
        public AsyncApiComponents Components { get; set; }

        /// <summary>
        /// Gets or sets a list of tags used by the specification with additional metadata. Each tag name in the list MUST be unique.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// Gets or sets additional external documentation.
        /// </summary>
        public AsyncApiExternalDocumentation ExternalDocs { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            // asyncApi
            writer.WriteProperty(AsyncApiConstants.AsyncApi, "2.2.0");

            // info
            writer.WriteRequiredObject(AsyncApiConstants.Info, this.Info, (w, i) => i.SerializeV2(w));

            // servers
            writer.WriteOptionalMap(AsyncApiConstants.Servers, this.Servers, (w, s) => s.SerializeV2(w));

            // channels
            writer.WriteRequiredObject(AsyncApiConstants.Channels, this.Channels, (w, p) => p.SerializeV2(w));

            // components
            writer.WriteOptionalObject(AsyncApiConstants.Components, this.Components, (w, c) => c.SerializeV2(w));

            // tags
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, t) => t.SerializeV2WithoutReference(w));

            // external docs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV2(w));

            // extensions
            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_2_0);

            writer.WriteEndObject();
        }
    }
}
