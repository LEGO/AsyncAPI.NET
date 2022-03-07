// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    public struct Boolean : IPrimitiveValue<bool>
    {
        public Boolean(bool value)
            : this()
        {
            this.Value = value;
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType { get; } = PrimitiveType.Boolean;

        public bool Value { get; set; }

        public AnyType AnyType => AnyType.Primitive;

        public static explicit operator bool(Boolean b) => b.Value;

        public static explicit operator Boolean(bool b) => new (value: b);
    }
}