// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.Json.Nodes;
    using System.Threading;
    using System.Threading.Tasks;
    using Json.Pointer;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.Services;
    using LEGO.AsyncAPI.Services;
    using LEGO.AsyncAPI.Validations;
    using YamlDotNet.RepresentationModel;

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
                await this.ResolveReferencesAsync(diagnostic, document);
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

        private async Task ResolveReferencesAsync(AsyncApiDiagnostic diagnostic, IAsyncApiSerializable serializable)
        {
            if (this.settings.ReferenceResolution == ReferenceResolutionSetting.DoNotResolveReferences)
            {
                return;
            }

            var collector = new AsyncApiReferenceCollector(this.context.Workspace);
            var walker = new AsyncApiWalker(collector);
            walker.Walk(serializable);

            foreach (var reference in collector.References)
            {
                if (this.context.Workspace.Contains(reference.Reference.Reference))
                {
                    continue;
                }

                IAsyncApiSerializable component = null;
                if (reference.Reference.IsExternal)
                {
                    if (this.settings.ReferenceResolution != ReferenceResolutionSetting.ResolveAllReferences)
                    {
                        continue;
                    }

                    component = await this.ResolveExternalReferenceAsync(diagnostic, reference);
                }
                else
                {
                    var stream = this.context.Workspace.ResolveReference<Stream>(string.Empty); // get whole document.
                    component = this.ResolveStreamReference(stream, reference, diagnostic);
                }

                if (component == null)
                {
                    diagnostic.Errors.Add(new AsyncApiError(string.Empty, $"Unable to deserialize reference '{reference.Reference.Reference}'"));
                    continue;
                }

                this.context.Workspace.RegisterComponent(reference.Reference.Reference, component);
                this.ResolveReferences(diagnostic, component);
            }
        }

        private void ResolveReferences(AsyncApiDiagnostic diagnostic, IAsyncApiSerializable serializable)
        {
            if (this.settings.ReferenceResolution == ReferenceResolutionSetting.DoNotResolveReferences)
            {
                return;
            }

            var collector = new AsyncApiReferenceCollector(this.context.Workspace);
            var walker = new AsyncApiWalker(collector);
            walker.Walk(serializable);

            foreach (var reference in collector.References)
            {
                if (this.context.Workspace.Contains(reference.Reference.Reference))
                {
                    continue;
                }

                IAsyncApiSerializable component = null;
                if (reference.Reference.IsExternal)
                {
                    if (this.settings.ReferenceResolution != ReferenceResolutionSetting.ResolveAllReferences)
                    {
                        continue;
                    }

                    component = this.ResolveExternalReference(diagnostic, reference);
                }
                else
                {
                    var stream = this.context.Workspace.ResolveReference<Stream>(string.Empty); // get whole document.
                    component = this.ResolveStreamReference(stream, reference, diagnostic);
                }

                if (component == null)
                {
                    diagnostic.Errors.Add(new AsyncApiError(string.Empty, $"Unable to deserialize reference '{reference.Reference.Reference}'"));
                    continue;
                }

                this.context.Workspace.RegisterComponent(reference.Reference.Reference, component);
                this.ResolveReferences(diagnostic, component);
            }
        }

        private IAsyncApiSerializable ResolveExternalReference(AsyncApiDiagnostic diagnostic, IAsyncApiReferenceable reference)
        {
            if (reference is null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            var loader = this.settings.ExternalReferenceLoader ??= new DefaultStreamLoader(this.settings);
            try
            {
                Stream stream;
                if (this.context.Workspace.Contains(reference.Reference.ExternalResource))
                {
                    stream = this.context.Workspace.ResolveReference<Stream>(reference.Reference.ExternalResource);
                }
                else
                {
                    stream = loader.Load(new Uri(reference.Reference.ExternalResource, UriKind.RelativeOrAbsolute));
                    this.context.Workspace.RegisterComponent(reference.Reference.ExternalResource, stream);
                }

                return this.ResolveStreamReference(stream, reference, diagnostic);
            }
            catch (AsyncApiException ex)
            {
                diagnostic.Errors.Add(new AsyncApiError(ex));
                return null;
            }
        }

        private async Task<IAsyncApiSerializable> ResolveExternalReferenceAsync(AsyncApiDiagnostic diagnostic, IAsyncApiReferenceable reference)
        {
            if (reference is null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            var loader = this.settings.ExternalReferenceLoader ??= new DefaultStreamLoader(this.settings);
            try
            {
                Stream stream;
                if (this.context.Workspace.Contains(reference.Reference.ExternalResource))
                {
                    stream = this.context.Workspace.ResolveReference<Stream>(reference.Reference.ExternalResource);
                }
                else
                {
                    stream = await loader.LoadAsync(new Uri(reference.Reference.ExternalResource, UriKind.RelativeOrAbsolute));
                    this.context.Workspace.RegisterComponent(reference.Reference.ExternalResource, stream);
                }

                return this.ResolveStreamReference(stream, reference, diagnostic);
            }
            catch (AsyncApiException ex)
            {
                diagnostic.Errors.Add(new AsyncApiError(ex));
                return null;
            }
        }

        private JsonNode ReadToJson(Stream stream)
        {
            if (stream != null)
            {
                var reader = new StreamReader(stream);
                var yamlStream = new YamlStream();
                yamlStream.Load(reader);
                return yamlStream.Documents.First().ToJsonNode(this.settings.CultureInfo);
            }

            return default;
        }

        private IAsyncApiSerializable ResolveStreamReference(Stream stream, IAsyncApiReferenceable reference, AsyncApiDiagnostic diagnostic)
        {
            JsonNode json = null;
            try
            {
                json = this.ReadToJson(stream);
            }
            catch
            {
                diagnostic.Errors.Add(new AsyncApiError(string.Empty, $"Unable to deserialize reference: '{reference.Reference.Reference}'"));
                return null;
            }

            if (reference.Reference.IsFragment)
            {
                var pointer = JsonPointer.Parse(reference.Reference.FragmentId);
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

            AsyncApiDiagnostic fragmentDiagnostic = new AsyncApiDiagnostic();
            IAsyncApiSerializable result = null;
            switch (reference.Reference.Type)
            {
                case ReferenceType.Schema:
                    if (reference is AsyncApiJsonSchemaReference)
                    {
                        result = this.ReadFragment<AsyncApiJsonSchema>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    }

                    if (reference is AsyncApiAvroSchemaReference)
                    {
                        result = this.ReadFragment<AsyncApiAvroSchema>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    }

                    break;

                case ReferenceType.Server:
                    result = this.ReadFragment<AsyncApiServer>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.Channel:
                    result = this.ReadFragment<AsyncApiChannel>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.Message:
                    result = this.ReadFragment<AsyncApiMessage>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.SecurityScheme:
                    result = this.ReadFragment<AsyncApiSecurityScheme>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.Parameter:
                    result = this.ReadFragment<AsyncApiParameter>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.CorrelationId:
                    result = this.ReadFragment<AsyncApiCorrelationId>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.OperationTrait:
                    result = this.ReadFragment<AsyncApiOperationTrait>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.MessageTrait:
                    result = this.ReadFragment<AsyncApiMessageTrait>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.ServerBindings:
                    result = this.ReadFragment<AsyncApiBindings<IServerBinding>>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.ChannelBindings:
                    result = this.ReadFragment<AsyncApiBindings<IChannelBinding>>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.OperationBindings:
                    result = this.ReadFragment<AsyncApiBindings<IOperationBinding>>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                case ReferenceType.MessageBindings:
                    result = this.ReadFragment<AsyncApiBindings<IMessageBinding>>(json, AsyncApiVersion.AsyncApi2_0, out fragmentDiagnostic);
                    break;
                default:
                    diagnostic.Errors.Add(new AsyncApiError(reference.Reference.Reference, "Could not resolve reference."));
                    break;
            }

            diagnostic.Append(fragmentDiagnostic);
            return result;
        }
    }
}
