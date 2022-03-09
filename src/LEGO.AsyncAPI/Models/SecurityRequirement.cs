// Copyright (c) The LEGO Group. All rights reserved.

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
        /// Additional properties of security requirement, defined as object with array of strings.
        /// Example:
        /// {
        ///   "petstore_auth": [
        ///     "write:pets",
        ///     "read:pets"
        ///   ]
        /// }.
        /// </summary>
        public IDictionary<string, IAny> Extensions { get; set; } = new Dictionary<string, IAny>();
    }
}
