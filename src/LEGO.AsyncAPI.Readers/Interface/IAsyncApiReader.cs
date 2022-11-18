namespace LEGO.AsyncAPI.Readers.Interface
{
    using LEGO.AsyncAPI.Models;

    public interface IAsyncApiReader<TInput, TDiagnostic> where TDiagnostic : IDiagnostic
    {
        AsyncApiDocument Read(TInput input, out TDiagnostic diagnostic);
    }
}