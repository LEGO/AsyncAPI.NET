namespace LEGO.AsyncAPI.Surface;

using System.Text.Json;
using Newtonsoft.Json.Linq;

public interface IReader
{
    Task<ReaderReadResult> ReadAsync(Stream stream, CancellationToken cancellationToken);
    Task<ReaderReadResult> ReadAsync(JsonDocument jsonDocument, CancellationToken cancellationToken);
    Task<ReaderReadResult> ReadAsync(JObject jObject, CancellationToken cancellationToken);
    Task<ReaderReadResult> ReadAsync(string jsonAsyncApiDefinition, CancellationToken cancellationToken);
}