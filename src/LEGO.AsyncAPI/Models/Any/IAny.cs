// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Any
{
    /// <summary>
    /// Base interface for all the types that represent AsyncAPI Any.
    /// </summary>
    public interface IAny
    {
        /// <summary>
        /// Type of an <see cref="IAny"/>.
        /// </summary>
        AnyType AnyType { get; }
    }
}