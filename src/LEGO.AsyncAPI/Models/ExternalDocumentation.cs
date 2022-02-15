// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}