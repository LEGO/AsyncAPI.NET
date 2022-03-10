// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Primitive interface for all AsyncApi primitive types.
    /// </summary>
    public interface IPrimitive : IAny
    {
        /// <summary>
        /// Return AnyType.Primitive if not hidden.
        /// </summary>
        public new AnyType AnyType => AnyType.Primitive;

        /// <summary>
        /// Return PrimitiveType of type.
        /// </summary>
        public PrimitiveType PrimitiveType { get; }
    }
}