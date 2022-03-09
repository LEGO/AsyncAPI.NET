// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    /// <summary>
    /// AsyncApi stream reader.
    /// </summary>
    /// <typeparam name="T">AsyncAPI object type.</typeparam>
    public interface IStreamReader<T>
    {
        /// <summary>
        /// Deserializes stream into AsyncAPI object of type T.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <returns>AsyncAPI object.</returns>
        public T Read(Stream stream);
    }
}