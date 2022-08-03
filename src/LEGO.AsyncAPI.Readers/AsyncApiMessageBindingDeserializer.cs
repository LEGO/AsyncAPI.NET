// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    internal static partial class AsyncApiDeserializer
    {
        private static PatternFieldMap<T> BindingPatternExtensionFields<T>() where T : IMessageBinding, new()
        {
            return new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };
        }

        private static FixedFieldMap<HttpMessageBinding> httpBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "headers", (a, n) => { a.Headers = LoadSchema(n); } },
        };

        private static FixedFieldMap<KafkaMessageBinding> kafkaBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "key", (a, n) => { a.Key = LoadSchema(n); } },
        };

        internal static AsyncApiMessageBindings LoadMessageBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("messageBindings");

            var messageBindings = new AsyncApiMessageBindings();

            foreach (var property in mapNode)
            {
                var bindingType = property.Name.GetEnumFromDisplayName<MessageBindingType>();
                IMessageBinding messageBinding = null;
                switch (bindingType)
                {
                    case MessageBindingType.Kafka:
                        messageBinding = LoadBinding<KafkaMessageBinding>(property.Value, kafkaBindingFixedFields);
                        break;
                    case MessageBindingType.Http:
                        messageBinding = LoadBinding<HttpMessageBinding>(property.Value, httpBindingFixedFields);
                        break;
                    default:
                        throw new System.Exception("MessageBinding not found");
                }

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

        internal static T LoadBinding<T>(ParseNode node, FixedFieldMap<T> fieldMap) where T : IMessageBinding, new()
        {
            var mapNode = node.CheckMapNode("MessageBinding");
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<T>(ReferenceType.MessageBinding, pointer);
            }

            var messageBinding = new T();

            ParseMap(mapNode, messageBinding, fieldMap, BindingPatternExtensionFields<T>());

            return messageBinding;
        }
    }
}