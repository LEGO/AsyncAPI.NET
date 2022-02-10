// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Contact information for the exposed API.
    /// </summary>
    public class Contact : IExtensible
    {
        /// <summary>
        /// The identifying name of the contact person/organization.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The URL pointing to the contact information. MUST be in the format of a URL.
        /// </summary>
        [JsonProperty("uri")]
        public Uri Url { get; set; }

        /// <summary>
        /// The email address of the contact person/organization. MUST be in the format of an email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <inheritdoc/>
        [JsonExtensionData]
        public IDictionary<string, JToken> Extensions { get; set; }
    }
}