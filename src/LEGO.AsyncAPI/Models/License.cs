namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    public class License : IExtensible
    {
        public License(string name)
        {
            Name = name;
        }

        /// <summary>
        /// REQUIRED. The license name used for the API.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAny> Extensions { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as License);
        }

        public bool Equals(License license)
        {
            return license != null &&
                   string.Compare(Name, license.Name, StringComparison.Ordinal) == 0 &&
                   ((Url == null && license.Url == null) || Url.Equals(license.Url));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Url);
        }
    }
}