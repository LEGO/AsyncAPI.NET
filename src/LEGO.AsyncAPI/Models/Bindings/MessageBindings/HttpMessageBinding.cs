// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.MessageBindings
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Binding class for http messaging channels.
    /// </summary>
    public class HttpMessageBinding : IMessageBinding
    {
        /// <summary>
        /// Property containing http headers.
        /// </summary>
        public Schema Headers { get; set; }

        /// <summary>
        /// Property containing version of a binding.
        /// </summary>
        public string BindingVersion { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}