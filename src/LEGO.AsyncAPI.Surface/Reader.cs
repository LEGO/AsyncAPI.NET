namespace LEGO.AsyncAPI.Surface;

using System.Text;
using System.Text.Json;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stubs;

public class Reader : IReader
{
    private readonly IAsyncApiSchemaValidator _apiSchemaValidator;
    private readonly IAsyncApiReader<AsyncApiDocument> _asyncApiReader;

    public Reader(IAsyncApiSchemaValidator apiSchemaValidator, IAsyncApiReader<AsyncApiDocument> asyncApiReader)
    {
        _apiSchemaValidator = apiSchemaValidator;
        _asyncApiReader = asyncApiReader;
    }

    public async Task<ReaderReadResult> ReadAsync(JObject jObject, CancellationToken cancellationToken)
    {
        return await ValidateAndReadJObject(jObject, cancellationToken);
    }

    public async Task<ReaderReadResult> ReadAsync(string jsonAsyncApiDefinition, CancellationToken cancellationToken)
    {
        JObject jObject;

        try
        {
            jObject = JObject.Parse(jsonAsyncApiDefinition);
        }
        catch (Exception e)
        {
            return new ReaderReadResult { DiagnosticObject = DiagnosticObject.OnParseError(e) };
        }

        return await ValidateAndReadJObject(jObject, cancellationToken);
    }

    public async Task<ReaderReadResult> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        JObject jObject;

        try
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                jObject = await JObject.LoadAsync(jsonTextReader, cancellationToken);
            }
        }
        catch (Exception e)
        {
            return new ReaderReadResult { DiagnosticObject = DiagnosticObject.OnParseError(e) };
        }

        return await ValidateAndReadJObject(jObject, cancellationToken);
    }

    public Task<ReaderReadResult> ReadAsync(JsonDocument jsonDocument, CancellationToken cancellationToken)
    {
        string jsonString;

        using (var stream = new MemoryStream())
        {
            var writer = new Utf8JsonWriter(stream);
            jsonDocument.WriteTo(writer);
            writer.Flush();
            jsonString = Encoding.UTF8.GetString(stream.ToArray());
        }

        return ReadAsync(jsonString, cancellationToken);
    }

    private async Task<ReaderReadResult> ValidateAndReadJObject(JObject jObject, CancellationToken cancellationToken)
    {
        var validatorResults = await _apiSchemaValidator.ValidateAsync(jObject, cancellationToken);

        if (!validatorResults.IsValid)
        {
            return new ReaderReadResult { DiagnosticObject = DiagnosticObject.OnValidateError(validatorResults) };
        }

        AsyncApiDocument deserializationResult;

        try
        {
            deserializationResult = _asyncApiReader.Consume(jObject);
        }
        catch (Exception e)
        {
            return new ReaderReadResult { DiagnosticObject = DiagnosticObject.OnReadError(e) };
        }

        return new ReaderReadResult { Document = deserializationResult };
    }
}