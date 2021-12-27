// Copyright (c) The LEGO Group. All rights reserved.

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
        public string Name { get; set; }

        /// <summary>
        /// A URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();
    }
}