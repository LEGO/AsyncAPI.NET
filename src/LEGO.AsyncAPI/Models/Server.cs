// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The definition of a server this application MAY connect to.
    /// </summary>
    public class AsyncApiServer : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// Initializes Server object.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="protocol"></param>
        public AsyncApiServer(string url, string protocol)
        {
            this.Url = new Uri(url);
            this.Protocol = protocol;
        }

        /// <summary>
        /// Gets or sets REQUIRED. A URL to the target host.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets REQUIRED. The protocol this URL supports for connection.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the version of the protocol used for connection.
        /// </summary>
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets an optional string describing the host designated by the URL.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a map between a variable name and its value. The value is used for substitution in the server's URL template.
        /// </summary>
        public IDictionary<string, AsyncApiServerVariable> Variables { get; set; } = new Dictionary<string, AsyncApiServerVariable>();

        /// <summary>
        /// Gets or sets a declaration of which security mechanisms can be used with this server. The list of values includes alternative security requirement objects that can be used.
        /// </summary>
        /// <remarks>
        /// The name used for each property MUST correspond to a security scheme declared in the Security Schemes under the Components Object.
        /// </remarks>
        public IList<AsyncApiSecurityRequirement> Security { get; set; } = new List<AsyncApiSecurityRequirement>();

        /// <summary>
        /// Gets or sets a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the server.
        /// </summary>
        public IDictionary<string, IServerBinding> Bindings { get; set; } = new Dictionary<string, IServerBinding>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteRequiredProperty(AsyncApiConstants.Url, this.Url?.OriginalString);

            writer.WriteRequiredProperty(AsyncApiConstants.Protocol, this.Protocol);

            writer.WriteProperty(AsyncApiConstants.ProtocolVersion, this.ProtocolVersion);

            writer.WriteProperty(AsyncApiConstants.Description, this.Description);

            writer.WriteOptionalMap(AsyncApiConstants.Variables, this.Variables, (w, v) => v.SerializeV2(w));

            writer.WriteOptionalCollection(AsyncApiConstants.Security, this.Security, (w, s) => s.SerializeV2(w));

            writer.WriteOptionalMap(AsyncApiConstants.Bindings, this.Bindings, (w, b) => b.SerializeV2(w));

            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_2_0);

            writer.WriteEndObject();
        }
    }
}