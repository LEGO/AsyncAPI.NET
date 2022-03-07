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

        public Null()
        {
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Primitive;

        public PrimitiveType PrimitiveType => PrimitiveType.Null;

        [CanBeNull]
        public string Value
        {
            get => null;
            set => throw new NotImplementedException();
        }
    }
}