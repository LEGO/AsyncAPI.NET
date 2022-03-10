// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Interface for AsyncApi readers.
    /// </summary>
    /// <typeparam name="TInput">The type of input to read from.</typeparam>
    public interface IAsyncApiReader<TInput>
    {
        /// <summary>
        /// Reads the input and parses it into an AsyncApi document.
        /// </summary>
        /// <param name="input">The input to read from.</param>
        /// <param name="diagnostic">The diagnostic entity containing information from the reading process.</param>
        /// <returns>The AsyncApiDocument.</returns>
        AsyncApiDocument Read(TInput input, out AsyncApiDiagnostic diagnostic);
    }
}