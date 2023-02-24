// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings;
    using LEGO.AsyncAPI.Models.Bindings.Http;
    using LEGO.AsyncAPI.Models.Bindings.Kafka;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    internal static partial class AsyncApiV2Deserializer
    {
        internal static AsyncApiBindings<IMessageBinding> LoadMessageBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("messageBindings");

            var messageBindings = new AsyncApiBindings<IMessageBinding>();

            foreach (var property in mapNode)
            {
                var messageBinding = LoadMessageBinding(property);

                if (messageBinding != null)
                {
                    messageBindings.Add(messageBinding);
                }
                else
                {
                    mapNode.Context.Diagnostic.Errors.Add(
                        new AsyncApiError(node.Context.GetLocation(), $"MessageBinding {property.Name} is not found"));
                }
            }

            return messageBindings;
        }

        internal static IMessageBinding LoadMessageBinding(ParseNode node)
        {
            var property = node as PropertyNode;
            var bindingType = property.Name.GetEnumFromDisplayName<BindingType>();
            switch (bindingType)
            {
                case BindingType.Kafka:
                    return LoadBinding<KafkaMessageBinding>("MessageBinding", property.Value, kafkaMessageBindingFixedFields);
                case BindingType.Http:
                    return LoadBinding<HttpMessageBinding>("MessageBinding", property.Value, httpMessageBindingFixedFields);
                default:
                    throw new AsyncApiException($"MessageBinding {property.Name} is not supported");
            }
        }
    }
}
