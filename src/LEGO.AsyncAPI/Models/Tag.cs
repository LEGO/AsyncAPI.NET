namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Allows adding meta data to a single tag.
    /// </summary>
    public class Tag: IExtensible
    {
        /// <summary>
        /// REQUIRED. The name of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A short description for the tag. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Additional external documentation for this tag.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        public IDictionary<string, IAny> Extensions { get; set; }
    }
}