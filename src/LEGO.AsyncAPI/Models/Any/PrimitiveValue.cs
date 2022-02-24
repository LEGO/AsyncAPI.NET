// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    public interface PrimitiveValue<T> : Primitive
    {
        public T? Value { get; set; }
    }
}