// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Attributes;

    public enum ReferenceType
    {
        /// <summary>
        /// Schema item.
        /// </summary>
        [Display("schemas")] Schema,

        /// <summary>
        /// Servers item.
        /// </summary>
        [Display("servers")] Server,

        /// <summary>
        /// Channels item.
        /// </summary>
        [Display("channels")] Channel,

        /// <summary>
        /// Messages item.
        /// </summary>
        [Display("messages")] Message,

        /// <summary>
        /// SecuritySchemes item.
        /// </summary>
        [Display("securitySchemes")] SecurityScheme,

        /// <summary>
        /// Parameters item.
        /// </summary>
        [Display("parameters")] Parameter,

        /// <summary>
        /// CorrelationIds item.
        /// </summary>
        [Display("correlationIds")] CorrelationId,

        /// <summary>
        /// OperationTraits item.
        /// </summary>
        [Display("operationTraits")] OperationTrait,

        /// <summary>
        /// MessageTraits item.
        /// </summary>
        [Display("messageTraits")] MessageTrait,

        /// <summary>
        /// ServerBindings item.
        /// </summary>
        [Display("serverBindings")] ServerBinding,

        /// <summary>
        /// ChannelBindings item.
        /// </summary>
        [Display("channelBindings")] ChannelBinding,

        /// <summary>
        /// OperationBindings item.
        /// </summary>
        [Display("operationBindings")] OperationBinding,

        /// <summary>
        /// MessageBindings item.
        /// </summary>
        [Display("messageBindings")] MessageBinding,

        /// <summary>
        /// Responses item.
        /// </summary>
        [Display("responses")] Response,
        
        /// <summary>
        /// Links item.
        /// </summary>
        [Display("links")] Link,
        
        /// <summary>
        /// Examples item.
        /// </summary>
        [Display("examples")] Example,
        
        /// <summary>
        /// Headers item.
        /// </summary>
        [Display("headers")] Header,
        
        /// <summary>
        /// Callbacks item.
        /// </summary>
        [Display("callbacks")] Callback,
        
        /// <summary>
        /// Tags item.
        /// </summary>
        [Display("tags")] Tag,
    }
}