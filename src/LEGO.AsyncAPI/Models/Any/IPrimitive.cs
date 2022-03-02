// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    public interface IPrimitive : IAny
    {
        public new AnyType AnyType => AnyType.Primitive;

        public PrimitiveType PrimitiveType { get; }
    }
}