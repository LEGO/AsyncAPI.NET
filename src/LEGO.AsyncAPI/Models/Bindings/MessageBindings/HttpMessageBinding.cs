// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.MessageBindings
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Binding class for http messaging channels.
    /// </summary>
    public class HttpMessageBinding : IMessageBinding
    {
        /// <summary>
        /// Gets or sets property containing http headers.
        /// </summary>
        public Schema Headers { get; set; }

        /// <summary>
        /// Gets or sets property containing version of a binding.
        /// </summary>
        public string BindingVersion { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; }
    }
}