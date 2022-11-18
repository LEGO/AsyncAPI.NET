namespace LEGO.AsyncAPI.Models.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines that an AsyncAPI element can be extended.
    /// </summary>
    public interface IAsyncApiExtensible : IAsyncApiElement
    {
        /// <summary>
        /// Gets or sets this object MAY be extended with Specification Extensions.
        /// To protect the API from leaking the underlying JSON library, the extension data extraction is handled by a customer resolver.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; }
    }
}