namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Bindings;
    using LEGO.AsyncAPI.Models.Bindings.Http;
    using LEGO.AsyncAPI.Models.Bindings.Kafka;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Writers;
    using NUnit.Framework;

    public class AsyncApiDocumentTests
    {


        [Test]
        public void SerializeV2_WithFullSpec_Serializes()
        {
            var expected =
                @"asyncapi: '2.3.0'
info:
  title: apiTitle
  description: description
  termsOfService: https://example.com/termsOfService
  contact:
    name: contactName
    url: https://example.com/contact
    email: contactEmail
  license:
    name: licenseName
    url: https://example.com/license
    x-extension: value
  version: apiVersion
  x-extension: value
id: documentId
servers:
  myServer:
    url: https://example.com/server
    protocol: KafkaProtocol
    protocolVersion: protocolVersion
    description: serverDescription
    security:
      - '#/components/securitySchemes/securitySchemeName':
          - requirementItem
channels:
  channel1:
    description: channelDescription
    subscribe:
      operationId: myOperation
      summary: operationSummary
      description: operationDescription
      tags:
        - name: tagName
          description: tagDescription
      externalDocs:
        description: externalDocsDescription
        url: https://example.com/externalDocs
      traits:
        - operationId: myOperation
          summary: traitSummary
          description: traitDescription
          tags:
            - name: tagName
              description: tagDescription
          externalDocs:
            description: externalDocsDescription
            url: https://example.com/externalDocs
          x-extension: value
      message:
        oneOf:
          - contentType: contentType
            name: messageName
            title: messageTitle
            summary: messageSummary
            description: messageDescription
          - correlationId:
              description: correlationDescription
              location: correlationLocation
            schemaFormat: schemaFormat
            contentType: contentType
            name: messageName
            title: messageTitle
            summary: messageSummary
            description: messageDescription
            traits:
              - headers:
                  title: schemaTitle
                  description: schemaDescription
                  writeOnly: true
                  examples:
                    - key: value
                      otherKey: 9223372036854775807
                name: traitName
                title: traitTitle
                summary: traitSummary
                description: traitDescription
                tags:
                  - name: tagName
                    description: tagDescription
                externalDocs:
                  description: externalDocsDescription
                  url: https://example.com/externalDocs
                examples:
                  - name: exampleName
                    summary: exampleSummary
                    payload:
                      key: value
                      otherKey: 9223372036854775807
                    x-extension: value
                x-extension: value
            x-extension: value
      x-extension: value
components:
  securitySchemes:
    securitySchemeName:
      type: oauth2
      description: securitySchemeDescription
      flows:
        implicit:
          authorizationUrl: https://example.com/authorization
          tokenUrl: https://example.com/tokenUrl
          refreshUrl: https://example.com/refresh
          scopes:
            securitySchemeScopeKey: securitySchemeScopeValue
          x-extension: value";

            // Arrange
            var title = "apiTitle";
            string contactName = "contactName";
            string contactEmail = "contactEmail";
            string contactUri = "https://example.com/contact";
            string description = "description";
            string licenseName = "licenseName";
            string licenseUri = "https://example.com/license";
            string extensionKey = "x-extension";
            string extensionString = "value";
            string apiVersion = "apiVersion";
            string termsOfServiceUri = "https://example.com/termsOfService";
            string channelKey = "channel1";
            string channelDescription = "channelDescription";
            string operationDescription = "operationDescription";
            string operationId = "myOperation";
            string operationSummary = "operationSummary";
            string externalDocsUri = "https://example.com/externalDocs";
            string externalDocsDescription = "externalDocsDescription";
            string messageDescription = "messageDescription";
            string messageTitle = "messageTitle";
            string messageSummary = "messageSummary";
            string messageName = "messageName";
            string contentType = "contentType";
            string schemaFormat = "schemaFormat";
            string correlationLocation = "correlationLocation";
            string correlationDescription = "correlationDescription";
            string traitName = "traitName";
            string traitTitle = "traitTitle";
            string schemaTitle = "schemaTitle";
            string schemaDescription = "schemaDescription";
            string anyKey = "key";
            string anyOtherKey = "otherKey";
            string anyStringValue = "value";
            long anyLongValue = Int64.MaxValue;
            string exampleSummary = "exampleSummary";
            string exampleName = "exampleName";
            string traitDescription = "traitDescription";
            string traitSummary = "traitSummary";
            string tagName = "tagName";
            string tagDescription = "tagDescription";
            string documentId = "documentId";
            string serverKey = "myServer";
            string serverDescription = "serverDescription";
            string protocolVersion = "protocolVersion";
            string serverUrl = "https://example.com/server";
            string protocol = "KafkaProtocol";
            string securirySchemeDescription = "securitySchemeDescription";
            string securitySchemeName = "securitySchemeName";
            string bearerFormat = "bearerFormat";
            string scheme = "scheme";
            string scopeKey = "securitySchemeScopeKey";
            string scopeValue = "securitySchemeScopeValue";
            string tokenUrl = "https://example.com/tokenUrl";
            string refreshUrl = "https://example.com/refresh";
            string authorizationUrl = "https://example.com/authorization";
            string requirementString = "requirementItem";
            var document = new AsyncApiDocument()
            {
                Id = documentId,
                Components = new AsyncApiComponents
                {
                    SecuritySchemes = new Dictionary<string, AsyncApiSecurityScheme>
                    {
                        {
                            securitySchemeName, new AsyncApiSecurityScheme
                            {
                                Description = securirySchemeDescription,
                                Name = securitySchemeName,
                                BearerFormat = bearerFormat,
                                Scheme = scheme,
                                Type = SecuritySchemeType.OAuth2,
                                Flows = new AsyncApiOAuthFlows
                                {
                                    Implicit = new AsyncApiOAuthFlow
                                    {
                                        Scopes = new Dictionary<string, string>
                                        {
                                            { scopeKey, scopeValue },
                                        },
                                        TokenUrl = new Uri(tokenUrl),
                                        RefreshUrl = new Uri(refreshUrl),
                                        AuthorizationUrl = new Uri(authorizationUrl),
                                        Extensions = new Dictionary<string, IAsyncApiExtension>
                                        {
                                            { extensionKey, new AsyncApiString(extensionString) },
                                        },
                                    },
                                },
                            }
                        },
                    },
                },
                Servers = new Dictionary<string, AsyncApiServer>
                {
                    {
                        serverKey, new AsyncApiServer
                        {
                            Description = serverDescription,
                            ProtocolVersion = protocolVersion,
                            Url = serverUrl,
                            Protocol = protocol,
                            Security = new List<AsyncApiSecurityRequirement>
                            {
                                new AsyncApiSecurityRequirement
                                {
                                    {
                                        new AsyncApiSecurityScheme()
                                        {
                                            Name = securitySchemeName,

                                            Reference = new AsyncApiReference()
                                            {
                                                Id = securitySchemeName,
                                                Type = ReferenceType.SecurityScheme,
                                            },
                                        }, new List<string>
                                        {
                                            requirementString,
                                        }
                                    },
                                },
                            },
                        }
                    },
                },
                Info = new AsyncApiInfo()
                {
                    Title = title,
                    Contact = new AsyncApiContact()
                    {
                        Name = contactName,
                        Email = contactEmail,
                        Url = new Uri(contactUri),
                    },
                    Description = description,
                    License = new AsyncApiLicense()
                    {
                        Name = licenseName,
                        Url = new Uri(licenseUri),
                        Extensions = new Dictionary<string, IAsyncApiExtension>
                        {
                            { extensionKey, new AsyncApiString(extensionString) },
                        },
                    },
                    Version = apiVersion,
                    TermsOfService = new Uri(termsOfServiceUri),
                    Extensions = new Dictionary<string, IAsyncApiExtension>
                    {
                        { extensionKey, new AsyncApiString(extensionString) },
                    },
                },
                Channels = new Dictionary<string, AsyncApiChannel>
                {
                    {
                        channelKey, new AsyncApiChannel
                        {
                            Description = channelDescription,
                            Subscribe = new AsyncApiOperation
                            {
                                Description = operationDescription,
                                OperationId = operationId,
                                Summary = operationSummary,
                                ExternalDocs = new AsyncApiExternalDocumentation
                                {
                                    Url = new Uri(externalDocsUri),
                                    Description = externalDocsDescription,
                                },
                                Message = new List<AsyncApiMessage>
                                {
                                    {
                                    new AsyncApiMessage
                                        {
                                            Description = messageDescription,
                                            Title = messageTitle,
                                            Summary = messageSummary,
                                            Name = messageName,
                                            ContentType = contentType,
                                        }
                                    },
                                    {
                                        new AsyncApiMessage
                                        {
                                            Description = messageDescription,
                                            Title = messageTitle,
                                            Summary = messageSummary,
                                            Name = messageName,
                                            ContentType = contentType,
                                            SchemaFormat = schemaFormat,
                                            CorrelationId = new AsyncApiCorrelationId
                                            {
                                                Location = correlationLocation,
                                                Description = correlationDescription,
                                                Extensions = new Dictionary<string, IAsyncApiExtension>
                                                {
                                                    { extensionKey, new AsyncApiString(extensionString) },
                                                },
                                            },
                                            Traits = new List<AsyncApiMessageTrait>
                                            {
                                                new AsyncApiMessageTrait
                                                {
                                                    Name = traitName,
                                                    Title = traitTitle,
                                                    Headers = new AsyncApiSchema
                                                    {
                                                        Title = schemaTitle,
                                                        WriteOnly = true,
                                                        Description = schemaDescription,
                                                        Examples = new List<IAsyncApiAny>
                                                        {
                                                            new AsyncApiObject
                                                            {
                                                                { anyKey, new AsyncApiString(anyStringValue) },
                                                                { anyOtherKey, new AsyncApiLong(anyLongValue) },
                                                            },
                                                        },
                                                    },
                                                    Examples = new List<AsyncApiMessageExample>
                                                    {
                                                        new AsyncApiMessageExample
                                                        {
                                                            Summary = exampleSummary,
                                                            Name = exampleName,
                                                            Payload = new AsyncApiObject
                                                            {
                                                                { anyKey, new AsyncApiString(anyStringValue) },
                                                                { anyOtherKey, new AsyncApiLong(anyLongValue) },
                                                            },
                                                            Extensions = new Dictionary<string, IAsyncApiExtension>
                                                            {
                                                                { extensionKey, new AsyncApiString(extensionString) },
                                                            },
                                                        },
                                                    },
                                                    Description = traitDescription,
                                                    Summary = traitSummary,
                                                    Tags = new List<AsyncApiTag>
                                                    {
                                                        new AsyncApiTag
                                                        {
                                                            Name = tagName,
                                                            Description = tagDescription,
                                                        },
                                                    },
                                                    ExternalDocs = new AsyncApiExternalDocumentation
                                                    {
                                                        Url = new Uri(externalDocsUri),
                                                        Description = externalDocsDescription,
                                                    },
                                                    Extensions = new Dictionary<string, IAsyncApiExtension>
                                                    {
                                                        { extensionKey, new AsyncApiString(extensionString) },
                                                    },
                                                },
                                            },
                                            Extensions = new Dictionary<string, IAsyncApiExtension>
                                            {
                                                { extensionKey, new AsyncApiString(extensionString) },
                                            },
                                        }
                                    },
                                },
                                Extensions = new Dictionary<string, IAsyncApiExtension>
                                {
                                    { extensionKey, new AsyncApiString(extensionString) },
                                },
                                Tags = new List<AsyncApiTag>
                                {
                                    new AsyncApiTag
                                    {
                                        Name = tagName,
                                        Description = tagDescription,
                                    },
                                },
                                Traits = new List<AsyncApiOperationTrait>
                                {
                                    new AsyncApiOperationTrait
                                    {
                                        Description = traitDescription,
                                        Summary = traitSummary,
                                        Tags = new List<AsyncApiTag>
                                        {
                                            new AsyncApiTag
                                            {
                                                Name = tagName,
                                                Description = tagDescription,
                                            },
                                        },
                                        ExternalDocs = new AsyncApiExternalDocumentation
                                        {
                                            Url = new Uri(externalDocsUri),
                                            Description = externalDocsDescription,
                                        },
                                        OperationId = operationId,
                                        Extensions = new Dictionary<string, IAsyncApiExtension>
                                        {
                                            { extensionKey, new AsyncApiString(extensionString) },
                                        },
                                    },
                                },
                            },
                        }
                    },
                },
            };

            var outputString = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiYamlWriter(outputString);

            // Act
            document.SerializeV2(writer);
            var actual = outputString.GetStringBuilder().ToString();

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void Serialize_WithBindings_Serializes()
        {
            var expected = @"asyncapi: '2.3.0'
info:
  description: test description
servers:
  production:
    url: example.com
    protocol: pulsar+ssl
    description: test description
channels:
  testChannel:
    publish:
      message:
        bindings:
          http:
            headers:
              description: this mah binding
          kafka:
            key:
              description: this mah other binding
    bindings:
      kafka:
        partitions: 2
        replicas: 1";

            var doc = new AsyncApiDocument();
            doc.Info = new AsyncApiInfo()
            {
                Description = "test description"
            };
            doc.Servers.Add("production", new AsyncApiServer
            {
                Description = "test description",
                Protocol = "pulsar+ssl",
                Url = "example.com",
            });
            doc.Channels.Add("testChannel",
                new AsyncApiChannel
                {
                    Bindings = new AsyncApiBindings<IChannelBinding>
                    {
                        {
                            new KafkaChannelBinding
                            {
                                Partitions = 2,
                                Replicas = 1,
                            }
                        },
                    },
                    Publish = new AsyncApiOperation
                    {
                        Message = new List<AsyncApiMessage>
                        {
                            {
                                new AsyncApiMessage
                                {
                                    Bindings = new AsyncApiBindings<IMessageBinding>
                                    {
                                        {
                                            new HttpMessageBinding
                                            {
                                                Headers = new AsyncApiSchema
                                                {
                                                    Description = "this mah binding",
                                                },
                                            }
                                        },
                                        {
                                            new KafkaMessageBinding
                                            {
                                                Key = new AsyncApiSchema
                                                {
                                                    Description = "this mah other binding",
                                                },
                                            }
                                        },

                                    },
                                }
                            },
                        },
                    },
                });
            var actual = doc.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            var reader = new AsyncApiStringReader();
            var deserialized = reader.Read(actual, out var diagnostic);

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(actual, expected);
            Assert.AreEqual(2, deserialized.Channels.First().Value.Publish.Message.First().Bindings.Count);

            var binding = deserialized.Channels.First().Value.Publish.Message.First().Bindings.First();
            Assert.AreEqual(BindingType.Http, binding.Key);
            var httpBinding = binding.Value as HttpMessageBinding;

            Assert.AreEqual("this mah binding", httpBinding.Headers.Description);
        }
    }
}
