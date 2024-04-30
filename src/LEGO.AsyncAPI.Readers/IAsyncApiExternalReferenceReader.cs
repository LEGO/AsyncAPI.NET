namespace LEGO.AsyncAPI.Readers;

/// <summary>
/// Interface that provides method for reading external references.å
/// </summary>
public interface IAsyncApiExternalReferenceReader
{
    /// <summary>
    /// Method that returns the AsyncAPI content that the external reference from the $ref points to.
    /// </summary>
    /// <param name="reference">The content address of the $ref.</param>
    /// <returns>The content of the reference as a string.</returns>
    public string GetExternalResource(string reference);
}