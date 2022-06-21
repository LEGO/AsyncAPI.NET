using System;
using System.Collections.Generic;
using System.IO;
using LEGO.AsyncAPI;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.Interface;
using LEGO.AsyncAPI.Validations;

namespace LEGO.AsyncAPI.Readers
{
    public enum ReferenceResolutionSetting
    {
        /// <summary>
        /// Create placeholder objects with an AsyncApiReference instance and UnresolvedReference set to true.
        /// </summary>
        DoNotResolveReferences,
        /// <summary>
        /// Convert local references to references of valid domain objects.
        /// </summary>
        ResolveLocalReferences,
        /// <summary>
        /// ResolveAllReferences effectively means load external references. Will be removed in v2. External references are never "resolved".
        /// </summary>
        ResolveAllReferences
    }

    /// <summary>
    /// Configuration settings to control how AsyncApi documents are parsed
    /// </summary>
    public class AsyncApiReaderSettings
    {
        /// <summary>
        /// Indicates how references in the source document should be handled.
        /// </summary>
        /// <remarks>This setting will be going away in the next major version of this library.  Use GetEffective on model objects to get resolved references.</remarks>
        public ReferenceResolutionSetting ReferenceResolution { get; set; } = ReferenceResolutionSetting.ResolveLocalReferences;

        /// <summary>
        /// When external references are found, load them into a shared workspace
        /// </summary>
        public bool LoadExternalRefs { get; set; } = false;

        /// <summary>
        /// Dictionary of parsers for converting extensions into strongly typed classes
        /// </summary>
        public Dictionary<string, Func<IAsyncApiAny, AsyncApiSpecVersion, IAsyncApiExtension>> ExtensionParsers { get; set; } = new Dictionary<string, Func<IAsyncApiAny, AsyncApiSpecVersion, IAsyncApiExtension>>();

        /// <summary>
        /// Rules to use for validating AsyncApi specification.  If none are provided a default set of rules are applied.
        /// </summary>
        public ValidationRuleSet RuleSet { get; set; } = ValidationRuleSet.GetDefaultRuleSet();

        /// <summary>
        /// URL where relative references should be resolved from if the description does not contain Server definitions
        /// </summary>
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Function used to provide an alternative loader for accessing external references.
        /// </summary>
        /// <remarks>
        /// Default loader will attempt to dereference http(s) urls and file urls.
        /// </remarks>
        public IStreamLoader CustomExternalLoader { get; set; }

        /// <summary>
        /// Whether to leave the <see cref="Stream"/> object open after reading
        /// from an <see cref="AsyncApiStreamReader"/> object.
        /// </summary>
        public bool LeaveStreamOpen { get; set; }
    }
}
