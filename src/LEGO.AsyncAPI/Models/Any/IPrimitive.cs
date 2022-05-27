// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Primitive interface for all AsyncApi primitive types.
    /// </summary>
    public interface IPrimitive : IAsyncApiAny
    {
        /// <summary>
        /// Gets return AnyType.Primitive if not hidden.
        /// </summary>
        public new AnyType AnyType => AnyType.Primitive;

        /// <summary>
        /// Gets return PrimitiveType of type.
        /// </summary>
        public PrimitiveType PrimitiveType { get; }
    }
}