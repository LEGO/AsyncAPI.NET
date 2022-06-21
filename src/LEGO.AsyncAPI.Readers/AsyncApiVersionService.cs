
using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Exceptions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Exceptions;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncApi.Readers;
using LEGO.AsyncApi.Readers.Interface;
using LEGO.AsyncApi.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncApi.Readers
{
    internal class AsyncApiVersionService : IAsyncApiVersionService
    {
        public AsyncApiDiagnostic Diagnostic { get; }

        /// <summary>
        /// Create Parsing Context
        /// </summary>
        /// <param name="diagnostic">Provide instance for diagnotic object for collecting and accessing information about the parsing.</param>
        public AsyncApiVersionService(AsyncApiDiagnostic diagnostic)
        {
            Diagnostic = diagnostic;
        }

        private IDictionary<Type, Func<ParseNode, object>> _loaders = new Dictionary<Type, Func<ParseNode, object>>
        {
            [typeof(IAsyncApiAny)] = AsyncApiDeserializer.LoadAny,
            [typeof(AsyncApiCallback)] = AsyncApiDeserializer.LoadCallback,
            [typeof(AsyncApiComponents)] = AsyncApiDeserializer.LoadComponents,
            [typeof(AsyncApiEncoding)] = AsyncApiDeserializer.LoadEncoding,
            [typeof(AsyncApiExample)] = AsyncApiDeserializer.LoadExample,
            [typeof(AsyncApiExternalDocs)] = AsyncApiDeserializer.LoadExternalDocs,
            [typeof(AsyncApiHeader)] = AsyncApiDeserializer.LoadHeader,
            [typeof(AsyncApiInfo)] = AsyncApiDeserializer.LoadInfo,
            [typeof(AsyncApiLicense)] = AsyncApiDeserializer.LoadLicense,
            [typeof(AsyncApiLink)] = AsyncApiDeserializer.LoadLink,
            [typeof(AsyncApiMediaType)] = AsyncApiDeserializer.LoadMediaType,
            [typeof(AsyncApiOAuthFlow)] = AsyncApiDeserializer.LoadOAuthFlow,
            [typeof(AsyncApiOAuthFlows)] = AsyncApiDeserializer.LoadOAuthFlows,
            [typeof(AsyncApiOperation)] = AsyncApiDeserializer.LoadOperation,
            [typeof(AsyncApiParameter)] = AsyncApiDeserializer.LoadParameter,
            [typeof(AsyncApiPathItem)] = AsyncApiDeserializer.LoadPathItem,
            [typeof(AsyncApiPaths)] = AsyncApiDeserializer.LoadPaths,
            [typeof(AsyncApiRequestBody)] = AsyncApiDeserializer.LoadRequestBody,
            [typeof(AsyncApiResponse)] = AsyncApiDeserializer.LoadResponse,
            [typeof(AsyncApiResponses)] = AsyncApiDeserializer.LoadResponses,
            [typeof(AsyncApiSchema)] = AsyncApiDeserializer.LoadSchema,
            [typeof(AsyncApiSecurityRequirement)] = AsyncApiDeserializer.LoadSecurityRequirement,
            [typeof(AsyncApiSecurityScheme)] = AsyncApiDeserializer.LoadSecurityScheme,
            [typeof(AsyncApiServer)] = AsyncApiDeserializer.LoadServer,
            [typeof(AsyncApiServerVariable)] = AsyncApiDeserializer.LoadServerVariable,
            [typeof(AsyncApiTag)] = AsyncApiDeserializer.LoadTag,
            [typeof(AsyncApiXml)] = AsyncApiDeserializer.LoadXml
        };

        /// <summary>
        /// Parse the string to a <see cref="AsyncApiReference"/> object.
        /// </summary>
        /// <param name="reference">The URL of the reference</param>
        /// <param name="type">The type of object refefenced based on the context of the reference</param>
        public AsyncApiReference ConvertToAsyncApiReference(
            string reference,
            ReferenceType? type)
        {
            if (!string.IsNullOrWhiteSpace(reference))
            {
                var segments = reference.Split('#');
                if (segments.Length == 1)
                {
                    if (type == ReferenceType.Tag || type == ReferenceType.SecurityScheme)
                    {
                        return new AsyncApiReference
                        {
                            Type = type,
                            Id = reference
                        };
                    }

                    // Either this is an external reference as an entire file
                    // or a simple string-style reference for tag and security scheme.
                    return new AsyncApiReference
                    {
                        Type = type,
                        ExternalResource = segments[0]
                    };
                }
                else if (segments.Length == 2)
                {
                    if (reference.StartsWith("#"))
                    {
                        // "$ref": "#/components/schemas/Pet"
                        try
                        {
                            return ParseLocalReference(segments[1]);
                        }
                        catch (AsyncApiException ex)
                        {
                            Diagnostic.Errors.Add(new AsyncApiError(ex));
                            return null;
                        }
                    }
                    // Where fragments point into a non-AsyncApi document, the id will be the complete fragment identifier
                    string id = segments[1];
                    // $ref: externalSource.yaml#/Pet
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

                    return new AsyncApiReference
                    {
                        ExternalResource = segments[0],
                        Type = type,
                        Id = id
                    };
                }
            }

            throw new AsyncApiException(string.Format("The reference string '{0}' has invalid format.", reference));
        }

        public AsyncApiDocument LoadDocument(RootNode rootNode)
        {
            return AsyncApiDeserializer.LoadAsyncApi(rootNode);
        }

        public T LoadElement<T>(ParseNode node) where T : IAsyncApiElement
        {
            return (T)_loaders[typeof(T)](node);
        }

        private AsyncApiReference ParseLocalReference(string localReference)
        {
            if (string.IsNullOrWhiteSpace(localReference))
            {
                throw new ArgumentException(string.Format("The argument '{0}' is null, empty or consists only of white-space.", nameof(localReference)));
            }

            var segments = localReference.Split('/');

            if (segments.Length == 4) // /components/{type}/pet
            {
                if (segments[1] == "components")
                {
                    var referenceType = segments[2].GetEnumFromDisplayName<ReferenceType>();
                    return new AsyncApiReference { Type = referenceType, Id = segments[3] };
                }
            }

            throw new AsyncApiException(string.Format("The reference string '{0}' has invalid format.", localReference));
        }
    }
}
