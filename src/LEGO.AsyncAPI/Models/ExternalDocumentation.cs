// Copyright (c) The LEGO Group. All rights reserved.

using Newtonsoft.Json;

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// Allows referencing an external resource for extended documentation.
    /// </summary>
    public class ExternalDocumentation : IExtensible
    {
        /// <summary>
        /// A short description of the target documentation. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. The URL for the target documentation. Value MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public IDictionary<string, string> Extensions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}