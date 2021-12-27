// Copyright (c) The LEGO Group. All rights reserved.

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
        public IDictionary<string, string> Extensions { get; set; }
    }
}