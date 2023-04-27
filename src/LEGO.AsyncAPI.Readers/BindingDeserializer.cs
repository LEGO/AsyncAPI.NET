// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public class BindingDeserializer
    {
        private static Type messageBindingType = typeof(IMessageBinding);
        private static Type operationBindingType = typeof(IOperationBinding);
        private static Type channelBindingType = typeof(IChannelBinding);
        private static Type serverBindingType = typeof(IServerBinding);

        private static PatternFieldMap<T> BindingPatternExtensionFields<T>()
    where T : IBinding, new()
        {
            return new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, AsyncApiV2Deserializer.LoadExtension(p, n)) },
            };
        }

        public static T LoadBinding<T>(string nodeName, ParseNode node, FixedFieldMap<T> fieldMap)
   where T : IBinding, new()
        {
            var mapNode = node.CheckMapNode(nodeName);
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                ReferenceType referenceType = ReferenceType.None;
                var bindingType = typeof(T);

                if (bindingType.IsAssignableTo(messageBindingType))
                {
                    referenceType = ReferenceType.MessageBinding;
                }

                if (bindingType.IsAssignableTo(operationBindingType))
                {
                    referenceType = ReferenceType.OperationBinding;
                }

                if (bindingType.IsAssignableTo(channelBindingType))
                {
                    referenceType = ReferenceType.ChannelBinding;
                }

                if (bindingType.IsAssignableTo(serverBindingType))
                {
                    referenceType = ReferenceType.ServerBinding;
                }

                if (referenceType == ReferenceType.None)
                {
                    throw new ArgumentException($"ReferenceType not found {typeof(T).Name}");
                }

                return mapNode.GetReferencedObject<T>(referenceType, pointer);
            }

            var binding = new T();

            AsyncApiV2Deserializer.ParseMap(mapNode, binding, fieldMap, BindingPatternExtensionFields<T>());

            return binding;
        }
    }
}
