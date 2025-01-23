// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
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
    using LEGO.AsyncAPI.Readers.Services;
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
            var loader = this.settings.ExternalReferenceLoader ?? new DefaultStreamLoader(this.settings.BaseUrl);
            var collector = new AsyncApiRemoteReferenceCollector();
            var walker = new AsyncApiWalker(collector);
            walker.Walk(document);

            var reader = new AsyncApiStreamReader(this.settings);
            foreach (var reference in collector.References)
            {
                var input = loader.Load(new Uri(reference.ExternalResource, UriKind.RelativeOrAbsolute));

                // If Id is not null, the reference is for a fragment of a full document.
                if (reference.Id != null)
                {
                    var result = reader.Read(input, out var streamDiagnostics); // How about avro?!
                    if (streamDiagnostics.Warnings.Any() || streamDiagnostics.Errors.Any())
                    {
                        diagnostic.Append(streamDiagnostics, reference.ExternalResource);
                    }

                    if (result != null)
                    {
                        this.ResolveAllReferences(diagnostic, result);
                    }
                }
                else
                {
                    // If id IS null, its a fragment that we can resolve directly.
                    // #TODO Use proxy references for easier fragment resolution.
                    IAsyncApiElement result = null;
                    switch (reference.Type)
                    {
                        case ReferenceType.Schema:
                            result = reader.ReadFragment<AsyncApiJsonSchema>(input, AsyncApiVersion.AsyncApi2_0, out var streamDiagnostics);
                            break;
                        //case ReferenceType.Server:
                        //    result = reader.ReadFragment<AsyncApiServer>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.Channel:
                            result = reader.ReadFragment<AsyncApiChannel>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.Message:
                            result = reader.ReadFragment<AsyncApiMessage>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.SecurityScheme:
                            result = reader.ReadFragment<AsyncApiSecurityScheme>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.Parameter:
                            result = reader.ReadFragment<AsyncApiParameter>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.CorrelationId:
                            result = reader.ReadFragment<AsyncApiCorrelationId>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.OperationTrait:
                            result = reader.ReadFragment<AsyncApiOperationTrait>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.MessageTrait:
                            result = reader.ReadFragment<AsyncApiMessage>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.ServerBindings:
                            result = reader.ReadFragment<AsyncApiMessage>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.ChannelBindings:
                            result = reader.ReadFragment<AsyncApiMessage>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.OperationBindings:
                            result = reader.ReadFragment<AsyncApiMessage>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        case ReferenceType.MessageBindings:
                            result = reader.ReadFragment<AsyncApiMessage>(input, AsyncApiVersion.AsyncApi2_0, out var _);
                            break;
                        default:
                            diagnostic.Errors.Add(new AsyncApiError(reference.Reference, "Could not resolve reference."));
                            break;
                    }

                    if (result != null)
                    {
                        this.ResolveAllReferences(diagnostic, document);
                    }
            }

            }
        }
    }
}