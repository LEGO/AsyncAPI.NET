// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    /// <summary>
    /// Generic string writer.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public interface IStringWriter<T>
    {
        /// <summary>
        /// Serializes object of type T into string.
        /// </summary>
        /// <param name="object">Object.</param>
        /// <returns>Serialized string.</returns>
        string Write(T @object);
    }
}