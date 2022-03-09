// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using JetBrains.Annotations;

    /// <summary>
    /// Async API null.
    /// </summary>
    public struct Null : IPrimitiveValue<string>
    {
        public static readonly Null Instance = new ();

        /// <summary>
        /// Default null primitive type constructor.
        /// </summary>
        public Null()
        {
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Primitive;

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType => PrimitiveType.Null;

        [CanBeNull]
        public string Value
        {
            get => null;
            set => throw new NotImplementedException();
        }
    }
}