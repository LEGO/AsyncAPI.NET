// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using System.Collections.Generic;

    /// <summary>
    /// Async API object.
    /// </summary>
    public class AsyncAPIObject : Dictionary<string, IAny>, IAny
    {
        /// <summary>
        /// Type of <see cref="IAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Object;
    }
}