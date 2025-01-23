// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    public class AsyncApiServer : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// REQUIRED. A URL to the target host.
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// REQUIRED. The protocol this URL supports for connection.
        /// </summary>
        public virtual string Protocol { get; set; }

        /// <summary>
        /// the version of the protocol used for connection.
        /// </summary>
        public virtual string ProtocolVersion { get; set; }

        /// <summary>
        /// an optional string describing the host designated by the URL.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// a map between a variable name and its value. The value is used for substitution in the server's URL template.
        /// </summary>
        public virtual IDictionary<string, AsyncApiServerVariable> Variables { get; set; } = new Dictionary<string, AsyncApiServerVariable>();

        /// <summary>
        /// a declaration of which security mechanisms can be used with this server. The list of values includes alternative security requirement objects that can be used.
        /// </summary>
        /// <remarks>
        /// The name used for each property MUST correspond to a security scheme declared in the Security Schemes under the Components Object.
        /// </remarks>
        public virtual IList<AsyncApiSecurityRequirement> Security { get; set; } = new List<AsyncApiSecurityRequirement>();

        /// <summary>
        /// A list of tags for logical grouping and categorization of servers.
        /// </summary>
        public virtual IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the server.
        /// </summary>
        public virtual AsyncApiBindings<IServerBinding> Bindings { get; set; } = new AsyncApiBindings<IServerBinding>();

        /// <inheritdoc/>
        public virtual IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public virtual void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            writer.WriteRequiredProperty(AsyncApiConstants.Url, this.Url);

            writer.WriteRequiredProperty(AsyncApiConstants.Protocol, this.Protocol);

            writer.WriteOptionalProperty(AsyncApiConstants.ProtocolVersion, this.ProtocolVersion);

            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);

            writer.WriteOptionalMap(AsyncApiConstants.Variables, this.Variables, (w, v) => v.SerializeV2(w));

            writer.WriteOptionalCollection(AsyncApiConstants.Security, this.Security, (w, s) => s.SerializeV2(w));

            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, s) => s.SerializeV2(w));

            writer.WriteOptionalObject(AsyncApiConstants.Bindings, this.Bindings, (w, t) => t.SerializeV2(w));
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }
    }
}