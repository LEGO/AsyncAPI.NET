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
    public class AsyncApiServer : IAsyncApiSerializable, IAsyncApiExtensible, IAsyncApiReferenceable
    {
        /// <summary>
        /// REQUIRED. The server host name. It MAY include the port. This field supports Server Variables. Variable substitutions will be made when a variable is named in {braces}.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The path to a resource in the host. This field supports Server Variables. Variable substitutions will be made when a variable is named in {braces}.
        /// </summary>
        public string PathName { get; set; }

        /// <summary>
        /// REQUIRED. The protocol this URL supports for connection.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// The version of the protocol used for connection. For instance: AMQP 0.9.1, HTTP 2.0, Kafka 1.0.0, etc.
        /// </summary>
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// An optional string describing the server. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A human-friendly title for the server.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short summary of the server.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// a map between a variable name and its value. The value is used for substitution in the server's URL template.
        /// </summary>
        public IDictionary<string, AsyncApiServerVariable> Variables { get; set; } = new Dictionary<string, AsyncApiServerVariable>();

        /// <summary>
        /// a declaration of which security mechanisms can be used with this server. The list of values includes alternative security requirement objects that can be used.
        /// </summary>
        /// <remarks>
        /// The name used for each property MUST correspond to a security scheme declared in the Security Schemes under the Components Object.
        /// </remarks>
        public IList<AsyncApiSecurityScheme> Security { get; set; } = new List<AsyncApiSecurityScheme>();

        /// <summary>
        /// A list of tags for logical grouping and categorization of servers.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the server.
        /// </summary>
        public AsyncApiBindings<IServerBinding> Bindings { get; set; } = new AsyncApiBindings<IServerBinding>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
            {
                this.Reference.SerializeV2(writer);
                return;
            }

            this.SerializeV2WithoutReference(writer);
        }

        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            writer.WriteRequiredProperty(AsyncApiConstants.Url, this.GenerateServerUrl());

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

        public void SerializeV3(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
            {
                this.Reference.SerializeV3(writer);
                return;
            }

            writer.WriteStartObject();

            writer.WriteRequiredProperty(AsyncApiConstants.Host, this.Host);

            writer.WriteRequiredProperty(AsyncApiConstants.PathName, this.PathName);

            writer.WriteRequiredProperty(AsyncApiConstants.Protocol, this.Protocol);

            writer.WriteOptionalProperty(AsyncApiConstants.ProtocolVersion, this.ProtocolVersion);

            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);

            writer.WriteOptionalProperty(AsyncApiConstants.Title, this.Title);

            writer.WriteOptionalProperty(AsyncApiConstants.Summary, this.Summary);

            writer.WriteOptionalMap(AsyncApiConstants.Variables, this.Variables, (w, v) => v.SerializeV3(w));

            writer.WriteOptionalCollection(AsyncApiConstants.Security, this.Security, (w, s) => s.SerializeV3(w));

            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, s) => s.SerializeV3(w));

            writer.WriteOptionalObject(AsyncApiConstants.Bindings, this.Bindings, (w, t) => t.SerializeV3(w));
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }

        private string GenerateServerUrl()
        {
            var baseUri = new Uri($"{this.Protocol}{Uri.SchemeDelimiter}{this.Host}");
            return new Uri(baseUri, this.PathName).ToString();
        }
    }
}