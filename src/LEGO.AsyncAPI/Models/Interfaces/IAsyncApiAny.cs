// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Models.Any;

    /// <summary>
    /// Base interface for all the types that represent AsyncAPI Any.
    /// </summary>
    public interface IAsyncApiAny : IAsyncApiElement, IAsyncApiExtension
    {
        /// <summary>
        /// Gets type of an <see cref="IAsyncApiAny"/>.
        /// </summary>
        AnyType AnyType { get; }
    }
}