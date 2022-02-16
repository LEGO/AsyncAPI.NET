namespace LEGO.AsyncAPI.Readers.Interface;

using Models;

public interface IAsyncApiReader<TInput>
{
    AsyncApiDocument Read(TInput input, out AsyncApiDiagnostic diagnostic);
}