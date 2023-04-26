// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Validations;
    using YamlDotNet.RepresentationModel;

    /// <summary>
    /// Service class for converting contents of TextReader into AsyncApiDocument instances
    /// </summary>
    internal class AsyncApiYamlDocumentReader : IAsyncApiReader<YamlDocument, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings settings;

        /// <summary>
        /// Create stream reader with custom settings if desired.
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiYamlDocumentReader(AsyncApiReaderSettings settings = null)
        {
            this.settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the stream input and parses it into an AsyncApi document.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public AsyncApiDocument Read(YamlDocument input, out AsyncApiDiagnostic diagnostic)
        {
            diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic)
            {
                ExtensionParsers = this.settings.ExtensionParsers,
                ChannelBindingParsers = this.settings.BindingParsers.OfType<IBindingParser<IChannelBinding>>().ToDictionary(b => b.Type, b => b),
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
                foreach (var item in asyncApiErrors.Where(e => e is AsyncApiValidatorError))
                {
                    diagnostic.Errors.Add(item);
                }

                foreach (var item in asyncApiErrors.Where(e => e is AsyncApiValidatorWarning))
                {
                    diagnostic.Warnings.Add(item);
                }
            }

            return document;
        }

        public Task<ReadResult> ReadAsync(YamlDocument input)
        {
            var diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic)
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
                var errors = document.Validate(this.settings.RuleSet);
                foreach (var item in errors)
                {
                    diagnostic.Errors.Add(item);
                }
            }

            return Task.FromResult(new ReadResult
            {
                AsyncApiDocument = document,
                AsyncApiDiagnostic = diagnostic,
            });
        }

        private void ResolveReferences(AsyncApiDiagnostic diagnostic, AsyncApiDocument document)
        {
            var errors = new List<AsyncApiError>();

            // Resolve References if requested
            switch (this.settings.ReferenceResolution)
            {
                case ReferenceResolutionSetting.ResolveReferences:
                    errors.AddRange(document.ResolveReferences());
                    break;
                case ReferenceResolutionSetting.DoNotResolveReferences:
                    break;
            }

            foreach (var item in errors)
            {
                diagnostic.Errors.Add(item);
            }
        }


        /// <summary>
        /// Reads the stream input and parses the fragment of an AsyncApi description into an AsyncApi Element.
        /// </summary>
        /// <param name="input">TextReader containing AsyncApi description to parse.</param>
        /// <param name="version">Version of the AsyncApi specification that the fragment conforms to.</param>
        /// <param name="diagnostic">Returns diagnostic object containing errors detected during parsing</param>
        /// <returns>Instance of newly created AsyncApiDocument</returns>
        public T ReadFragment<T>(YamlDocument input, AsyncApiVersion version, out AsyncApiDiagnostic diagnostic)
            where T : IAsyncApiElement
        {
            diagnostic = new AsyncApiDiagnostic();
            var context = new ParsingContext(diagnostic)
            {
                ExtensionParsers = this.settings.ExtensionParsers,
                ChannelBindingParsers = this.settings.BindingParsers.OfType<IBindingParser<IChannelBinding>>().ToDictionary(b => b.Type, b => b),
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
    }
}
