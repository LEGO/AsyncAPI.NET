// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public abstract class OperationBinding<T> : Binding<T>, IOperationBinding
        where T : IOperationBinding, new()
    {
        protected abstract FixedFieldMap<T> FixedFieldMap { get; }

        public override T LoadBinding(PropertyNode node) => BindingDeserializer.LoadBinding("OperationBinding", node.Value, this.FixedFieldMap);
    }
}
