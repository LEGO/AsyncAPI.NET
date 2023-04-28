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
        internal static AsyncApiBindings<IOperationBinding> LoadOperationBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("operationBindings");

            var operationBindings = new AsyncApiBindings<IOperationBinding>();

            foreach (var property in mapNode)
            {
                var operationBinding = LoadOperationBinding(property);

                if (operationBinding != null)
                {
                    operationBindings.Add(operationBinding);
                }
                else
                {
                    mapNode.Context.Diagnostic.Errors.Add(
                        new AsyncApiError(node.Context.GetLocation(), $"OperationBinding {property.Name} is not found"));
                }
            }

            return operationBindings;
        }

        internal static IOperationBinding LoadOperationBinding(ParseNode node)
        {
            var property = node as PropertyNode;
            try
            {
                if (node.Context.OperationBindingParsers.TryGetValue(property.Name, out var parser))
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
