// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Interfaces
{
    /// <summary>
    /// Primitive type.
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// Integer
        /// </summary>
        Integer,

        /// <summary>
        /// Long
        /// </summary>
        Long,

        /// <summary>
        /// Float
        /// </summary>
        Float,

        /// <summary>
        /// Double
        /// </summary>
        Double,

        /// <summary>
        /// String
        /// </summary>
        String,

        /// <summary>
        /// Byte
        /// </summary>
        Byte,

        /// <summary>
        /// Binary
        /// </summary>
        Binary,

        /// <summary>
        /// Boolean
        /// </summary>
        Boolean,

        /// <summary>
        /// Date
        /// </summary>
        Date,

        /// <summary>
        /// DateTime
        /// </summary>
        DateTime,
    }

    /// <summary>
    /// Base interface for the Primitive type.
    /// </summary>
    public interface IAsyncApiPrimitive : IAsyncApiAny
    {
        /// <summary>
        /// Primitive type.
        /// </summary>
        PrimitiveType PrimitiveType { get; }
    }
}
