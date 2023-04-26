// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers.Interface
{
    public interface IBindingParser<T> : IBinding
        where T : IBinding
    {
        T Parse(PropertyNode node);
    }
}