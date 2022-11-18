// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    internal static partial class AsyncApiDeserializer
    {
        internal static AsyncApiBindings<IOperationBinding> LoadOperationBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("operationBinding");

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
            var bindingType = property.Name.GetEnumFromDisplayName<BindingType>();
            switch (bindingType)
            {
                case BindingType.Kafka:
                    return LoadBinding("OperationBinding", property.Value, kafkaOperationBindingFixedFields);
                case BindingType.Http:
                    return LoadBinding("OperationBinding", property.Value, httpOperationBindingFixedFields);
                default:
                    throw new System.Exception("OperationBinding not found");
            }
        }
    }
}
