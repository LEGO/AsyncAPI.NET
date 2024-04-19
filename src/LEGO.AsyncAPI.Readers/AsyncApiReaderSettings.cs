// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Validations;

    public enum ReferenceResolutionSetting
    {
        /// <summary>
        /// Create placeholder objects with an AsyncApiReference instance and UnresolvedReference set to true.
        /// </summary>
        DoNotResolveReferences,

        /// <summary>
        /// Resolve internal component references and inline them.
        /// </summary>
        ResolveReferences,
    }

    /// <summary>
    /// Configuration settings to control how AsyncApi documents are parsed.
    /// </summary>
    public class AsyncApiReaderSettings : AsyncApiSettings
    {
        /// <summary>
        /// Indicates how references in the source document should be handled.
        /// </summary>
        public ReferenceResolutionSetting ReferenceResolution { get; set; } =
            ReferenceResolutionSetting.ResolveReferences;

        /// <summary>
        /// Dictionary of parsers for converting extensions into strongly typed classes.
        /// </summary>
        public Dictionary<string, Func<AsyncApiAny, IAsyncApiExtension>>
            ExtensionParsers
        { get; set; } =
            new Dictionary<string, Func<AsyncApiAny, IAsyncApiExtension>>();

        public IEnumerable<IBindingParser<IBinding>>
           Bindings
        { get; set; } =
           new List<IBindingParser<IBinding>>();

        /// <summary>
        /// Rules to use for validating AsyncApi specification.  If none are provided a default set of rules are applied.
        /// </summary>
        public ValidationRuleSet RuleSet { get; set; } = ValidationRuleSet.GetDefaultRuleSet();

        /// <summary>
        /// Whether to leave the <see cref="Stream"/> object open after reading
        /// from an <see cref="AsyncApiStreamReader"/> object.
        /// </summary>
        public bool LeaveStreamOpen { get; set; }

        /// <summary>
        /// External reference reader implementation provided by users for reading external resources.
        /// </summary>
        public IAsyncApiExternalReferenceReader ExternalReferenceReader { get; set; }
    }
}