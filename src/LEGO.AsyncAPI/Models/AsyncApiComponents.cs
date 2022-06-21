// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Holds a set of reusable objects for different aspects of the AsyncAPI specification.
    /// </summary>
    /// <remarks>
    /// All objects defined within the components object will have no effect on the API unless they are explicitly referenced from properties outside the components object.
    /// </remarks>
    public class AsyncApiComponents : IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets an object to hold reusable Schema Objects.
        /// </summary>
        public IDictionary<string, Schema> Schemas { get; set; } = new Dictionary<string, Schema>();

        /// <summary>
        /// Gets or sets an object to hold reusable Message Objects.
        /// </summary>
        public IDictionary<string, AsyncApiMessage> Messages { get; set; } = new Dictionary<string, AsyncApiMessage>();

        /// <summary>
        /// Gets or sets an object to hold reusable Security Scheme Objects.
        /// </summary>
        public IDictionary<string, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, SecurityScheme>();

        /// <summary>
        /// Gets or sets an object to hold reusable Parameter Objects.
        /// </summary>
        public IDictionary<string, AsyncApiParameter> Parameters { get; set; } = new Dictionary<string, AsyncApiParameter>();

        /// <summary>
        /// Gets or sets an object to hold reusable Correlation ID Objects.
        /// </summary>
        public IDictionary<string, CorrelationId> CorrelationIds { get; set; } = new Dictionary<string, CorrelationId>();

        /// <summary>
        /// Gets or sets an object to hold reusable Operation Trait Objects.
        /// </summary>
        public IDictionary<string, AsyncApiOperationTrait> OperationTraits { get; set; } = new Dictionary<string, AsyncApiOperationTrait>();

        /// <summary>
        /// Gets or sets an object to hold reusable Message Trait Objects.
        /// </summary>
        public IDictionary<string, MessageTrait> MessageTraits { get; set; } = new Dictionary<string, MessageTrait>();

        /// <summary>
        /// Gets or sets an object to hold reusable Server Bindings Objects.
        /// </summary>
        public IDictionary<string, IServerBinding> ServerBindings { get; set; } = new Dictionary<string, IServerBinding>();

        /// <summary>
        /// Gets or sets an object to hold reusable Channel Bindings Objects.
        /// </summary>
        public IDictionary<string, IChannelBinding> ChannelBindings { get; set; } = new Dictionary<string, IChannelBinding>();

        /// <summary>
        /// Gets or sets an object to hold reusable Operation Bindings Objects.
        /// </summary>
        public IDictionary<string, IOperationBinding> OperationBindings { get; set; } = new Dictionary<string, IOperationBinding>();

        /// <summary>
        /// Gets or sets an object to hold reusable Message Bindings Objects.
        /// </summary>
        public IDictionary<string, IMessageBinding> MessageBindings { get; set; } = new Dictionary<string, IMessageBinding>();

        public IDictionary<string, AsyncApiServer> Servers { get; set; } = new Dictionary<string, AsyncApiServer>();

        public IDictionary<string, AsyncApiChannel> Channels { get; set; } = new Dictionary<string, AsyncApiChannel>();

        IDictionary<string, IAsyncApiExtension> IAsyncApiExtensible.Extensions { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}