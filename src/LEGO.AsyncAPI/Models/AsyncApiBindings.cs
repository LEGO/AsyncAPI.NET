// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public static class BindingExtensions
    {
        public static bool TryGetValue<TBinding>(this AsyncApiBindings<IServerBinding> bindings, out IServerBinding binding)
            where TBinding : IServerBinding
        {
            return bindings.TryGetValue(Activator.CreateInstance<TBinding>().BindingKey, out binding);
        }

        public static bool TryGetValue<TBinding>(this AsyncApiBindings<IChannelBinding> bindings, out IChannelBinding binding)
    where TBinding : IChannelBinding
        {
            return bindings.TryGetValue(Activator.CreateInstance<TBinding>().BindingKey, out binding);
        }

        public static bool TryGetValue<TBinding>(this AsyncApiBindings<IOperationBinding> bindings, out IOperationBinding binding)
    where TBinding : IOperationBinding
        {
            return bindings.TryGetValue(Activator.CreateInstance<TBinding>().BindingKey, out binding);
        }

        public static bool TryGetValue<TBinding>(this AsyncApiBindings<IMessageBinding> bindings, out IMessageBinding binding)
    where TBinding : IMessageBinding
        {
            return bindings.TryGetValue(Activator.CreateInstance<TBinding>().BindingKey, out binding);
        }
    }

    public class AsyncApiBindings<TBinding> : Dictionary<string, TBinding>, IAsyncApiReferenceable
        where TBinding : IBinding
    {
        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public void Add(TBinding binding)
        {
            this[binding.BindingKey] = binding;
        }

        public void SerializeV2(IAsyncApiWriter writer)
        {

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
            {
                this.Reference.SerializeV2(writer);
                return;
            }

            this.SerializeV2WithoutReference(writer);
        }

        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            foreach (var binding in this)
            {
                var bindingType = binding.Key;
                var bindingValue = binding.Value;

                writer.WritePropertyName(bindingType);

                bindingValue.SerializeV2(writer);
            }

            writer.WriteEndObject();
        }
    }
}
