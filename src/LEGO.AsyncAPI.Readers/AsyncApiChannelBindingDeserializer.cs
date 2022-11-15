// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings;
    using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<KafkaChannelBinding> kafkaChannelBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "topic", (a, n) => { a.Topic = n.GetScalarValue(); } },
            { "partitions", (a, n) => { a.Topic = n.GetScalarValue(); } },
            { "replicas", (a, n) => { a.Topic = n.GetScalarValue(); } },
        };

        internal static AsyncApiBindings<IChannelBinding> LoadChannelBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("channelBinding");

            var channelBindings = new AsyncApiBindings<IChannelBinding>();

            foreach (var property in mapNode)
            {
                var bindingType = property.Name.GetEnumFromDisplayName<BindingType>();
                IChannelBinding channelBinding = null;
                switch (bindingType)
                {
                    case BindingType.Kafka:
                        channelBinding = LoadBinding("ChannelBinding", property.Value, kafkaChannelBindingFixedFields);
                        break;
                    default:
                        throw new System.Exception("ChannelBinding not found");
                }

                if (channelBinding != null)
                {
                    channelBindings.Add(channelBinding);
                }
                else
                {
                    mapNode.Context.Diagnostic.Errors.Add(
                        new AsyncApiError(node.Context.GetLocation(), $"ChannelBinding {property.Name} is not found"));
                }
            }

            return channelBindings;
        }
    }
}