// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Fpr AsyncApi primitive types that have value.
    /// </summary>
    /// <typeparam name="T">Type of primitive value.</typeparam>
    public interface IPrimitiveValue<T> : IPrimitive
    {
        /// <summary>
        /// Value.
        /// </summary>
        public T? Value { get; set; }
    }
}