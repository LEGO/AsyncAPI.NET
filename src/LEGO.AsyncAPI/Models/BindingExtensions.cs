// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;

    public static class BindingExtensions
    {
        public static bool TryGetValue<TBinding>(this AsyncApiBindings<IServerBinding> bindings, out TBinding binding)
            where TBinding : class, IServerBinding
        {
            if (bindings.TryGetValue(Activator.CreateInstance<TBinding>().BindingKey, out var serverBinding))
            {
                binding = serverBinding as TBinding;
                return true;
            }

            binding = default;
            return false;
        }

        public static bool TryGetValue<TBinding>(this AsyncApiBindings<IChannelBinding> bindings, out TBinding binding)
    where TBinding : class, IChannelBinding
        {
            if (bindings.TryGetValue(Activator.CreateInstance<TBinding>().BindingKey, out var channelBinding))
            {
                binding = channelBinding as TBinding;
                return true;
            }

            binding = default;
            return false;
        }

        public static bool TryGetValue<TBinding>(this AsyncApiBindings<IOperationBinding> bindings, out TBinding binding)
    where TBinding : class, IOperationBinding
        {
            if (bindings.TryGetValue(Activator.CreateInstance<TBinding>().BindingKey, out var operationBinding))
            {
                binding = operationBinding as TBinding;
                return true;
            }

            binding = default;
            return false;
        }

        public static bool TryGetValue<TBinding>(this AsyncApiBindings<IMessageBinding> bindings, out TBinding binding)
    where TBinding : class, IMessageBinding
        {
            if (bindings.TryGetValue(Activator.CreateInstance<TBinding>().BindingKey, out var messageBinding))
            {
                binding = messageBinding as TBinding;
                return true;
            }

            binding = default;
            return false;
        }
    }
}
