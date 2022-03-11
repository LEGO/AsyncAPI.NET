// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using System.IO;

    /// <summary>
    /// Generic stream reader.
    /// </summary>
    /// <typeparam name="T">Generic object type.</typeparam>
    public interface IStreamReader<T>
    {
        /// <summary>
        /// Deserializes stream into object of type T.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <returns>Object of type T.</returns>
        public T Read(Stream stream);
    }
}