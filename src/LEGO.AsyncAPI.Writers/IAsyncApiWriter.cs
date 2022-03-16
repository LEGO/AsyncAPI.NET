namespace LEGO.AsyncAPI.Writers
{
    using Models;

    /// <summary>
    /// Interface for AsyncApi writers.
    /// </summary>
    public interface IAsyncApiWriter
    {
        /// <summary>
        /// Serializes AsyncApiDocument to JSON string.
        /// </summary>
        /// <returns>Serialized AsyncApiDocument.</returns>
        string Write(AsyncApiDocument document, out AsyncApiDiagnostic diagnostic);
    }
}