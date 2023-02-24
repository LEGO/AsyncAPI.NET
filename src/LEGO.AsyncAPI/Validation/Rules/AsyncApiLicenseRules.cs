namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiLicenseRules
    {
        public static ValidationRule<AsyncApiLicense> LicenseRequiredFields =>
            new ValidationRule<AsyncApiLicense>(
                (context, license) =>
                {
                    context.Enter("name");
                    if (license.Name == null)
                    {
                        context.CreateError(
                            nameof(LicenseRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "name", "license"));
                    }

                    context.Exit();
                });

        public static ValidationRule<AsyncApiLicense> LicenseUrlMustBeAbsolute =>
           new ValidationRule<AsyncApiLicense>(
               (context, license) =>
               {

                   context.Enter("url");
                   if (license.Url != null && !license.Url.IsAbsoluteUri)
                   {
                       context.CreateError(
                            nameof(LicenseUrlMustBeAbsolute),
                            string.Format(Resource.Validation_MustBeAbsoluteUrl, "url", "license"));

                   }

                   context.Exit();
               });
    }
}
