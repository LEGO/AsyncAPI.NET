// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Attributes;

    public enum ReferenceType
    {
        None,

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
        [Display("serverBindings")] ServerBindings,

        /// <summary>
        /// ChannelBindings item.
        /// </summary>
        [Display("channelBindings")] ChannelBindings,

        /// <summary>
        /// OperationBindings item.
        /// </summary>
        [Display("operationBindings")] OperationBindings,

        /// <summary>
        /// MessageBindings item.
        /// </summary>
        [Display("messageBindings")] MessageBindings,

        /// <summary>
        /// Examples item.
        /// </summary>
        [Display("examples")] Example,

        /// <summary>
        /// Headers item.
        /// </summary>
        [Display("headers")] Header,

        /// <summary>
        /// The server variable
        /// </summary>
        [Display("serverVariable")] ServerVariable,
    }
}
