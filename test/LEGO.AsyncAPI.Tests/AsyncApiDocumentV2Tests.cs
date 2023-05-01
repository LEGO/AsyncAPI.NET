namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using AsyncAPI.Bindings.Pulsar;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Bindings.Http;
    using LEGO.AsyncAPI.Bindings.Kafka;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Bindings;
    using LEGO.AsyncAPI.Models.Bindings.Kafka;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Writers;
    using NUnit.Framework;

    public class AsyncApiDocumentV2Tests
    {

        [Test]
        public void test()
        {
            var input = @"asyncapi: 2.0.0
info:
  title: Sites
  version: 1.0.0
  description: Responsible for emitting the site on/off
  x-application-id: APP-02042
  x-audience: component-internal
channels:
  site-events:
    subscribe:
      message:
        payload:
          properties:
            data:
              '$ref': '#/components/schemas/DtosSiteUpdatedEvent'
              description: The actual payload of the event
            datacontenttype:
              description: Always application/json
              type: string
              example: application/json
            id:
              description: The unique ID of the event
              type: string
              example: 3489d4b1e21badf3665dae24c6526169
            source:
              description: The source of the event
              type: string
              example: LEGO.OmnichannelFulfilment.DeliveryOrchestration/Sites
            specversion:
              description: The CloudEvents schema version used
              type: string
              example: 1.0
            time:
              description: The time the event was published
              format: date-time
              type: string
              example: 2022-10-13T11:57:36.268054757Z
            type:
              description: The type of the event
              type: string
              example: siteUpdatedV1
            traceparent:
              description: The value to propagate context information that enables distributed tracing scenarios
              type: string
              example: 3489d4b1e21badf3665dae24c6526169
          type: object
        summary: Subscriber message
        description: All data used for turning on or off a site request
    x-classification: green
    x-datalakesubscription: false
    x-eventarchetype: objectchanged
    x-eventdurability: persistent
components:
  schemas:
    DtosSiteUpdatedEvent:
      properties:
        enabled:
          description: The Enabled shows the current status of the site
          type: boolean
          examples:
          - false
        siteId:
          description: The SiteCode related to the site that is being turn on or off
          type: string
          examples:
          - 489
        reason:
          description: The reason the site is being turned on or off
          type: string
          examples:
          - Workers striking require us to temporary close the site.
      type: object
";
            var serialized = new AsyncApiStringReader().Read(input, out var diag);

        }
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
                        "kafka", new KafkaOperationBinding()
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
            Assert.AreEqual(expected, actual);
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
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void tesT()
        {
            var spec = @"asyncapi: '2.6.0'
defaultContentType: 'application/json'
info:
  title: '{TITLE}'
  version: '{VERSION}'
  x-audience: company-internal
  x-application-id: APP-01575
  x-eventdeduplication: false
  contact:
    name: Team Deadlock
    email: Deadlock@o365.corp.LEGO.com
    url: https://legogroup.atlassian.net/wiki/spaces/TD/pages/37143022928/Consent+Service
  description: |
    Emits events related to consent changes for both LEGO Account users and anonymous users.
    This includes both parental consents and cookie consents.
channels:
  userconsents.objectchanged:
    x-eventarchetype: objectchanged
    x-eventdurability: persistent
    x-classification: yellow
    description: |
      A topic for events regarding changes to user consents. The event archetype is set to 'objectchanged' which will enable tombstoning and compaction. 
    subscribe:
      operationId: UserConsentsObjectChanged
      message:
        oneOf:
          - $ref: '#/components/messages/UserConsentsObjectCreated'
          - $ref: '#/components/messages/UserConsentsObjectChanged'
          - $ref: '#/components/messages/UserConsentsObjectDeleted'
  userconsents.fieldchanged:
    x-eventarchetype: fieldchanged
    x-eventdurability: persistent
    x-classification: yellow
    description: |
      A topic for deleted user consents events. The event archetype is set to 'fieldchanged' in order to enforce a 28 days retention policy. 
    subscribe:
      operationId: UserConsentsFieldChanged
      message:
        oneOf:
          - $ref: '#/components/messages/UserConsentFieldCreated'
          - $ref: '#/components/messages/UserConsentFieldChanged'
          - $ref: '#/components/messages/UserConsentFieldDeleted'
  anonymousconsent.fieldchange:
    x-eventarchetype: fieldchanged
    x-eventdurability: persistent
    x-classification: green
    description: |
      A topic for events regarding changes to anonymous consents. The event archetype is set to 'fieldchanged' in order to enforce a 28 days retention policy.
    subscribe:
      operationId: AnonymousConsentsFieldChanged
      message:
        oneOf:
          - $ref: '#/components/messages/AnonymousConsentFieldChange'
  experiences.events:
    x-eventarchetype: objectchanged
    x-eventdurability: persistent
    x-classification: yellow
    description: |
      A topic for experience events. The event archetype is set to 'objectchanged' in order to store event forever.
    subscribe:
      operationId: ExperiencesChanged
      message:
        oneOf:
          - $ref: '#/components/messages/ExperienceCreated'
          - $ref: '#/components/messages/ExperienceDeleted'
          - $ref: '#/components/messages/ExperienceUpdated'
          - $ref: '#/components/messages/ExperienceClientAdded'
          - $ref: '#/components/messages/ExperienceClientRemoved'
          - $ref: '#/components/messages/ExperienceConsentOptionAdded'
          - $ref: '#/components/messages/ExperienceConsentOptionRemoved'
components:
  schemas:
    EnvelopeBase:
      type: object
      properties:
        type: 
          type: string
          description: The type of event.
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header         
        data:
          type: object

    EnvelopeOfUserConsentsObjectCreated:
      allOf:
        - $ref: '#/components/schemas/EnvelopeBase'
        - type: object
          properties:
            type: 
              enum: [UserConsentsObjectCreated]
            data:
              $ref: '#/components/schemas/UserConsentsObjectCreated'

    EnvelopeOfUserConsentsObjectChanged:
      allOf:
        - $ref: '#/components/schemas/EnvelopeBase'
        - type: object
          properties:
            type: 
              enum: [UserConsentsObjectChanged]
            data:
              $ref: '#/components/schemas/UserConsentsObjectChanged'
    EnvelopeOfUserConsentsObjectDeleted:
      allOf:
        - $ref: '#/components/schemas/EnvelopeBase'
        - type: object
          properties:
            type: 
              enum: [UserConsentsObjectDeleted]
            data:
              $ref: '#/components/schemas/UserConsentsObjectDeleted'
    UserConsentsObjectCreated:
      type: object
      required:
        - changeType
        - changeTime
        - userId
        - consents
      properties:
        changeType:
          type: string
          enum: ['created']
          description: The change type of the event.
        changeTime:
          type: string
          format: date-time
          description: 'The date and time of when the user consent was changed. The format is in RFC 3339.'
          examples: 
            - 2023-01-18T13:19:08.132084
        userId:
          type: string
          format: guid
          description: The ID of the user. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consents:
          type: array
          items:
            $ref: '#/components/schemas/UserConsent'
    UserConsentsObjectChanged:
      type: object
      required:
        - changeType
        - changeTime
        - userId
        - consents
      properties:
        changeType:
          type: string
          enum: ['updated']
          description: The change type of the event.
        changeTime:
          type: string
          format: date-time
          description: 'The date and time of when the user consent was changed. The format is in RFC 3339.'
          examples: 
            - 2023-01-18T13:19:08.132084
        userId:
          type: string
          format: guid
          description: The ID of the user. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consents:
          type: array
          items:
            $ref: '#/components/schemas/UserConsent'
    UserConsentsObjectDeleted:
      type: object
      required:
        - changeType
        - changeTime
        - userId
      properties:
        changeType:
          type: string
          enum: ['deleted']
          description: The change type of the event.
        changeTime:
          type: string
          format: date-time
          description: 'The date and time of when the user consent was changed. The format is in RFC 3339.'
          examples: 
            - 2023-01-18T13:19:08.132084
        userId:
          type: string
          format: guid
          description: The ID of the user. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
    UserConsent:
      type: object
      required:
        - consentId
        - consenterUserId
        - consentState
        - culture
      properties:
        consentId:
          type: string
          format: uri
          description: The consent option URI
          examples: 
            - self-consent://global/analytic-cookies
            - self-consent://global/necessary-cookies
            - self-consent://global/lego-marketing-cookies
        consenterUserId:
          type: string
          format: guid
          description: The ID of the user giving the consent. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consentState:
          type: string
          description: The state of the consent for the given consent option
          enum: ['granted','denied','undecided']
        culture:
          type: string
          minLength: 5
          maxLength: 5
          description: The culture where the consent was changed.
          examples: 
            - da-DK
            - en-US
            - en-GB
        submissionSource:
          type: string
          description: The method used by the user to submit the cookies
          enum: ['prebannerAcceptAll', 'prebannerRejectAll', 'savePrefButton', 'cloned' ]    
        
    EnvelopeOfUserConsentFieldCreatedEvent:
      allOf:
        - $ref: '#/components/schemas/EnvelopeBase'
        - type: object
          properties:
            type: 
              enum: [UserConsentFieldCreatedEvent]
            data:
              $ref: '#/components/schemas/UserConsentFieldCreatedEvent'
    EnvelopeOfUserConsentFieldChangedEvent:
      allOf:
        - $ref: '#/components/schemas/EnvelopeBase'
        - type: object
          properties:
            type: 
              enum: [UserConsentFieldChangedEvent]
            data:
              $ref: '#/components/schemas/UserConsentFieldChangedEvent'
    EnvelopeOfUserConsentFieldDeletedEvent:
      allOf:
        - $ref: '#/components/schemas/EnvelopeBase'
        - type: object
          properties:
            type: 
              enum: [UserConsentFieldDeletedEvent]
            data:
              $ref: '#/components/schemas/UserConsentFieldDeletedEvent'

    UserConsentFieldCreatedEvent:
      type: object
      required:
        - changeType
        - changeTime
        - userId
        - consentId
        - consenterUserId
        - consentState
        - culture
      properties:
        changeType:
          type: string
          enum: ['created','updated','deleted']
          description: The change type of the event.
        changeTime:
          type: string
          format: date-time
          description: 'The date and time of when the user consent was changed. The format is in RFC 3339.'
          examples: 
            - 2023-01-18T13:19:08.132084
        userId:
          type: string
          format: guid
          description: The ID of the user. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consentId:
          type: string
          format: uri
          description: The consent option URI
          examples: 
            - self-consent://global/analytic-cookies
            - self-consent://global/necessary-cookies
            - self-consent://global/lego-marketing-cookies
        consenterUserId:
          type: string
          format: guid
          description: The ID of the user giving the consent. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consentState:
          type: string
          description: The state of the consent for the given consent option
          enum: ['granted','denied','undecided']
        culture:
          type: string
          minLength: 5
          maxLength: 5
          description: The culture where the consent was changed.
          examples: 
            - da-DK
            - en-US
            - en-GB
        submissionSource:
          type: string
          description: The method used by the user to submit the cookies
          enum: ['prebannerAcceptAll', 'prebannerRejectAll', 'savePrefButton', 'cloned' ]  

    UserConsentFieldChangedEvent:
      type: object
      required:
        - changeType
        - changeTime
        - userId
        - consentId
        - consenterUserId
        - consentState
        - culture
      properties:
        changeType:
          type: string
          enum: ['created','updated','deleted']
          description: The change type of the event.
        changeTime:
          type: string
          format: date-time
          description: 'The date and time of when the user consent was changed. The format is in RFC 3339.'
          examples: 
            - 2023-01-18T13:19:08.132084
        userId:
          type: string
          format: guid
          description: The ID of the user. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consentId:
          type: string
          format: uri
          description: The consent option URI
          examples: 
            - self-consent://global/analytic-cookies
            - self-consent://global/necessary-cookies
            - self-consent://global/lego-marketing-cookies
        consenterUserId:
          type: string
          format: guid
          description: The ID of the user giving the consent. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consentState:
          type: string
          description: The state of the consent for the given consent option
          enum: ['granted','denied','undecided']
        culture:
          type: string
          minLength: 5
          maxLength: 5
          description: The culture where the consent was changed.
          examples: 
            - da-DK
            - en-US
            - en-GB
        submissionSource:
          type: string
          description: The method used by the user to submit the cookies
          enum: ['prebannerAcceptAll', 'prebannerRejectAll', 'savePrefButton', 'cloned' ]  

    UserConsentFieldDeletedEvent:
      type: object
      required:
        - changeType
        - changeTime
        - userId
        - consentId
      properties:
        changeType:
          type: string
          enum: ['deleted']
          description: The change type of the event.
        changeTime:
          type: string
          format: date-time
          description: 'The date and time of when the user consent was changed. The format is in RFC 3339.'
          examples: 
            - 2023-01-18T13:19:08.132084
        userId:
          type: string
          format: guid
          description: The ID of the user. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consentId:
          type: string
          format: uri
          description: The consent option URI
          examples: 
            - self-consent://global/analytic-cookies
            - self-consent://global/necessary-cookies
            - self-consent://global/lego-marketing-cookies

    EnvelopeOfAnonymousConsentFieldChangeEvent:
      type: object
      properties:
        type:
          type: string
          description: The type of event. Will always have the value 'anonymousconsents.fieldchanged'.
          enum: [anonymousconsents.fieldchanged]
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header
        data:
          $ref: '#/components/schemas/AnonymousConsentFieldChangeEvent'
    EnvelopeOfExperienceCreatedEvent:
      type: object
      properties:
        type:
          type: string
          description: The type of event. Will always have the value 'experience.created'.
          enum: [experience.created]
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header
        data:
          $ref: '#/components/schemas/ExperienceCreatedEvent'
    EnvelopeOfExperienceDeletedEvent:
      type: object
      properties:
        type:
          type: string
          description: The type of event. Will always have the value 'experience.deleted'.
          enum: [experience.deleted]
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header
        data:
          $ref: '#/components/schemas/ExperienceDeletedEvent'
    EnvelopeOfExperienceUpdatedEvent:
      type: object
      properties:
        type:
          type: string
          description: The type of event. Will always have the value 'experience.updated'.
          enum: [experience.updated]
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header
        data:
          $ref: '#/components/schemas/ExperienceUpdatedEvent'
    EnvelopeOfExperienceClientAddedEvent:
      type: object
      properties:
        type:
          type: string
          description: The type of event. Will always have the value 'experience.client.added'.
          enum: [experience.client.added]
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header
        data:
          $ref: '#/components/schemas/ExperienceClientAddedEvent'
    EnvelopeOfExperienceClientRemovedEvent:
      type: object
      properties:
        type:
          type: string
          description: The type of event. Will always have the value 'experience.client.removed'.
          enum: [experience.client.removed]
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header
        data:
          $ref: '#/components/schemas/ExperienceClientRemovedEvent'
    EnvelopeOfExperienceConsentOptionAddedEvent:
      type: object
      properties:
        type:
          type: string
          description: The type of event. Will always have the value 'experience.consentoption.added'.
          enum: [experience.consentoption.added]
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header
        data:
          $ref: '#/components/schemas/ExperienceConsentOptionAddedEvent'
    EnvelopeOfExperienceConsentOptionRemovedEvent:
      type: object
      properties:
        type:
          type: string
          description: The type of event. Will always have the value 'experience.consentoption.removed'.
          enum: [experience.consentoption.removed]
        correlationId:
          type: string
          description: |
            The correlation ID as defined in https://github.com/LEGO/api-matters/blob/main/docs/practices/sync-apis/restful/readme.md#124-correlation-id-header
        data:
          $ref: '#/components/schemas/ExperienceConsentOptionRemovedEvent'
    
    AnonymousConsentFieldChangeEvent:
      type: object
      required:
        - ChangeType
        - ChangeTime
        - AnonymousUserId
        - ConsentId
        - ConsentState
        - Culture
      properties:
        changeType:
          type: string
          description: The change type
          examples:
            - Created
            - Updated
            - Deleted
        changeTime:
          type: string
          format: date-time
          description: 'The date and time of when the user consent was changed. The format is in RFC 3339.'
          examples:
            - 2023-01-18T13:19:08.132084
        anonymousUserId:
          type: string
          format: guid
          description: The anonymous user ID. The GUID is 32 digits separated by hyphens and is not considered to be PII.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
        consentId:
          type: string
          description: The consent option URI
          examples:
            - self-consent://global/analytic-cookies
            - self-consent://global/necessary-cookies
            - self-consent://global/lego-marketing-cookies
        consentState:
          type: string
          description: The state of the consent for the given consent id
          examples:
            - granted
            - denied
            - undecided
        culture:
          type: string
          minLength: 5
          maxLength: 5
          description: The culture where the consent was changed.
          examples:
            - da-DK
            - en-US
            - en-GB
    ExperienceCreatedEvent:
      type: object
      required:
        - experienceId
        - occurredOnTimestamp
        - name
      properties:
        experienceId:
          type: string
          description: The experience id.
          minLength: 1
          maxLength: 40
          examples:
            - lego.com
        occurredOnTimestamp:
          type: string
          format: date-time
          description: 'The date and time of when experience was created. The format is in RFC 3339.'
          examples:
            - 2023-01-18T13:19:08.132084
        name:
          type: string
          description: name of experience
          minLength: 1
          maxLength: 100
          examples:
            - LEGO Webshop
    ExperienceUpdatedEvent:
      type: object
      required:
        - experienceId
        - occurredOnTimestamp
        - name
      properties:
        experienceId:
          type: string
          description: The experience id.
          minLength: 1
          maxLength: 40
          examples:
            - lego.com
        occurredOnTimestamp:
          type: string
          format: date-time
          description: 'The date and time of when experience was created. The format is in RFC 3339.'
          examples:
            - 2023-01-18T13:19:08.132084
        name:
          type: string
          description: name of experience
          minLength: 1
          maxLength: 100
          examples:
            - LEGO Webshop
    ExperienceDeletedEvent:
      type: object
      required:
        - experienceId
        - occurredOnTimestamp
      properties:
        experienceId:
          type: string
          description: The experience id.
          minLength: 1
          maxLength: 40
          examples:
            - lego.com
        occurredOnTimestamp:
          type: string
          format: date-time
          description: 'The date and time of when experience was created. The format is in RFC 3339.'
          examples:
            - 2023-01-18T13:19:08.132084
    ExperienceClientAddedEvent:
      type: object
      required:
        - experienceId
        - occurredOnTimestamp
        - clientId
      properties:
        experienceId:
          type: string
          description: The experience id.
          minLength: 1
          maxLength: 40
          examples:
            - lego.com
        occurredOnTimestamp:
          type: string
          format: date-time
          description: 'The date and time of when experience was created. The format is in RFC 3339.'
          examples:
            - 2023-01-18T13:19:08.132084
        clientId:
          type: string
          format: guid
          description: Identity client id.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
    ExperienceClientRemovedEvent:
      type: object
      required:
        - experienceId
        - occurredOnTimestamp
        - clientId
      properties:
        experienceId:
          type: string
          description: The experience id.
          minLength: 1
          maxLength: 40
          examples:
            - lego.com
        occurredOnTimestamp:
          type: string
          format: date-time
          description: 'The date and time of when experience was created. The format is in RFC 3339.'
          examples:
            - 2023-01-18T13:19:08.132084
        clientId:
          type: string
          format: guid
          description: Identity client id.
          minLength: 36
          maxLength: 36
          examples:
            - 95b2cb5f-d551-4106-805c-9b800b1a0133
    ExperienceConsentOptionAddedEvent:
      type: object
      required:
        - experienceId
        - occurredOnTimestamp
        - consentOption
      properties:
        experienceId:
          type: string
          description: The experience id.
          minLength: 1
          maxLength: 40
          examples:
            - lego.com
        occurredOnTimestamp:
          type: string
          format: date-time
          description: 'The date and time of when experience was created. The format is in RFC 3339.'
          examples:
            - 2023-01-18T13:19:08.132084
        clientId:
          type: string
          description: The consent option URI
          examples:
            - self-consent://global/analytic-cookies
            - self-consent://global/necessary-cookies
            - self-consent://global/lego-marketing-cookies
    ExperienceConsentOptionRemovedEvent:
      type: object
      required:
        - experienceId
        - occurredOnTimestamp
        - consentOption
      properties:
        experienceId:
          type: string
          description: The experience id.
          minLength: 1
          maxLength: 40
          examples:
            - lego.com
        occurredOnTimestamp:
          type: string
          format: date-time
          description: 'The date and time of when experience was created. The format is in RFC 3339.'
          examples:
            - 2023-01-18T13:19:08.132084
        clientId:
          type: string
          description: The consent option URI
          examples:
            - self-consent://global/analytic-cookies
            - self-consent://global/necessary-cookies
            - self-consent://global/lego-marketing-cookies
  messages:
    UserConsentsObjectCreated:
      messageId: UserConsentsObjectCreated
      name: User consents object created
      title: The object state of consents for a user
      description: An event emitted whenever a user's consent is created.
      tags:
        - name: user
        - name: consents
        - name: created
      payload:
        $ref: '#/components/schemas/EnvelopeOfUserConsentsObjectCreated'
    UserConsentsObjectChanged:
      messageId: UserConsentsObjectChanged
      name: User consents object changed
      title: The object state of consents for a user
      description: An event emitted whenever a user's consent is changed.
      tags:
        - name: user
        - name: consents
        - name: changed
      payload:
        $ref: '#/components/schemas/EnvelopeOfUserConsentsObjectChanged'
    UserConsentsObjectDeleted:
      messageId: UserConsentsObjectDeleted
      name: User consents object deleted
      title: The object state of consents for a user
      description: An event emitted whenever a user's consent is deleted.
      tags:
        - name: user
        - name: consents
        - name: deleted
      payload:
        $ref: '#/components/schemas/EnvelopeOfUserConsentsObjectDeleted'
    UserConsentFieldCreated:
      messageId: UserConsentFieldCreated
      name: User consent field created
      title: The change of 1 specific consent being created
      description: An event emitted whenever a user's consent is created.
      tags:
        - name: user
        - name: consents
        - name: created
      payload:
        $ref: '#/components/schemas/EnvelopeOfUserConsentFieldCreatedEvent'
    UserConsentFieldChanged:
      messageId: UserConsentFieldChanged
      name: User consent field changed
      title: The change of 1 specific consent being changed
      description: An event emitted whenever a user's consent is changed.
      tags:
        - name: user
        - name: consents
        - name: changed
      payload:
        $ref: '#/components/schemas/EnvelopeOfUserConsentFieldChangedEvent'
    UserConsentFieldDeleted:
      messageId: UserConsentFieldDeleted
      name: User consent field deleted
      title: The change of 1 specific consent being deleted
      description: An event emitted whenever a user's consent is deleted.
      tags:
        - name: user
        - name: consents
        - name: deleted
      payload:
        $ref: '#/components/schemas/EnvelopeOfUserConsentFieldDeletedEvent'
    AnonymousConsentFieldChange:
      messageId: AnonymousConsentFieldChange
      name: Anonymous consent field change
      title: Anonymous consent field change event
      description: An event emitted whenever an anonymous consent is changed (added, updated or removed).
      tags:
        - name: anonymous
        - name: consents
        - name: deleted
      payload:
        $ref: '#/components/schemas/EnvelopeOfAnonymousConsentFieldChangeEvent'
    ExperienceCreated:
      messageId: ExperienceCreated
      name: Experience created
      title: Experience created event
      description: An event emitted whenever an experience is created.
      tags:
        - name: experience
        - name: created
      payload:
        $ref: '#/components/schemas/EnvelopeOfExperienceCreatedEvent'
    ExperienceDeleted:
      messageId: ExperienceDeleted
      name: Experience deleted
      title: Experience deleted event
      description: An event emitted whenever an experience is deleted.
      tags:
        - name: experience
        - name: deleted
      payload:
        $ref: '#/components/schemas/EnvelopeOfExperienceDeletedEvent'
    ExperienceUpdated:
      messageId: ExperienceUpdated
      name: Experience updated
      title: Experience updated event
      description: An event emitted whenever an experience is updated.
      tags:
        - name: experience
        - name: updated
      payload:
        $ref: '#/components/schemas/EnvelopeOfExperienceUpdatedEvent'
    ExperienceClientAdded:
      messageId: ExperienceClientAdded
      name: Experience client added
      title: Experience client added event
      description: An event emitted whenever a client is added to experience.
      tags:
        - name: experience
        - name: updated
      payload:
        $ref: '#/components/schemas/EnvelopeOfExperienceClientAddedEvent'
    ExperienceClientRemoved:
      messageId: ExperienceClientRemoved
      name: Experience client removed
      title: Experience client removed event
      description: An event emitted whenever a client is removed from experience.
      tags:
        - name: experience
        - name: updated
      payload:
        $ref: '#/components/schemas/EnvelopeOfExperienceClientRemovedEvent'
    ExperienceConsentOptionAdded:
      messageId: ExperienceConsentOptionAdded
      name: Experience consent option added
      title: Experience consent option added event
      description: An event emitted whenever an consent option is added to experience.
      tags:
        - name: experience
        - name: updated
      payload:
        $ref: '#/components/schemas/EnvelopeOfExperienceConsentOptionAddedEvent'
    ExperienceConsentOptionRemoved:
      messageId: ExperienceConsentOptionRemoved
      name: Experience consent option removed
      title: Experience consent option removed event
      description: An event emitted whenever an consent option is removed from experience.
      tags:
        - name: experience
        - name: updated
      payload:
        $ref: '#/components/schemas/EnvelopeOfExperienceConsentOptionRemovedEvent'
";

            var reader = new AsyncApiStringReader();
            var deserialized = reader.Read(spec, out var diagnostic);
        }

        [Test]
        public void Serialize_WithBindingReferences_SerializesDeserializes()
        {
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
                Bindings = new AsyncApiBindings<IServerBinding>()
                {
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.ServerBindings,
                        Id = "bindings",
                    },
                },
            });
            doc.Components = new AsyncApiComponents()
            {
                Channels = new Dictionary<string, AsyncApiChannel>()
                {
                    { "otherchannel", new AsyncApiChannel()
                        {
                            Publish = new AsyncApiOperation()
                            {
                                Description = "test",
                            },
                            Bindings = new AsyncApiBindings<IChannelBinding>()
                            {
                                Reference = new AsyncApiReference()
                                {
                                    Type = ReferenceType.ChannelBindings,
                                    Id = "bindings",
                                },
                            },
                        } 
                    }
                },
                ServerBindings = new Dictionary<string, AsyncApiBindings<IServerBinding>>()
                {
                    {
                        "bindings", new AsyncApiBindings<IServerBinding>()
                        {
                            new PulsarServerBinding()
                            {
                                Tenant = "staging"
                            },
                        }
                    }
                },
                ChannelBindings = new Dictionary<string, AsyncApiBindings<IChannelBinding>>()
                {
                    {
                        "bindings", new AsyncApiBindings<IChannelBinding>()
                        {
                            new PulsarChannelBinding()
                            {
                                Namespace = "users", 
                                Persistence = AsyncAPI.Models.Bindings.Pulsar.Persistence.Persistent,
                            }
                        }
                    }
                },
            };
            doc.Channels.Add("testChannel",
                new AsyncApiChannel
                {
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.Channel,
                        Id = "otherchannel"
                    }
                });
            var actual = doc.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);

            var settings = new AsyncApiReaderSettings();
            settings.Bindings.AddRange(BindingsCollection.Pulsar);
            var reader = new AsyncApiStringReader(settings);
            var deserialized = reader.Read(actual, out var diagnostic);
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

            var settings = new AsyncApiReaderSettings();
            settings.Bindings.AddRange(BindingsCollection.All);
            var reader = new AsyncApiStringReader(settings);
            var deserialized = reader.Read(actual, out var diagnostic);

            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(2, deserialized.Channels.First().Value.Publish.Message.First().Bindings.Count);

            var binding = deserialized.Channels.First().Value.Publish.Message.First().Bindings.First();
            Assert.AreEqual("http", binding.Key);
            var httpBinding = binding.Value as HttpMessageBinding;

            Assert.AreEqual("this mah binding", httpBinding.Headers.Description);
        }
    }
}
