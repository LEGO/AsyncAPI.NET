namespace LEGO.AsyncAPI.Surface.Stubs
{
    using System.Text.Json;
    using Json.Schema;

    public interface IAsyncApiSchemaValidator
    {
        Task<ValidationResults> ValidateAsync(JsonDocument jsonDocument);
    }
}