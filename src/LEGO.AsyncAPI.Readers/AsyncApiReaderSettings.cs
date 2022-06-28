using System;
using System.Collections.Generic;
using System.IO;
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
        /// ResolveAllReferences effectively means load external references. Will be removed in v2. External references are never "resolved".
        /// </summary>
        ResolveReferences
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
        public ReferenceResolutionSetting ReferenceResolution { get; set; } =
            ReferenceResolutionSetting.ResolveReferences;

        /// <summary>
        /// Dictionary of parsers for converting extensions into strongly typed classes
        /// </summary>
        public Dictionary<string, Func<IAsyncApiAny, AsyncApiVersion, IAsyncApiExtension>>
            ExtensionParsers { get; set; } =
            new Dictionary<string, Func<IAsyncApiAny, AsyncApiVersion, IAsyncApiExtension>>();

        /// <summary>
        /// Rules to use for validating AsyncApi specification.  If none are provided a default set of rules are applied.
        /// </summary>
        public ValidationRuleSet RuleSet { get; set; } = ValidationRuleSet.GetDefaultRuleSet();
        
        /// <summary>
        /// Whether to leave the <see cref="Stream"/> object open after reading
        /// from an <see cref="AsyncApiStreamReader"/> object.
        /// </summary>
        public bool LeaveStreamOpen { get; set; }
    }
}