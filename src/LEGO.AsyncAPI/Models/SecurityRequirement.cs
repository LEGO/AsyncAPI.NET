namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Allows adding security requirement.
    /// </summary>
    public class SecurityRequirement : IExtensible
    {
        /// <summary>
        /// Additional external documentation for this security requirement.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        public IDictionary<string, IAny> Extensions { get; set; }
    }
}
