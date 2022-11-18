namespace LEGO.AsyncAPI
{
    using System;
    using System.IO;
    using System.Text.Json;
    using Json.Schema;

    /// <summary>
    /// AsyncApi json schema validator, based on schema v2.3.0.
    /// </summary>
    public class AsyncApiSchemaValidator
    {
        private const string SampleFolderPath = "";

        /// <summary>
        /// Validates input against public AsyncApi schema.
        /// </summary>
        /// <param name="jsonDocument">JsonDocument input to validate.</param>
        /// <returns>Validation results of the input.</returns>
        public ValidationResults Validate(JsonDocument jsonDocument)
        {
            var type = typeof(AsyncApiSchemaValidator);
            var schemaPath = type.Namespace + "." +
                             Path.Combine(SampleFolderPath, "schema-v2.3.0.json").Replace("/", ".");
            Stream schemaStream = type.Assembly.GetManifestResourceStream(schemaPath);

            var schema = JsonSchema.FromText(new StreamReader(schemaStream ?? throw new InvalidOperationException("Async Api JSON Schema not found.")).ReadToEnd());

            var json = jsonDocument;

            var options = new ValidationOptions();
            options.DefaultBaseUri = new Uri("https://raw.githubusercontent.com/asyncapi/spec-json-schemas/master/schemas/2.3.0.json");
            var result = schema.Validate(json.RootElement, options);
            return result;
        }
    }
}