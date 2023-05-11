// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public abstract class Binding<T> : AsyncApiBinding, IBindingParser<T>
        where T : IBinding, new()
    {
        public abstract T LoadBinding(PropertyNode node);
    }
}
