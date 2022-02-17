namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// The definition of a server this application MAY connect to.
    /// </summary>
    public class Server : IExtensible
    {
        public Server(string url, string protocol)
        {
            Url = new Uri(url);
            Protocol = protocol;
        }

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
        public IDictionary<string, ServerVariable> Variables { get; set; }

        /// <summary>
        /// A declaration of which security mechanisms can be used with this server. The list of values includes alternative security requirement objects that can be used.
        /// </summary>
        /// <remarks>
        /// The name used for each property MUST correspond to a security scheme declared in the Security Schemes under the Components Object.
        /// </remarks>
        public IDictionary<string, string[]> Security { get; set; }

        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the server.
        /// </summary>
        [JsonConverter(typeof(ServerBindingConverter))]
        public IDictionary<string, IServerBinding> Bindings { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}