// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiComponents> componentsFixedFields = new()
        {
            { "schemas", (a, n) => a.Schemas = n.CreateMapWithReference(ReferenceType.Schema, LoadSchema) },
            { "servers", (a, n) => a.Servers = n.CreateMapWithReference(ReferenceType.Server, LoadServer) },
            { "channels", (a, n) => a.Channels = n.CreateMapWithReference(ReferenceType.Channel, LoadChannel) },
            { "messages", (a, n) => a.Messages = n.CreateMapWithReference(ReferenceType.Message, LoadMessage) },
            { "securitySchemas", (a, n) => a.SecuritySchemes = n.CreateMapWithReference(ReferenceType.SecurityScheme, LoadSecurityScheme) },
            { "parameters", (a, n) => a.Parameters = n.CreateMapWithReference(ReferenceType.Parameter, LoadParameter) },
            { "correlationIds", (a, n) => a.CorrelationIds = n.CreateMapWithReference(ReferenceType.CorrelationId, LoadCorrelationId) },
            { "operationTraits", (a, n) => a.OperationTraits = n.CreateMapWithReference(ReferenceType.OperationTrait, LoadOperationTrait) },
            { "messageTraits", (a, n) => a.MessageTraits = n.CreateMapWithReference(ReferenceType.MessageTrait, LoadMessageTrait) },

            //{"serverBindings", (a, n) => a.ServerBindings = n.CreateMapWithReference(ReferenceType.ServerBinding, LoadServerBinding)}, // TODO: Do something with bindings
            //{"channelBindings", (a, n) => a.ChannelBindings = n.CreateMapWithReference(ReferenceType.ChannelBinding, LoadChannelBinding)},
            //{"operationBindings", (a, n) => a.OperationBindings = n.CreateMapWithReference(ReferenceType.OperationBinding, LoadOperationBinding)},
            //{"messageBindings", (a, n) => a.MessageBindings = n.CreateMapWithReference(ReferenceType.MessageBinding, LoadMessageBinding)},
        };


        private static PatternFieldMap<AsyncApiComponents> componentsPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiComponents LoadComponents(ParseNode node)
        {
            var mapNode = node.CheckMapNode("components");
            var components = new AsyncApiComponents();

            ParseMap(mapNode, components, componentsFixedFields, componentsPatternFields);

            return components;
        }
    }
}
