// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Type of an <see cref="IAsyncApiAny"/>.
    /// </summary>
    public enum AnyType
    {
        /// <summary>
        /// Primitive.
        /// </summary>
        Primitive,

        /// <summary>
        /// Null.
        /// </summary>
        Null,

        /// <summary>
        /// Array.
        /// </summary>
        Array,

        /// <summary>
        /// Object.
        /// </summary>
        Object,
    }
}