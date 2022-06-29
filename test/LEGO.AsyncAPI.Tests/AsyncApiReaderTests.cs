namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Models;
    using Models.Any;
    using Models.Interfaces;
    using NUnit.Framework;
    using Readers;
    using Writers;

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
        correlationId:
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
      type: OAuth2
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
                        {securitySchemeName, new AsyncApiSecurityScheme
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
                                            {scopeKey, scopeValue}
                                        },
                                        TokenUrl = new Uri(tokenUrl),
                                        RefreshUrl = new Uri(refreshUrl),
                                        AuthorizationUrl = new Uri(authorizationUrl),
                                        Extensions = new Dictionary<string, IAsyncApiExtension>
                                        {
                                            {extensionKey, new AsyncApiString(extensionString)}
                                        }
                                    }
                                }}
                    }
                }
                    },
                Servers = new Dictionary<string, AsyncApiServer>
                {
                    {serverKey, new AsyncApiServer
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
                                            Type = ReferenceType.SecurityScheme
                                        },
                                }, new List<string>
                                {
                                    requirementString,
                                }

                                }
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
                        Url = new Uri(contactUri)
                    },
                    Description = description,
                    License = new AsyncApiLicense()
                    {
                        Name = licenseName,
                        Url = new Uri(licenseUri),
                        Extensions = new Dictionary<string, IAsyncApiExtension>
                        {
                            {extensionKey, new AsyncApiString(extensionString)}
                        },
                    },
                    Version = apiVersion,
                    TermsOfService = new Uri(termsOfServiceUri),
                    Extensions = new Dictionary<string, IAsyncApiExtension>
                    {
                        {extensionKey, new AsyncApiString(extensionString)}
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
                            Message = new AsyncApiMessage
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
                                        {extensionKey, new AsyncApiString(extensionString)},
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
                                                        {anyKey, new AsyncApiString(anyStringValue)},
                                                        {anyOtherKey, new AsyncApiLong(anyLongValue)}
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
                                                    {anyKey, new AsyncApiString(anyStringValue)},
                                                    {anyOtherKey, new AsyncApiLong(anyLongValue)}
                                                },
                                                Extensions = new Dictionary<string, IAsyncApiExtension>
                                                {
                                                    {extensionKey, new AsyncApiString(extensionString)}
                                                }
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
                                            }
                                        },
                                        ExternalDocs = new AsyncApiExternalDocumentation
                                        {
                                            Url = new Uri(externalDocsUri),
                                            Description = externalDocsDescription
                                        },
                                        Extensions = new Dictionary<string, IAsyncApiExtension>
                                        {
                                            {extensionKey, new AsyncApiString(extensionString)},
                                        },
                                    },
                                },
                                Extensions = new Dictionary<string, IAsyncApiExtension>
                                {
                                    {extensionKey, new AsyncApiString(extensionString)},
                                },
                            },
                            Extensions = new Dictionary<string, IAsyncApiExtension>
                            {
                                {extensionKey, new AsyncApiString(extensionString)},
                            },
                            Tags = new List<AsyncApiTag>
                            {
                                new AsyncApiTag
                                {
                                    Name = tagName,
                                    Description = tagDescription,
                                }
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
                                        }
                                    },
                                    ExternalDocs = new AsyncApiExternalDocumentation
                                    {
                                        Url = new Uri(externalDocsUri),
                                        Description = externalDocsDescription
                                    },
                                    OperationId = operationId,
                                    Extensions = new Dictionary<string, IAsyncApiExtension>
                                    {
                                        {extensionKey, new AsyncApiString(extensionString)},
                                    },
                                    }
                                }
                            }
                        }},
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

    public class AsyncApiReaderTests
    {
        [Test]
        public void Read_WithFullSpec_Deserializes()
        {
            var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
  version: 1.0.0
  x-audience: component-internal
  x-application-id: APP-12345
  description: |
    Sending AMMA metadata events to the topic.
  license:
    name: Apache 2.0
    url: 'https://www.apache.org/licenses/LICENSE-2.0'
servers:
  production:
    url: 'pulsar+ssl://prod.events.managed.async.api.legogroup.io:6651'
    protocol: pulsar+ssl
    description: Pulsar broker
channels:
  workspace:
    x-eventarchetype: objectchanged
    x-classification: green
    x-datalakesubscription: true
    publish:
      bindings:
        http:
          type: response
      message:
        $ref: '#/components/messages/WorkspaceEventPayload'
  api:
    x-eventarchetype: objectchanged
    x-classification: green
    x-datalakesubscription: true
    publish:
      bindings:
        http:
          type: response
      message:
        $ref: '#/components/messages/APIEventPayload'
components:
  messages:
    WorkspaceEventPayload:
      schemaFormat: application/schema+yaml;version=draft-07
      summary: Metadata about a workspace that has been created, updated or deleted.
      payload:
        type: object
        properties:
          key:
            type: string
            description: Key of the message.
          event:
            type: string
            description: Event type.
          payload:
            type: object
            properties:
              workspace:
                type: string
                description: Name of the workspace.
              href:
                type: string
                description: Send an API request to this url for detailed data on the referenced workspace.
                
    APIEventPayload:
      schemaFormat: application/schema+yaml;version=draft-07
      summary: Metadata about an API that has been created, updated or deleted.
      payload:
        type: object
        properties:
          key:
            type: string
            description: Key of the message.
          event:
            type: string
            description: Event type.
          payload:
            type: object
            properties:
              workspace:
                type: string
                description: Name of the workspace.
              api:
                type: string
                description: Name of the API.
              href:
                type: string
                description: Send an API request to this url for detailed data on the referenced API.
";
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);
            Assert.AreEqual((doc.Channels["workspace"].Extensions["x-eventarchetype"] as AsyncApiString).Value, "objectchanged");
            Assert.AreEqual((doc.Channels["workspace"].Extensions["x-classification"] as AsyncApiString).Value, "green");
            Assert.AreEqual((doc.Channels["workspace"].Extensions["x-datalakesubscription"] as AsyncApiBoolean).Value, true);
            var message = doc.Channels["workspace"].Publish.Message;
            Assert.AreEqual(message.SchemaFormat, "application/schema+yaml;version=draft-07");
            Assert.AreEqual(message.Summary, "Metadata about a workspace that has been created, updated or deleted.");
            var payload = doc.Channels["workspace"].Publish.Message.Payload;
            Assert.NotNull(payload);
            Assert.AreEqual(typeof(AsyncApiObject), payload.GetType());
        }
    }
}