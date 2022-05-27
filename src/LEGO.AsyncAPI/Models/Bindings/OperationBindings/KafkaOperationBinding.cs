// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.OperationBindings
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Binding class for Kafka operations.
    /// </summary>
    public class KafkaOperationBinding : IOperationBinding
    {
        /// <summary>
        /// Gets or sets kafka group id.
        /// </summary>
        public Schema GroupId { get; set; }

        /// <summary>
        /// Gets or sets kafka client id.
        /// </summary>
        public Schema ClientId { get; set; }

        /// <summary>
        /// Gets or sets property containing version of a binding.
        /// </summary>
        public string BindingVersion { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; }
    }
}