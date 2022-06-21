using LEGO.AsyncAPI.Models;

namespace LEGO.AsyncApi.Readers.Interface
{
    public interface IAsyncApiReader<TInput, TDiagnostic> where TDiagnostic : IDiagnostic
    {
        AsyncApiDocument Read(TInput input, out TDiagnostic diagnostic);
    }
}