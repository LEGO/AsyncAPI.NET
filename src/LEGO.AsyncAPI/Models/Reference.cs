// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using SharpYaml.Serialization;

    /// <summary>
    /// A simple object to allow referencing other components in the specification, internally and externally.
    /// </summary>
    public class Reference
    {
        /// <summary>
        /// REQUIRED. The reference string.
        /// </summary>
        [YamlMember("$ref")]
        public string Id { get; set; }
    }
}