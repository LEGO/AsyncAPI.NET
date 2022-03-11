// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using System;

    /// <summary>
    /// Async API null.
    /// </summary>
    public struct Null : IPrimitiveValue<string>
    {
        /// <summary>
        /// Static instance of Null class.
        /// </summary>
        public static readonly Null Instance = new();

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public AnyType AnyType => AnyType.Primitive;

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType => PrimitiveType.Null;

        /// <summary>
        /// Null.
        /// </summary>
        public string Value
        {
            get => null;
            set => throw new NotImplementedException();
        }
    }
}