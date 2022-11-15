// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings;
    using LEGO.AsyncAPI.Models.Bindings.ServerBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<KafkaServerBinding> kafkaServerBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "schemaRegistryUrl", (a, n) => { a.SchemaRegistryUrl = n.GetScalarValue(); } },
            { "schemaRegistryVendor", (a, n) => { a.SchemaRegistryVendor = n.GetScalarValue(); } },
        };

        internal static AsyncApiBindings<IServerBinding> LoadServerBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("serverBinding");

            var serverBindings = new AsyncApiBindings<IServerBinding>();

            foreach (var property in mapNode)
            {
                var bindingType = property.Name.GetEnumFromDisplayName<BindingType>();
                IServerBinding serverBinding = null;
                switch (bindingType)
                {
                    case BindingType.Kafka:
                        serverBinding = LoadBinding("ServerBinding", property.Value, kafkaServerBindingFixedFields);
                        break;
                    default:
                        throw new System.Exception("ServerBinding not found");
                }

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
    }
}