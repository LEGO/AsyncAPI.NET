// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        internal static AsyncApiBindings<IMessageBinding> LoadMessageBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("messageBindings");
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return new AsyncApiBindingsReference<IMessageBinding>(pointer);
            }

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
            try
            {
                if (node.Context.MessageBindingParsers.TryGetValue(property.Name, out var parser))
                {
                    return parser.LoadBinding(property);
                }
            }
            catch (AsyncApiException ex)
            {
                ex.Pointer = node.Context.GetLocation();
                node.Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
            }

            return null;
        }
    }
}
