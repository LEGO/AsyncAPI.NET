// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    public interface IPrimitiveValue<T> : IPrimitive
    {
        public T? Value { get; set; }
    }
}