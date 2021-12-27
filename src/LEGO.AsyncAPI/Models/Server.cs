// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// The definition of a server this application MAY connect to.
    /// </summary>
    public class Server : IExtensible
    {
        /// <summary>
        /// REQUIRED. A URL to the target host.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// REQUIRED. The protocol this URL supports for connection.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// The version of the protocol used for connection.
        /// </summary>
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// An optional string describing the host designated by the URL.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A map between a variable name and its value. The value is used for substitution in the server's URL template.
        /// </summary>
        public IDictionary<string, ServerVariable> Variables { get; set; } = new Dictionary<string, ServerVariable>();

        /// <summary>
        /// A declaration of which security mechanisms can be used with this server. The list of values includes alternative security requirement objects that can be used.
        /// </summary>
        /// <remarks>
        /// The name used for each property MUST correspond to a security scheme declared in the Security Schemes under the Components Object.
        /// </remarks>
        public IDictionary<string, string[]> Security { get; set; } = new Dictionary<string,  string[]>();

        /// <inheritdoc/>
        public IDictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();
    }
}