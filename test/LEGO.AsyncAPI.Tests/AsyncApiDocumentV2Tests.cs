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

    public class AsyncApiDocumentV2Tests
    {
        [Test]
        public void AsyncApiDocument_WithStreetLightsExample_SerializesAndDeserializes()
        {
            // Arrange
            var expected =
@"asyncapi: '2.6.0'
info:
  title: Streetlights Kafka API
  version: 1.0.0
  description: The Smartylighting Streetlights API allows you to remotely manage the city lights.
  license:
    name: Apache 2.0
    url: https://www.apache.org/licenses/LICENSE-2.0
servers:
  scram-connections:
    url: test.mykafkacluster.org:18092
    protocol: kafka-secure
    description: Test broker secured with scramSha256
    security:
      - saslScram: []
    tags:
      - name: env:test-scram
        description: This environment is meant for running internal tests through scramSha256
      - name: kind:remote
        description: This server is a remote server. Not exposed by the application
      - name: visibility:private
        description: This resource is private and only available to certain users
  mtls-connections:
    url: test.mykafkacluster.org:28092
    protocol: kafka-secure
    description: Test broker secured with X509
    security:
      - certs: []
    tags:
      - name: env:test-mtls
        description: This environment is meant for running internal tests through mtls
      - name: kind:remote
        description: This server is a remote server. Not exposed by the application
      - name: visibility:private
        description: This resource is private and only available to certain users
defaultContentType: application/json
channels:
  'smartylighting.streetlights.1.0.event.{streetlightId}.lighting.measured':
    description: The topic on which measured values may be produced and consumed.
    publish:
      operationId: receiveLightMeasurement
      summary: Inform about environmental lighting conditions of a particular streetlight.
      traits:
        - $ref: '#/components/operationTraits/kafka'
      message:
        $ref: '#/components/messages/lightMeasured'
    parameters:
      streetlightId:
        $ref: '#/components/parameters/streetlightId'
  'smartylighting.streetlights.1.0.action.{streetlightId}.turn.on':
    subscribe:
      operationId: turnOn
      traits:
        - $ref: '#/components/operationTraits/kafka'
      message:
        $ref: '#/components/messages/turnOnOff'
    parameters:
      streetlightId:
        $ref: '#/components/parameters/streetlightId'
  'smartylighting.streetlights.1.0.action.{streetlightId}.turn.off':
    subscribe:
      operationId: turnOff
      traits:
        - $ref: '#/components/operationTraits/kafka'
      message:
        $ref: '#/components/messages/turnOnOff'
    parameters:
      streetlightId:
        $ref: '#/components/parameters/streetlightId'
  'smartylighting.streetlights.1.0.action.{streetlightId}.dim':
    subscribe:
      operationId: dimLight
      traits:
        - $ref: '#/components/operationTraits/kafka'
      message:
        $ref: '#/components/messages/dimLight'
    parameters:
      streetlightId:
        $ref: '#/components/parameters/streetlightId'
components:
  schemas:
    lightMeasuredPayload:
      type: object
      properties:
        lumens:
          type: integer
          description: Light intensity measured in lumens.
          minimum: 0
        sentAt:
          $ref: '#/components/schemas/sentAt'
    turnOnOffPayload:
      type: object
      properties:
        command:
          type: string
          description: Whether to turn on or off the light.
          enum:
            - on
            - off
        sentAt:
          $ref: '#/components/schemas/sentAt'
    dimLightPayload:
      type: object
      properties:
        percentage:
          type: integer
          description: Percentage to which the light should be dimmed to.
          maximum: 100
          minimum: 0
        sentAt:
          $ref: '#/components/schemas/sentAt'
    sentAt:
      type: string
      format: date-time
      description: Date and time when the message was sent.
  messages:
    lightMeasured:
      payload:
        $ref: '#/components/schemas/lightMeasuredPayload'
      contentType: application/json
      name: lightMeasured
      title: Light measured
      summary: Inform about environmental lighting conditions of a particular streetlight.
      traits:
        - $ref: '#/components/messageTraits/commonHeaders'
    turnOnOff:
      payload:
        $ref: '#/components/schemas/turnOnOffPayload'
      name: turnOnOff
      title: Turn on/off
      summary: Command a particular streetlight to turn the lights on or off.
      traits:
        - $ref: '#/components/messageTraits/commonHeaders'
    dimLight:
      payload:
        $ref: '#/components/schemas/dimLightPayload'
      name: dimLight
      title: Dim light
      summary: Command a particular streetlight to dim the lights.
      traits:
        - $ref: '#/components/messageTraits/commonHeaders'
  securitySchemes:
    saslScram:
      type: scramSha256
      description: Provide your username and password for SASL/SCRAM authentication
    certs:
      type: X509
      description: Download the certificate files from service provider
  parameters:
    streetlightId:
      description: The ID of the streetlight.
      schema:
        type: string
  operationTraits:
    kafka:
      bindings:
        kafka:
          clientId:
            type: string
            enum:
              - my-app-id
  messageTraits:
    commonHeaders:
      headers:
        type: object
        properties:
          my-app-header:
            type: integer
            maximum: 100
            minimum: 0";

        var asyncApiDocument = new AsyncApiDocumentBuilder()
            .WithInfo(new AsyncApiInfo
            {
                Title = "Streetlights Kafka API",
                Version = "1.0.0",
                Description = "The Smartylighting Streetlights API allows you to remotely manage the city lights.",
                License = new AsyncApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0"),
                },
            })
            .WithServer("scram-connections", new AsyncApiServer
            {
                Url = "test.mykafkacluster.org:18092",
                Protocol = "kafka-secure",
                Description = "Test broker secured with scramSha256",
                Security = new List<AsyncApiSecurityRequirement>
                {
                    new AsyncApiSecurityRequirement
                    {
                        {
                            new AsyncApiSecurityScheme()
                            {
                                Reference = new AsyncApiReference()
                                {
                                    Id = "saslScram",
                                    Type = ReferenceType.SecurityScheme,
                                },
                            }, new List<string>()
                        },
                    },
                },
                Tags = new List<AsyncApiTag>
                {
                    new AsyncApiTag
                    {
                        Name = "env:test-scram",
                        Description = "This environment is meant for running internal tests through scramSha256",
                    },
                    new AsyncApiTag
                    {
                        Name = "kind:remote",
                        Description = "This server is a remote server. Not exposed by the application",
                    },
                    new AsyncApiTag
                    {
                        Name = "visibility:private",
                        Description = "This resource is private and only available to certain users",
                    },
                },
            })
            .WithServer("mtls-connections", new AsyncApiServer
            {
                Url = "test.mykafkacluster.org:28092",
                Protocol = "kafka-secure",
                Description = "Test broker secured with X509",
                Security = new List<AsyncApiSecurityRequirement>
                {
                    new AsyncApiSecurityRequirement
                    {
                        {
                            new AsyncApiSecurityScheme()
                            {
                                Reference = new AsyncApiReference()
                                {
                                    Id = "certs",
                                    Type = ReferenceType.SecurityScheme,
                                },
                            }, new List<string>()
                        },
                    },
                },
                Tags = new List<AsyncApiTag>
                {
                    new AsyncApiTag
                    {
                        Name = "env:test-mtls",
                        Description = "This environment is meant for running internal tests through mtls",
                    },
                    new AsyncApiTag
                    {
                        Name = "kind:remote",
                        Description = "This server is a remote server. Not exposed by the application",
                    },
                    new AsyncApiTag
                    {
                        Name = "visibility:private",
                        Description = "This resource is private and only available to certain users",
                    },
                },
            })
            .WithDefaultContentType()
            .WithChannel(
            "smartylighting.streetlights.1.0.event.{streetlightId}.lighting.measured",
            new AsyncApiChannel()
            {
                Description = "The topic on which measured values may be produced and consumed.",
                Parameters = new Dictionary<string, AsyncApiParameter>
                {
                    {
                        "streetlightId", new AsyncApiParameter()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "streetlightId",
                                Type = ReferenceType.Parameter,
                            },
                        }
                    },
                },
                Publish = new AsyncApiOperation()
                {
                    Summary = "Inform about environmental lighting conditions of a particular streetlight.",
                    OperationId = "receiveLightMeasurement",
                    Traits = new List<AsyncApiOperationTrait>
                    {
                        new AsyncApiOperationTrait()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "kafka",
                                Type = ReferenceType.OperationTrait,
                            },
                        },
                    },
                    Message = new List<AsyncApiMessage>
                    {
                        new AsyncApiMessage()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "lightMeasured",
                                Type = ReferenceType.Message,
                            },
                        },
                    },
                },
            })
            .WithChannel(
            "smartylighting.streetlights.1.0.action.{streetlightId}.turn.on",
            new AsyncApiChannel()
            {
                Parameters = new Dictionary<string, AsyncApiParameter>
                {
                    {
                        "streetlightId", new AsyncApiParameter()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "streetlightId",
                                Type = ReferenceType.Parameter,
                            },
                        }
                    },
                },
                Subscribe = new AsyncApiOperation()
                {
                    OperationId = "turnOn",
                    Traits = new List<AsyncApiOperationTrait>
                    {
                        new AsyncApiOperationTrait()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "kafka",
                                Type = ReferenceType.OperationTrait,
                            },
                        },
                    },
                    Message = new List<AsyncApiMessage>
                    {
                        new AsyncApiMessage()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "turnOnOff",
                                Type = ReferenceType.Message,
                            },
                        },
                    },
                },
            })
            .WithChannel(
            "smartylighting.streetlights.1.0.action.{streetlightId}.turn.off",
            new AsyncApiChannel()
            {
                Parameters = new Dictionary<string, AsyncApiParameter>
                {
                    {
                        "streetlightId", new AsyncApiParameter()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "streetlightId",
                                Type = ReferenceType.Parameter,
                            },
                        }
                    },
                },
                Subscribe = new AsyncApiOperation()
                {
                    OperationId = "turnOff",
                    Traits = new List<AsyncApiOperationTrait>
                    {
                        new AsyncApiOperationTrait()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "kafka",
                                Type = ReferenceType.OperationTrait,
                            },
                        },
                    },
                    Message = new List<AsyncApiMessage>
                    {
                        new AsyncApiMessage()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "turnOnOff",
                                Type = ReferenceType.Message,
                            },
                        },
                    },
                },
            })
            .WithChannel(
            "smartylighting.streetlights.1.0.action.{streetlightId}.dim",
            new AsyncApiChannel()
            {
                Parameters = new Dictionary<string, AsyncApiParameter>
                {
                    {
                        "streetlightId", new AsyncApiParameter()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "streetlightId",
                                Type = ReferenceType.Parameter,
                            },
                        }
                    },
                },
                Subscribe = new AsyncApiOperation()
                {
                    OperationId = "dimLight",
                    Traits = new List<AsyncApiOperationTrait>
                    {
                        new AsyncApiOperationTrait()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "kafka",
                                Type = ReferenceType.OperationTrait,
                            },
                        },
                    },
                    Message = new List<AsyncApiMessage>
                    {
                        new AsyncApiMessage()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Id = "dimLight",
                                Type = ReferenceType.Message,
                            },
                        },
                    },
                },
            })
            .WithComponent("lightMeasured", new AsyncApiMessage()
            {
                Name = "lightMeasured",
                Title = "Light measured",
                Summary = "Inform about environmental lighting conditions of a particular streetlight.",
                ContentType = "application/json",
                Traits = new List<AsyncApiMessageTrait>()
                {
                    new AsyncApiMessageTrait()
                    {
                        Reference = new AsyncApiReference()
                        {
                            Type = ReferenceType.MessageTrait,
                            Id = "commonHeaders",
                        },
                    },
                },
                Payload = new AsyncApiSchema()
                {
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = "lightMeasuredPayload",
                    },
                },
            })
            .WithComponent("turnOnOff", new AsyncApiMessage()
            {
                Name = "turnOnOff",
                Title = "Turn on/off",
                Summary = "Command a particular streetlight to turn the lights on or off.",
                Traits = new List<AsyncApiMessageTrait>()
                {
                    new AsyncApiMessageTrait()
                    {
                        Reference = new AsyncApiReference()
                        {
                            Type = ReferenceType.MessageTrait,
                            Id = "commonHeaders",
                        },
                    },
                },
                Payload = new AsyncApiSchema()
                {
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = "turnOnOffPayload",
                    },
                },
            })
            .WithComponent("dimLight", new AsyncApiMessage()
            {
                Name = "dimLight",
                Title = "Dim light",
                Summary = "Command a particular streetlight to dim the lights.",
                Traits = new List<AsyncApiMessageTrait>()
                {
                    new AsyncApiMessageTrait()
                    {
                        Reference = new AsyncApiReference()
                        {
                            Type = ReferenceType.MessageTrait,
                            Id = "commonHeaders",
                        },
                    },
                },
                Payload = new AsyncApiSchema()
                {
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = "dimLightPayload",
                    },
                },
            })
            .WithComponent("lightMeasuredPayload", new AsyncApiSchema()
            {
                Type = new List<SchemaType> { SchemaType.Object },
                Properties = new Dictionary<string, AsyncApiSchema>()
                {
                    {
                        "lumens", new AsyncApiSchema()
                        {
                            Type = new List<SchemaType> { SchemaType.Integer },
                            Minimum = 0,
                            Description = "Light intensity measured in lumens.",
                        }
                    },
                    {
                        "sentAt", new AsyncApiSchema()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Type = ReferenceType.Schema,
                                Id = "sentAt",
                            },
                        }
                    },
                },
            })
            .WithComponent("turnOnOffPayload", new AsyncApiSchema()
            {
                Type = new List<SchemaType> { SchemaType.Object },
                Properties = new Dictionary<string, AsyncApiSchema>()
                {
                    {
                        "command", new AsyncApiSchema()
                        {
                            Type = new List<SchemaType> { SchemaType.String },
                            Enum = new List<IAsyncApiAny>
                            {
                                new AsyncApiString("on"),
                                new AsyncApiString("off"),
                            },
                            Description = "Whether to turn on or off the light."
                        }
                    },
                    {
                        "sentAt", new AsyncApiSchema()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Type = ReferenceType.Schema,
                                Id = "sentAt",
                            },
                        }
                    },
                },
            })
            .WithComponent("dimLightPayload", new AsyncApiSchema()
            {
                Type = new List<SchemaType> { SchemaType.Object },
                Properties = new Dictionary<string, AsyncApiSchema>()
                {
                    {
                        "percentage", new AsyncApiSchema()
                        {
                            Type = new List<SchemaType> { SchemaType.Integer },
                            Description = "Percentage to which the light should be dimmed to.",
                            Minimum = 0,
                            Maximum = 100,
                        }
                    },
                    {
                        "sentAt", new AsyncApiSchema()
                        {
                            Reference = new AsyncApiReference()
                            {
                                Type = ReferenceType.Schema,
                                Id = "sentAt",
                            },
                        }
                    },
                },
            })
            .WithComponent("sentAt", new AsyncApiSchema()
            {
                Type = new List<SchemaType> { SchemaType.String },
                Format = "date-time",
                Description = "Date and time when the message was sent.",

            })
            .WithComponent("saslScram", new AsyncApiSecurityScheme
            {
                Type = SecuritySchemeType.ScramSha256,
                Description = "Provide your username and password for SASL/SCRAM authentication",
            })
            .WithComponent("certs", new AsyncApiSecurityScheme
            {
                Type = SecuritySchemeType.X509,
                Description = "Download the certificate files from service provider",
            })
            .WithComponent("streetlightId", new AsyncApiParameter()
            {
                Description = "The ID of the streetlight.",
                Schema = new AsyncApiSchema()
                {
                    Type = new List<SchemaType> { SchemaType.String },
                },
            })
            .WithComponent("commonHeaders", new AsyncApiMessageTrait()
            {
                Headers = new AsyncApiSchema()
                {
                    Type = new List<SchemaType> { SchemaType.Object },
                    Properties = new Dictionary<string, AsyncApiSchema>()
                    {
                        {
                            "my-app-header", new AsyncApiSchema()
                            {
                                Type = new List<SchemaType> { SchemaType.Integer },
                                Minimum = 0,
                                Maximum = 100,
                            }
                        },
                    },
                },
            })
            .WithComponent("kafka", new AsyncApiOperationTrait()
            {
                Bindings = new AsyncApiBindings<IOperationBinding>()
                {
                    {
                        BindingType.Kafka, new KafkaOperationBinding()
                        {
                            ClientId = new AsyncApiSchema()
                            {
                                Type = new List<SchemaType> { SchemaType.String },
                                Enum = new List<IAsyncApiAny>
                                {
                                    new AsyncApiString("my-app-id"),
                                },
                            },
                        }
                    },
                },
            })
            .Build();

            // Act
            var actual = asyncApiDocument.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void SerializeV2_WithFullSpec_Serializes()
        {
            var expected =
                @"asyncapi: '2.6.0'
info:
  title: apiTitle
  version: apiVersion
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
  x-extension: value
id: documentId
servers:
  myServer:
    url: https://example.com/server
    protocol: KafkaProtocol
    protocolVersion: protocolVersion
    description: serverDescription
    security:
      - securitySchemeName:
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
              x-extension: value
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
        public void Serializev2_WithBindings_Serializes()
        {
            var expected = @"asyncapi: '2.6.0'
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
