namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiExternalDocumentationRules
    {
        public static ValidationRule<AsyncApiExternalDocumentation> ExternalDocumentationRequiredFields =>
            new ValidationRule<AsyncApiExternalDocumentation>(
                (context, externalDocumentation) =>
                {
                    context.Enter("url");
                    if (externalDocumentation.Url == null)
                    {
                        context.CreateError(
                            nameof(ExternalDocumentationRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "url", "externalDocumentation"));
                    }

                    context.Exit();

                });

        public static ValidationRule<AsyncApiExternalDocumentation> ExternalDocumentationUrlMustBeAbsolute =>
           new ValidationRule<AsyncApiExternalDocumentation>(
               (context, externalDocumentation) =>
               {
                   context.Enter("url");
                   if (externalDocumentation.Url != null && !externalDocumentation.Url.IsAbsoluteUri)
                   {
                       context.CreateError(
                            nameof(ExternalDocumentationUrlMustBeAbsolute),
                            string.Format(Resource.Validation_MustBeAbsoluteUrl, "url", "externalDocumentation"));

                   }

                   context.Exit();
               });
    }
}