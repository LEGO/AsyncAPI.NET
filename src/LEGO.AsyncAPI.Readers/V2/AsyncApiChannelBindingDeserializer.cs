// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        internal static AsyncApiBindings<IChannelBinding> LoadChannelBindings(ParseNode node)
        {
            var mapNode = node.CheckMapNode("channelBinding");

            var channelBindings = new AsyncApiBindings<IChannelBinding>();

            foreach (var property in mapNode)
            {
                var channelBinding = LoadChannelBinding(property);

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
