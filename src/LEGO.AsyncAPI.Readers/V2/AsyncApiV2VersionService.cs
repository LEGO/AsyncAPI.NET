// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.V2
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    internal class AsyncApiV2VersionService : IAsyncApiVersionService
    {
        public AsyncApiDiagnostic Diagnostic { get; }

        /// <summary>
        /// Create Parsing Context.
        /// </summary>
        /// <param name="diagnostic">Provide instance for diagnostic object for collecting and accessing information about the parsing.</param>
        public AsyncApiV2VersionService(AsyncApiDiagnostic diagnostic)
        {
            this.Diagnostic = diagnostic;
        }

        private IDictionary<Type, Func<ParseNode, object>> loaders = new Dictionary<Type, Func<ParseNode, object>>
        {
            [typeof(AsyncApiAny)] = AsyncApiV2Deserializer.LoadAny,
            [typeof(AsyncApiComponents)] = AsyncApiV2Deserializer.LoadComponents,
            [typeof(AsyncApiExternalDocumentation)] = AsyncApiV2Deserializer.LoadExternalDocs,
            [typeof(AsyncApiInfo)] = AsyncApiV2Deserializer.LoadInfo,
            [typeof(AsyncApiLicense)] = AsyncApiV2Deserializer.LoadLicense,
            [typeof(AsyncApiOAuthFlow)] = AsyncApiV2Deserializer.LoadOAuthFlow,
            [typeof(AsyncApiOAuthFlows)] = AsyncApiV2Deserializer.LoadOAuthFlows,
            [typeof(AsyncApiOperation)] = AsyncApiV2Deserializer.LoadOperation,
            [typeof(AsyncApiParameter)] = AsyncApiV2Deserializer.LoadParameter,
            [typeof(AsyncApiJsonSchema)] = AsyncApiSchemaDeserializer.LoadSchema,
            [typeof(AvroSchema)] = AsyncApiAvroSchemaDeserializer.LoadSchema,
            [typeof(AsyncApiJsonSchemaPayload)] = AsyncApiV2Deserializer.LoadJsonSchemaPayload,
            [typeof(AsyncApiAvroSchemaPayload)] = AsyncApiV2Deserializer.LoadAvroPayload,
            [typeof(AsyncApiSecurityRequirement)] = AsyncApiV2Deserializer.LoadSecurityRequirement,
            [typeof(AsyncApiSecurityScheme)] = AsyncApiV2Deserializer.LoadSecurityScheme,
            [typeof(AsyncApiServer)] = AsyncApiV2Deserializer.LoadServer,
            [typeof(AsyncApiServerVariable)] = AsyncApiV2Deserializer.LoadServerVariable,
            [typeof(AsyncApiTag)] = AsyncApiV2Deserializer.LoadTag,
            [typeof(AsyncApiMessage)] = AsyncApiV2Deserializer.LoadMessage,
            [typeof(AsyncApiChannel)] = AsyncApiV2Deserializer.LoadChannel,
        };

        /// <summary>
        /// Parse the string to a <see cref="AsyncApiReference"/> object.
        /// </summary>
        /// <param name="reference">The URL of the reference.</param>
        /// <param name="type">The type of object referenced based on the context of the reference.</param>
        public AsyncApiReference ConvertToAsyncApiReference(
            string reference,
            ReferenceType? type)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new AsyncApiException($"The reference string '{reference}' has invalid format.");
            }

            var segments = reference.Split('#');
            if (segments.Length == 1)
            {
                if (type == ReferenceType.SecurityScheme)
                {
                    return new AsyncApiReference
                    {
                        Type = type,
                        Id = reference,
                    };
                }

                var asyncApiReference = new AsyncApiReference();
                asyncApiReference.Type = type;
                asyncApiReference.ExternalResource = segments[0];

                return asyncApiReference;
            }
            else if (segments.Length == 2)
            {
                // Local components reference
                if (reference.StartsWith("#"))
                {
                    try
                    {
                        return this.ParseReference(segments[1]);
                    }
                    catch (AsyncApiException ex)
                    {
                        this.Diagnostic.Errors.Add(new AsyncApiError(ex));
                        return null;
                    }
                }

                var asyncApiReference = new AsyncApiReference();
                var id = segments[1];
                if (id.StartsWith("/components/"))
                {
                    var localSegments = segments[1].Split('/');
                    var referencedType = localSegments[2].GetEnumFromDisplayName<ReferenceType>();
                    if (type == null)
                    {
                        type = referencedType;
                    }
                    else
                    {
                        if (type != referencedType)
                        {
                            throw new AsyncApiException("Referenced type mismatch");
                        }
                    }

                    id = localSegments[3];
                }
                else
                {
                    asyncApiReference.IsFragment = true;
                }

                asyncApiReference.ExternalResource = segments[0];
                asyncApiReference.Type = type;
                asyncApiReference.Id = id;

                return asyncApiReference;
            }

            throw new AsyncApiException($"The reference string '{reference}' has invalid format.");
        }

        public AsyncApiDocument LoadDocument(RootNode rootNode)
        {
            return AsyncApiV2Deserializer.LoadAsyncApi(rootNode);
        }

        public T LoadElement<T>(ParseNode node)
            where T : IAsyncApiElement
        {
            return (T)this.loaders[typeof(T)](node);
        }

        private AsyncApiReference ParseReference(string localReference)
        {
            if (string.IsNullOrWhiteSpace(localReference))
            {
                throw new ArgumentException(
                    $"The argument '{nameof(localReference)}' is null, empty or consists only of white-space.");
            }

            var segments = localReference.Split('/');

            if (segments.Length == 4)
            {
                if (segments[1] == "components")
                {
                    var referenceType = segments[2].GetEnumFromDisplayName<ReferenceType>();
                    return new AsyncApiReference { Type = referenceType, Id = segments[3] };
                }
            }

            throw new AsyncApiException($"The reference string '{localReference}' has invalid format.");
        }
    }
}