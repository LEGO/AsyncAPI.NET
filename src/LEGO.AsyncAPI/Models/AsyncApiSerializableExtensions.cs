// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public static class AsyncApiSerializableExtensions
    {
        /// <summary>
        /// Serialize the <see cref="IAsyncApiSerializable"/> to the AsyncApi document (JSON) using the given stream and specification version.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="specificationVersion">The AsyncApi specification version.</param>
        [Obsolete($"Please use overridden version that accepts a {nameof(AsyncApiWriterSettings)} instance.")]
        public static void SerializeAsJson<T>(this T element, Stream stream, AsyncApiVersion specificationVersion)
            where T : IAsyncApiSerializable
        {
            element.SerializeAsJson(stream, specificationVersion, AsyncApiWriterSettings.Default);
        }

        /// <summary>
        /// Serialize the <see cref="IAsyncApiSerializable"/> to the AsyncApi document (JSON) using the given stream and specification version.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="specificationVersion">The AsyncApi specification version.</param>
        /// <param name="settings">The settings used for writing</param>
        public static void SerializeAsJson<T>(this T element, Stream stream, AsyncApiVersion specificationVersion, AsyncApiWriterSettings settings)
            where T : IAsyncApiSerializable
        {
            element.Serialize(stream, specificationVersion, AsyncApiFormat.Json, settings);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the AsyncApi document (YAML) using the given stream and specification version.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="specificationVersion">The AsyncApi specification version.</param>
        [Obsolete($"Please use overridden version that accepts a {nameof(AsyncApiWriterSettings)} instance.")]
        public static void SerializeAsYaml<T>(this T element, Stream stream, AsyncApiVersion specificationVersion)
            where T : IAsyncApiSerializable
        {
            element.SerializeAsYaml(stream, specificationVersion, AsyncApiWriterSettings.Default);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the AsyncApi document (YAML) using the given stream and specification version.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="specificationVersion">The AsyncApi specification version.</param>
        /// <param name="settings">The settings used for writing</param>
        public static void SerializeAsYaml<T>(this T element, Stream stream, AsyncApiVersion specificationVersion, AsyncApiWriterSettings settings)
            where T : IAsyncApiSerializable
        {
            element.Serialize(stream, specificationVersion, AsyncApiFormat.Yaml, settings);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the AsyncApi document using
        /// the given stream, specification version and the format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="stream">The given stream.</param>
        /// <param name="specificationVersion">The AsyncApi specification version.</param>
        /// <param name="format">The output format (JSON or YAML).</param>
        [Obsolete($"Please use overridden version that accepts a {nameof(AsyncApiWriterSettings)} instance.")]
        public static void Serialize<T>(
            this T element,
            Stream stream,
            AsyncApiVersion specificationVersion,
            AsyncApiFormat format)
            where T : IAsyncApiSerializable
        {
            element.Serialize(stream, specificationVersion, format, new AsyncApiWriterSettings());
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the AsyncApi document using
        /// the given stream, specification version and the format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="stream">The given stream.</param>
        /// <param name="specificationVersion">The AsyncApi specification version.</param>
        /// <param name="format">The output format (JSON or YAML).</param>
        /// <param name="settings">Provide configuration settings for controlling writing output.</param>
        public static void Serialize<T>(
            this T element,
            Stream stream,
            AsyncApiVersion specificationVersion,
            AsyncApiFormat format,
            AsyncApiWriterSettings settings)
            where T : IAsyncApiSerializable
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var streamWriter = new FormattingStreamWriter(stream, settings.CultureInfo);

            IAsyncApiWriter writer = format switch
            {
                AsyncApiFormat.Json => new AsyncApiJsonWriter(streamWriter, settings, false),
                AsyncApiFormat.Yaml => new AsyncApiYamlWriter(streamWriter, settings),
                _ => throw new AsyncApiException(string.Format("The given AsyncApi format '{0}' is not supported.", format)),
            };
            element.Serialize(writer, specificationVersion);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable" /> to AsyncApi document using the given specification version and writer.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable" />.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="writer">The output writer.</param>
        /// <param name="specificationVersion">The specification version.</param>
        /// <exception cref="System.ArgumentNullException">
        /// element
        /// or
        /// writer.
        /// </exception>
        /// <exception cref="LEGO.AsyncAPI.Exceptions.AsyncApiException">specification version '{specificationVersion}' is not supported.</exception>
        public static void Serialize<T>(this T element, IAsyncApiWriter writer, AsyncApiVersion specificationVersion)
            where T : IAsyncApiSerializable
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            switch (specificationVersion)
            {
                case AsyncApiVersion.AsyncApi2_0:
                     element.SerializeV2(writer);
                     break;
                default:
                     throw new AsyncApiException($"specification version '{specificationVersion}' is not supported.");
            }

            writer.Flush();
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the AsyncApi document as a string in JSON format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="specVersion">The AsyncApi specification version.</param>
        public static string SerializeAsJson<T>(
            this T element,
            AsyncApiVersion specificationVersion)
            where T : IAsyncApiSerializable
        {
            return element.Serialize(specificationVersion, AsyncApiFormat.Json);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the AsyncApi document as a string in YAML format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="specVersion">The AsyncApi specification version.</param>
        public static string SerializeAsYaml<T>(
            this T element,
            AsyncApiVersion specificationVersion)
            where T : IAsyncApiSerializable
        {
            return element.Serialize(specificationVersion, AsyncApiFormat.Yaml);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the AsyncApi document as a string in the given format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/>.</typeparam>
        /// <param name="element">The AsyncApi element.</param>
        /// <param name="specVersion">The AsyncApi specification version.</param>
        /// <param name="format">AsyncApi document format.</param>
        public static string Serialize<T>(
            this T element,
            AsyncApiVersion specificationVersion,
            AsyncApiFormat format)
            where T : IAsyncApiSerializable
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            using (var stream = new MemoryStream())
            {
                element.Serialize(stream, specificationVersion, format);
                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
