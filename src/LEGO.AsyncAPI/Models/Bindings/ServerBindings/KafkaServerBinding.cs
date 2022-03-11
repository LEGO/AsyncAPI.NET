// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.ServerBindings
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Binding class for Kafka server settings.
    /// </summary>
    public class KafkaServerBinding : IServerBinding
    {
        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}