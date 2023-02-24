namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiInfoRules
    {
        public static ValidationRule<AsyncApiInfo> AsyncApiInfoRequiredFields =>
            new ValidationRule<AsyncApiInfo>(
                (context, info) =>
                {
                    // title
                    context.Enter("title");
                    if (info.Title == null)
                    {
                        context.CreateError(
                            nameof(AsyncApiInfoRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "title", "info"));
                    }

                    context.Exit();

                    // version
                    context.Enter("version");
                    if (info.Version == null)
                    {
                        context.CreateError(
                            nameof(AsyncApiInfoRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "version", "info"));
                    }

                    context.Exit();
                });

        public static ValidationRule<AsyncApiInfo> TermsOfServiceUrlMustBeAbsolute =>
            new ValidationRule<AsyncApiInfo>(
               (context, info) =>
               {
                   // termsOfService
                   context.Enter("termsOfService");
                   if (info.TermsOfService != null && !info.TermsOfService.IsAbsoluteUri)
                   {
                       context.CreateError(
                            nameof(TermsOfServiceUrlMustBeAbsolute),
                            string.Format(Resource.Validation_MustBeAbsoluteUrl, "termsOfService", "info"));

                   }

                   context.Exit();
               });
    }
}
