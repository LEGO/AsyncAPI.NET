namespace LEGO.AsyncAPI.Surface;

using System.Text.Json;
using Models;
using Stubs;

public class Reader
{
    private readonly IAsyncApiSchemaValidator _apiSchemaValidator;
    private readonly IAsyncApiReader<AsyncApiDocument> _asyncApiReader;

    public Reader(IAsyncApiSchemaValidator apiSchemaValidator, IAsyncApiReader<AsyncApiDocument> asyncApiReader)
    {
        _apiSchemaValidator = apiSchemaValidator;
        _asyncApiReader = asyncApiReader;
    }

    public async Task<ReaderReadResult> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        JsonDocument jsonDocument;

        try
        {
            jsonDocument = await JsonDocument.ParseAsync(stream, default, cancellationToken);
        }
        catch (Exception e)
        {
            return new ReaderReadResult { DiagnosticObject = DiagnosticObject.OnParseError(e) };
        }

        var validateResults = await _apiSchemaValidator.ValidateAsync(jsonDocument);

        if (!validateResults.IsValid)
        {
            return new ReaderReadResult
                { DiagnosticObject = DiagnosticObject.OnValidateError(validateResults) };
        }

        AsyncApiDocument deserializationResult;

        try
        {
            deserializationResult = _asyncApiReader.Consume(stream);
        }
        catch (Exception e)
        {
            return new ReaderReadResult { DiagnosticObject = DiagnosticObject.OnReadError(e) };
        }

        return new ReaderReadResult { Document = deserializationResult };
    }
}