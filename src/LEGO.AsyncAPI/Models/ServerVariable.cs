// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// An object representing a Server Variable for server URL template substitution.
    /// </summary>
    public class ServerVariable : IExtensible
    {
        /// <summary>
        /// An enumeration of string values to be used if the substitution options are from a limited set.
        /// </summary>
        [JsonProperty("enum")]
        public IList<string> Enum { get; set; }

        /// <summary>
        /// The default value to use for substitution, and to send, if an alternate value is not supplied.
        /// </summary>
        [JsonProperty("default")]
        public string Default { get; set; }

        /// <summary>
        /// An optional description for the server variable. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// An array of examples of the server variable.
        /// </summary>
        [JsonProperty("examples")]
        public IList<string> Examples { get; set; }

        /// <inheritdoc/>
        [JsonExtensionData]
        public IDictionary<string, JToken> Extensions { get; set; }
    }
}