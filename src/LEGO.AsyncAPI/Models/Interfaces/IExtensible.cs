// Copyright (c) The LEGO Group. All rights reserved.

using Newtonsoft.Json;

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// Defines that an AsyncAPI element can be extended.
    /// </summary>
    public interface IExtensible
    {
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, string> Extensions { get; set; }
    }
}