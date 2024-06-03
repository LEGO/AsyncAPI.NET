// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Nodes;
    using System.Threading;
    using System.Threading.Tasks;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Services;
    using LEGO.AsyncAPI.Validations;

    /// <summary>
    /// Service class for converting contents of TextReader into AsyncApiDocument instances.
    /// </summary>
    internal class AsyncApiJsonDocumentReader : IAsyncApiReader<JsonNode, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiJsonDocumentReader"/> class.
        /// </summary>
        /// <param name="settings">The settings used to read json.</param>
        public AsyncApiJsonDocumentReader(AsyncApiReaderSettings settings = null)
        {
            this.settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the stream input and parses it into an AsyncApi document.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public AsyncApiDocument Read(JsonNode input, out AsyncApiDiagnostic diagnostic)
        {
            diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic, this.settings)
            {
                ExtensionParsers = this.settings.ExtensionParsers,
                ServerBindingParsers = this.settings.Bindings.OfType<IBindingParser<IServerBinding>>().ToDictionary(b => b.BindingKey, b => b),
                ChannelBindingParsers = this.settings.Bindings.OfType<IBindingParser<IChannelBinding>>().ToDictionary(b => b.BindingKey, b => b),
                OperationBindingParsers = this.settings.Bindings.OfType<IBindingParser<IOperationBinding>>().ToDictionary(b => b.BindingKey, b => b),
                MessageBindingParsers = this.settings.Bindings.OfType<IBindingParser<IMessageBinding>>().ToDictionary(b => b.BindingKey, b => b),
            };

            AsyncApiDocument document = null;
            try
            {
                document = context.Parse(input);

                this.ResolveReferences(diagnostic, document);
            }
            catch (AsyncApiException ex)
            {
                diagnostic.Errors.Add(new AsyncApiError(ex));
            }

            if (this.settings.RuleSet != null && this.settings.RuleSet.Rules.Count > 0)
            {
                var asyncApiErrors = document.Validate(this.settings.RuleSet);
                foreach (var item in asyncApiErrors.OfType<AsyncApiValidatorError>())
                {
                    diagnostic.Errors.Add(item);
                }

                foreach (var item in asyncApiErrors.OfType<AsyncApiValidatorWarning>())
                {
                    diagnostic.Warnings.Add(item);
                }
            }

            return document;
        }

        public async Task<ReadResult> ReadAsync(JsonNode input, CancellationToken cancellationToken = default)
        {
            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic, this.settings)
            {
                ExtensionParsers = this.settings.ExtensionParsers,
            };

            AsyncApiDocument document = null;
            try
            {
                // Parse the AsyncApi Document
                document = context.Parse(input);
                this.ResolveReferences(diagnostic, document);
            }
            catch (AsyncApiException ex)
            {
                diagnostic.Errors.Add(new AsyncApiError(ex));
            }

            // Validate the document
            if (this.settings.RuleSet != null && this.settings.RuleSet.Rules.Count > 0)
            {
                var asyncApiErrors = document.Validate(this.settings.RuleSet);
                foreach (var item in asyncApiErrors.OfType<AsyncApiValidatorError>())
                {
                    diagnostic.Errors.Add(item);
                }

                foreach (var item in asyncApiErrors.OfType<AsyncApiValidatorWarning>())
                {
                    diagnostic.Warnings.Add(item);
                }
            }

            return new ReadResult
            {
                AsyncApiDocument = document,
                AsyncApiDiagnostic = diagnostic,
            };
        }

        /// <summary>
        /// Reads the stream input and parses the fragment of an AsyncApi description into an AsyncApi Element.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <param name="version">Version of the AsyncApi specification that the fragment conforms to.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing.</param>
        /// <returns>Instance of newly created AsyncApiDocument.</returns>
        public T ReadFragment<T>(JsonNode input, AsyncApiVersion version, out AsyncApiDiagnostic diagnostic)
            where T : IAsyncApiElement
        {
            diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic, this.settings)
            {
                ExtensionParsers = this.settings.ExtensionParsers,
                ServerBindingParsers = this.settings.Bindings.OfType<IBindingParser<IServerBinding>>().ToDictionary(b => b.BindingKey, b => b),
                ChannelBindingParsers = this.settings.Bindings.OfType<IBindingParser<IChannelBinding>>().ToDictionary(b => b.BindingKey, b => b),
                OperationBindingParsers = this.settings.Bindings.OfType<IBindingParser<IOperationBinding>>().ToDictionary(b => b.BindingKey, b => b),
                MessageBindingParsers = this.settings.Bindings.OfType<IBindingParser<IMessageBinding>>().ToDictionary(b => b.BindingKey, b => b),
            };

            IAsyncApiElement element = null;
            try
            {
                // Parse the AsyncApi element
                element = context.ParseFragment<T>(input, version);
            }
            catch (AsyncApiException ex)
            {
                diagnostic.Errors.Add(new AsyncApiError(ex));
            }

            // Validate the element
            if (this.settings.RuleSet != null && this.settings.RuleSet.Rules.Count > 0)
            {
                var errors = element.Validate(this.settings.RuleSet);
                foreach (var item in errors)
                {
                    diagnostic.Errors.Add(item);
                }
            }

            return (T)element;
        }

        private void ResolveReferences(AsyncApiDiagnostic diagnostic, AsyncApiDocument document)
        {
            switch (this.settings.ReferenceResolution)
            {
                case ReferenceResolutionSetting.ResolveAllReferences:
                    this.ResolveAllReferences(diagnostic, document);
                    break;
                case ReferenceResolutionSetting.ResolveInternalReferences:
                    this.ResolveInternalReferences(diagnostic, document);
                    break;
                case ReferenceResolutionSetting.DoNotResolveReferences:
                    break;
            }
        }

        private void ResolveAllReferences(AsyncApiDiagnostic diagnostic, AsyncApiDocument document)
        {
            this.ResolveInternalReferences(diagnostic, document);
            this.ResolveExternalReferences(diagnostic, document);
        }

        private void ResolveInternalReferences(AsyncApiDiagnostic diagnostic, AsyncApiDocument document)
        {
            var errors = new List<AsyncApiError>();

            var reader = new AsyncApiStringReader(this.settings);
            errors.AddRange(document.ResolveReferences());

            foreach (var item in errors)
            {
                diagnostic.Errors.Add(item);
            }
        }

        private void ResolveExternalReferences(AsyncApiDiagnostic diagnostic, AsyncApiDocument document)
        {
            var resolver = new AsyncApiExternalReferenceResolver(document, this.settings);
            var walker = new AsyncApiWalker(resolver);
            walker.Walk(document);

            foreach (var error in resolver.Errors)
            {
                diagnostic.Errors.Add(error);
            }
        }
    }
}