using LEGO.AsyncAPI.Models;

namespace LEGO.AsyncAPI.Readers.Interface
{
    public interface IAsyncApiReader<TInput, TDiagnostic> where TDiagnostic : IDiagnostic
    {
        AsyncApiDocument Read(TInput input, out TDiagnostic diagnostic);
    }
}