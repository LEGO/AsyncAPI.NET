// Copyright (c) The LEGO Group. All rights reserved.

using Newtonsoft.Json;

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    public class License : IExtensible
    {
        /// <summary>
        /// REQUIRED. The license name used for the API.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// A URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <inheritdoc/>
        [System.Text.Json.Serialization.JsonIgnore]
        public IDictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();

        public override bool Equals(object? obj)
        {
            return Equals(obj as License);
        }

        public bool Equals(License license)
        {
            return license != null &&
                   string.Compare(Name, license.Name, StringComparison.Ordinal) == 0 &&
                   ((Url == null && license.Url == null) || Url.Equals(license.Url));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Url);
        }
    }
}