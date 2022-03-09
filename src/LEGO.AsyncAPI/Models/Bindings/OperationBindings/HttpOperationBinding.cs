// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.OperationBindings
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Binding class for http operations.
    /// </summary>
    public class HttpOperationBinding : IOperationBinding
    {
        /// <summary>
        /// Type of binding.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Http method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Http query.
        /// </summary>
        public Schema Query { get; set; }

        /// <summary>
        /// Property containing version of a binding.
        /// </summary>
        public string BindingVersion { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}