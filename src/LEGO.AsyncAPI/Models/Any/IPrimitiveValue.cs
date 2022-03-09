// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Fpr AsyncApi primitive types that have value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPrimitiveValue<T> : IPrimitive
    {
        public T? Value { get; set; }
    }
}