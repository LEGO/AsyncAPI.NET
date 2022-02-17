namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Holds a set of reusable objects for different aspects of the AsyncAPI specification.
    /// </summary>
    /// <remarks>
    /// All objects defined within the components object will have no effect on the API unless they are explicitly referenced from properties outside the components object.
    /// </remarks>
    public class Components : IExtensible
    {
        /// <summary>
        /// An object to hold reusable Schema Objects.
        /// </summary>
        public IDictionary<string, Schema> Schemas { get; set; } = new Dictionary<string, Schema>();

        /// <summary>
        /// An object to hold reusable Message Objects.
        /// </summary>
        public IDictionary<string, Message> Messages { get; set; } = new Dictionary<string, Message>();

        /// <summary>
        /// An object to hold reusable Security Scheme Objects.
        /// </summary>
        public IDictionary<string, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, SecurityScheme>();

        /// <summary>
        /// An object to hold reusable Parameter Objects.
        /// </summary>
        public IDictionary<string, Parameter> Parameters { get; set; } = new Dictionary<string, Parameter>();

        /// <summary>
        /// An object to hold reusable Correlation ID Objects.
        /// </summary>
        public IDictionary<string, CorrelationId> CorrelationIds { get; set; } = new Dictionary<string, CorrelationId>();

        /// <summary>
        /// An object to hold reusable Operation Trait Objects.
        /// </summary>
        public IDictionary<string, OperationTrait> OperationTraits { get; set; } = new Dictionary<string, OperationTrait>();

        /// <summary>
        /// An object to hold reusable Message Trait Objects.
        /// </summary>
        public IDictionary<string, MessageTrait> MessageTraits { get; set; } = new Dictionary<string, MessageTrait>();

        /// <summary>
        /// An object to hold reusable Server Bindings Objects.
        /// </summary>
        [JsonConverter(typeof(ServerBindingConverter))]
        public IDictionary<string, IServerBinding> ServerBindings { get; set; } = new Dictionary<string, IServerBinding>();

        /// <summary>
        /// An object to hold reusable Channel Bindings Objects.
        /// </summary>
        [JsonConverter(typeof(ChannelJsonDictionaryContractBindingConverter))]
        public IDictionary<string, IChannelBinding> ChannelBindings { get; set; } = new Dictionary<string, IChannelBinding>();

        /// <summary>
        /// An object to hold reusable Operation Bindings Objects.
        /// </summary>
        [JsonConverter(typeof(OperationBindingConverter))]
        public IDictionary<string, IOperationBinding> OperationBindings { get; set; } = new Dictionary<string, IOperationBinding>();

        /// <summary>
        /// An object to hold reusable Message Bindings Objects.
        /// </summary>
        [JsonConverter(typeof(MessageJsonDictionaryContractBindingConverter))]
        public IDictionary<string, IMessageBinding> MessageBindings { get; set; } = new Dictionary<string, IMessageBinding>();

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; }

        public IDictionary<string, Server> Servers { get; set; } = new Dictionary<string, Server>();

        public IDictionary<string, Channel> Channels { get; set; } = new Dictionary<string, Channel>();
    }
}