// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Bindings
{
    public abstract class MessageBinding<T> : Binding<T>, IMessageBinding
        where T : IMessageBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override T LoadBinding(PropertyNode node) => BindingDeserializer.LoadBinding("MessageBinding", node.Value, this.FixedFieldMap);
    }
}
