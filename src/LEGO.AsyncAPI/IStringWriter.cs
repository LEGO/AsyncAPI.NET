// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    /// <summary>
    /// AsyncAPI object to string writer.
    /// </summary>
    /// <typeparam name="T">AsyncApi object type.</typeparam>
    public interface IStringWriter<T>
    {
        /// <summary>
        /// Serializes AsyncAPI object of type T into string.
        /// </summary>
        /// <param name="object">AsyncAPI object.</param>
        /// <returns>Serialized string.</returns>
        string Write(T @object);
    }
}