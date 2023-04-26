// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    internal static partial class AsyncApiV2Deserializer
    {
        internal static AsyncApiBindings<IServerBinding> LoadServerBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("serverBinding");

            var serverBindings = new AsyncApiBindings<IServerBinding>();

            foreach (var property in mapNode)
            {
                var serverBinding = LoadServerBinding(property);

                if (serverBinding != null)
                {
                    serverBindings.Add(serverBinding);
                }
                else
                {
                    mapNode.Context.Diagnostic.Errors.Add(
                        new AsyncApiError(node.Context.GetLocation(), $"ServerBinding {property.Name} is not found"));
                }
            }

            return serverBindings;
        }

        internal static IServerBinding LoadServerBinding(ParseNode node)
        {
            var property = node as PropertyNode;
            var bindingType = property.Name;
            switch (bindingType)
            {
                case "kafka":
                    return LoadBinding("ServerBinding", property.Value, kafkaServerBindingFixedFields);
                // TODO: case BindingType.Pulsar:
                //    return LoadBinding("ServerBinding", property.Value, pulsarServerBindingFixedFields); 
                default:
                    throw new AsyncApiException($"ServerBinding {property.Name} is not supported");
            }
        }
    }
}
