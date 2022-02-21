namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// A simple object to allow referencing other components in the specification, internally and externally.
    /// </summary>
    public class Reference
    {
        /// <summary>
        /// REQUIRED. The reference string.
        /// </summary>
        public string Id { get; set; }
    }
}