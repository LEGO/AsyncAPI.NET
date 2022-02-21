namespace LEGO.AsyncAPI
{
    using System.Text.Json;
    using Json.Schema;

    public class AsyncApiSchemaValidator
    {
        private const string SampleFolderPath = "";

        public async Task<ValidationResults> Validate(JsonDocument jsonDocument)
        {
            var type = typeof(AsyncApiSchemaValidator);
            var schemaPath = type.Namespace + "." +
                             Path.Combine(SampleFolderPath, "schema-v2.3.0.json").Replace("/", ".");
            Stream schemaStream = type.Assembly.GetManifestResourceStream(schemaPath);

            var schema = JsonSchema.FromText(new StreamReader(schemaStream).ReadToEnd());


            var json = jsonDocument;

            var options = new ValidationOptions();
            options.DefaultBaseUri =
                new Uri("https://raw.githubusercontent.com/asyncapi/spec-json-schemas/master/schemas/2.3.0.json");
            var result = schema.Validate(json.RootElement, options);
            return result;
        }
    }
}