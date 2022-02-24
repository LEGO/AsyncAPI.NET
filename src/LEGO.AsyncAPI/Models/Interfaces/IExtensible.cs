// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Models.Any;

    /// <summary>
    /// Defines that an AsyncAPI element can be extended.
    /// </summary>
    public interface IExtensible
    {
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// To protect the API from leaking the underlying JSON library, the extension data extraction is handled by a customer resolver.
        /// </summary>
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}