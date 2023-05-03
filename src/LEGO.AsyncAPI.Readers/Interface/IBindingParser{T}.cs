// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Interface
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public interface IBindingParser<out T> : IBinding
    {
        T LoadBinding(PropertyNode node);
    }
}
