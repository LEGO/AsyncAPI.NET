// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Threading;
    using System.Threading.Tasks;
    using Json.Pointer;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.Services;
    using LEGO.AsyncAPI.Services;
    using LEGO.AsyncAPI.Validations;

    /// <summary>
    /// Service class for converting contents of TextReader into AsyncApiDocument instances.
    /// </summary>
    internal class AsyncApiJsonDocumentReader : IAsyncApiReader<JsonNode, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings settings;
        private ParsingContext context;

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
            this.context ??= new ParsingContext(diagnostic, this.settings)
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
                document = this.context.Parse(input);
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
            this.context ??= new ParsingContext(diagnostic, this.settings)
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
                // Parse the AsyncApi Document
                document = this.context.Parse(input);
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
            this.context ??= new ParsingContext(diagnostic, this.settings)
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
                element = this.context.ParseFragment<T>(input, version);
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
            this.ResolveExternalReferences(diagnostic, document, document);
        }

        private void ResolveInternalReferences(AsyncApiDiagnostic diagnostic, AsyncApiDocument document)
        {
            var reader = new AsyncApiStringReader(this.settings);

            var resolver = new AsyncApiReferenceHostDocumentResolver(document);
            var walker = new AsyncApiWalker(resolver);
            walker.Walk(document);

            foreach (var item in resolver.Errors)
            {
                diagnostic.Errors.Add(item);
            }
        }

        private void ResolveExternalReferences(AsyncApiDiagnostic diagnostic, IAsyncApiSerializable serializable, AsyncApiDocument hostDocument)
        {
            var loader = this.settings.ExternalReferenceLoader ??= new DefaultStreamLoader();
            var collector = new AsyncApiRemoteReferenceCollector(hostDocument);
            var walker = new AsyncApiWalker(collector);
            walker.Walk(serializable);

            foreach (var reference in collector.References)
            {
                if (this.context.Workspace.Contains(reference.Reference.Reference))
                {
                    continue;
                }

                try
                {
                    var input = loader.Load(new Uri(reference.Reference.Reference, UriKind.RelativeOrAbsolute));
                    var component = this.ReadStreamFragment(input, reference, diagnostic);
                    if (component == null)
                    {
                        diagnostic.Errors.Add(new AsyncApiError(string.Empty, $"Unable to deserialize reference '{reference.Reference.Reference}'"));
                        continue;
                    }

                    this.context.Workspace.RegisterComponent(reference.Reference.Reference, component);
                    this.ResolveExternalReferences(diagnostic, component, hostDocument);
                }
                catch (AsyncApiException ex)
                {
                    diagnostic.Errors.Add(new AsyncApiError(ex));
                }
            }
        }

        private IAsyncApiSerializable ReadStreamFragment(Stream input, IAsyncApiReferenceable reference, AsyncApiDiagnostic diagnostic)
        {
            var json = JsonNode.Parse(input);
            if (reference.Reference.IsFragment)
            {
                var pointer = JsonPointer.Parse(reference.Reference.Id);
                if (pointer.TryEvaluate(json, out var pointerResult))
                {
                    json = pointerResult;
                }
                else
                {
                    diagnostic.Errors.Add(new AsyncApiError(reference.Reference.Reference, "Could not resolve reference fragment."));
                    return null;
                }
            }

            IAsyncApiSerializable result = null;
            switch (reference.Reference.Type)
            {
                case ReferenceType.Schema:
                    result = this.ReadFragment<AsyncApiJsonSchema>(json, AsyncApiVersion.AsyncApi2_0, out var streamDiagnostics);
                    break;
                case ReferenceType.Server:
                    result = this.ReadFragment<AsyncApiServer>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.Channel:
                    result = this.ReadFragment<AsyncApiChannel>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.Message:
                    result = this.ReadFragment<AsyncApiMessage>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.SecurityScheme:
                    result = this.ReadFragment<AsyncApiSecurityScheme>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.Parameter:
                    result = this.ReadFragment<AsyncApiParameter>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.CorrelationId:
                    result = this.ReadFragment<AsyncApiCorrelationId>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.OperationTrait:
                    result = this.ReadFragment<AsyncApiOperationTrait>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.MessageTrait:
                    result = this.ReadFragment<AsyncApiMessage>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.ServerBindings:
                    result = this.ReadFragment<AsyncApiMessage>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.ChannelBindings:
                    result = this.ReadFragment<AsyncApiMessage>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.OperationBindings:
                    result = this.ReadFragment<AsyncApiMessage>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                case ReferenceType.MessageBindings:
                    result = this.ReadFragment<AsyncApiMessage>(json, AsyncApiVersion.AsyncApi2_0, out var _);
                    break;
                default:
                    diagnostic.Errors.Add(new AsyncApiError(reference.Reference.Reference, "Could not resolve reference."));
                    break;
            }

            return result;
        }
    }
}
