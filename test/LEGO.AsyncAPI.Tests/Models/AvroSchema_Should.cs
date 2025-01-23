﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    public class AvroSchema_Should
    {
        [Test]
        public void Serialize_WithDefaultNull_SetJsonNull()
        {
            var input = """
            type: record
            name: User
            namespace: Producer
            doc: ESP Schema validation test
            fields:
              - name: userId
                type: int
              - name: userEmail
                type:
                  - null
                  - string
                default: null
            """;

            var model = new AsyncApiStringReader().ReadFragment<AvroSchema>(input, AsyncApiVersion.AsyncApi2_0, out var diag);
            var reserialized = model.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);
            reserialized.Should().Contain("default\": null");
        }

        [Test]
        public void Deserialize_WithMetadata_CreatesMetadata()
        {
            var input =
                """
                {
                  "type": "record",
                  "name": "SomeEvent",
                  "namespace": "my.namspace.for.event",
                  "fields": [
                    {
                      "name": "countryCode",
                      "type": "string",
                      "doc": "Country of the partner, (e.g. DE)"
                    },
                    {
                      "name": "occurredOn",
                      "type": "string",
                      "doc": "Timestamp of when action occurred."
                    },
                    {
                      "name": "partnerId",
                      "type": "string",
                      "doc": "Id of the partner"
                    },
                    {
                      "name": "platformSource",
                      "type": "string",
                      "doc": "Platform source"
                    }
                  ],
                  "example": {
                    "occurredOn": "2023-11-03T09:56.582+00:00",
                    "partnerId": "1",
                    "platformSource": "Brecht",
                    "countryCode": "DE"
                  }
                }
                """;
            var model = new AsyncApiStringReader().ReadFragment<AvroSchema>(input, AsyncApiVersion.AsyncApi2_0, out var diag);
            model.Metadata.Should().HaveCount(1);
            var reserialized = model.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);

            // Assert
            input.Should()
                  .BePlatformAgnosticEquivalentTo(reserialized);

        }

        [Test]
        public void SerializeV2_SerializesCorrectly()
        {
            // Arrange
            var expected = """
            type: record
            name: User
            namespace: com.example
            fields:
              - name: username
                type: string
                doc: The username of the user.
                default: guest
                order: ascending
              - name: status
                type:
                  type: enum
                  name: Status
                  symbols:
                    - ACTIVE
                    - INACTIVE
                    - BANNED
                doc: The status of the user.
              - name: emails
                type:
                  type: array
                  items: string
                doc: A list of email addresses.
              - name: metadata
                type:
                  type: map
                  values: string
                doc: Metadata associated with the user.
              - name: address
                type:
                  type: record
                  name: Address
                  fields:
                    - name: street
                      type: string
                    - name: city
                      type: string
                    - name: zipcode
                      type: string
                doc: The address of the user.
              - name: profilePicture
                type:
                  type: fixed
                  name: ProfilePicture
                  size: 256
                doc: A fixed-size profile picture.
              - name: contact
                type:
                  - 'null'
                  - type: record
                    name: PhoneNumber
                    fields:
                      - name: countryCode
                        type: int
                      - name: number
                        type: string
                doc: 'The contact information of the user, which can be either null or a phone number.'
            """;

            var schema = new AvroRecord
            {
                Name = "User",
                Namespace = "com.example",
                Fields = new List<AvroField>
                {
                    new AvroField
                    {
                        Name = "username",
                        Type = AvroPrimitiveType.String,
                        Doc = "The username of the user.",
                        Default = new AsyncApiAny("guest"),
                        Order = AvroFieldOrder.Ascending,
                    },
                    new AvroField
                    {
                        Name = "status",
                        Type = new AvroEnum
                        {
                            Name = "Status",
                            Symbols = new List<string> { "ACTIVE", "INACTIVE", "BANNED" },
                        },
                        Doc = "The status of the user.",
                    },
                    new AvroField
                    {
                        Name = "emails",
                        Type = new AvroArray
                        {
                            Items = AvroPrimitiveType.String,
                        },
                        Doc = "A list of email addresses.",
                    },
                    new AvroField
                    {
                        Name = "metadata",
                        Type = new AvroMap
                        {
                            Values = AvroPrimitiveType.String,
                        },
                        Doc = "Metadata associated with the user.",
                    },
                    new AvroField
                    {
                        Name = "address",
                        Type = new AvroRecord
                        {
                            Name = "Address",
                            Fields = new List<AvroField>
                            {
                                new AvroField { Name = "street", Type = AvroPrimitiveType.String },
                                new AvroField { Name = "city", Type = AvroPrimitiveType.String },
                                new AvroField { Name = "zipcode", Type = AvroPrimitiveType.String },
                            },
                        },
                        Doc = "The address of the user.",
                    },
                    new AvroField
                    {
                        Name = "profilePicture",
                        Type = new AvroFixed
                        {
                            Name = "ProfilePicture",
                            Size = 256,
                        },
                        Doc = "A fixed-size profile picture.",
                    },
                    new AvroField
                    {
                        Name = "contact",
                        Type = new AvroUnion
                        {
                            Types = new List<AvroSchema>
                            {
                                AvroPrimitiveType.Null,
                                new AvroRecord
                                {
                                    Name = "PhoneNumber",
                                    Fields = new List<AvroField>
                                    {
                                        new AvroField { Name = "countryCode", Type = AvroPrimitiveType.Int },
                                        new AvroField { Name = "number", Type = AvroPrimitiveType.String },
                                    },
                                },
                            },
                        },
                        Doc = "The contact information of the user, which can be either null or a phone number.",
                    },
                },
            };

            // Act
            var actual = schema.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }

        [Test]
        public void SerializeV2_WithLogicalTypes_SerializesCorrectly()
        {
            // Arrange
            var input = """
                        {
                          "type": "array",
                          "items": [
                            {
                              "type": "bytes",
                              "logicalType": "decimal",
                              "scale": 2,
                              "precision": 4
                            },
                            {
                              "type": "string",
                              "logicalType": "uuid"
                            },
                            {
                              "type": "int",
                              "logicalType": "date"
                            },
                            {
                              "type": "int",
                              "logicalType": "time-millis"
                            },
                            {
                              "type": "long",
                              "logicalType": "time-micros"
                            },
                            {
                              "type": "long",
                              "logicalType": "timestamp-millis"
                            },
                            {
                              "type": "long",
                              "logicalType": "timestamp-micros"
                            },
                            {
                              "type": "fixed",
                              "logicalType": "duration",
                              "name": "Duration",
                              "size": 12
                            }
                          ]
                        }
                        """;

            // Act
            var model = new AsyncApiStringReader().ReadFragment<AvroSchema>(input, AsyncApiVersion.AsyncApi2_0, out var diag);
            var serialized = model.SerializeAsJson(AsyncApiVersion.AsyncApi2_0);
            // Assert
            model.As<AvroArray>().Items.As<AvroUnion>().Types.Should().HaveCount(8);

            serialized.Should().BePlatformAgnosticEquivalentTo(input);
        }

        [Test]
        public void ReadFragment_DeserializesCorrectly()
        {
            // Arrange
            var input = """
            type: record
            name: User
            namespace: com.example
            fields:
              - name: username
                type: string
                doc: The username of the user.
                default: guest
                order: ascending
              - name: status
                type:
                  type: enum
                  name: Status
                  symbols:
                    - ACTIVE
                    - INACTIVE
                    - BANNED
                doc: The status of the user.
              - name: emails
                type:
                  type: array
                  items: string
                doc: A list of email addresses.
              - name: metadata
                type:
                  type: map
                  values: string
                doc: Metadata associated with the user.
              - name: address
                type:
                  type: record
                  name: Address
                  fields:
                    - name: street
                      type: string
                    - name: city
                      type: string
                    - name: zipcode
                      type: string
                doc: The address of the user.
              - name: profilePicture
                type:
                  type: fixed
                  name: ProfilePicture
                  size: 256
                doc: A fixed-size profile picture.
              - name: contact
                type:
                  - 'null'
                  - type: record
                    name: PhoneNumber
                    fields:
                      - name: countryCode
                        type: int
                      - name: number
                        type: string
                doc: 'The contact information of the user, which can be either null or a phone number.'
            """;

            var expected = new AvroRecord
            {
                Name = "User",
                Namespace = "com.example",
                Fields = new List<AvroField>
                {
                    new AvroField
                    {
                        Name = "username",
                        Type = AvroPrimitiveType.String,
                        Doc = "The username of the user.",
                        Default = new AsyncApiAny("guest"),
                        Order = AvroFieldOrder.Ascending,
                    },
                    new AvroField
                    {
                        Name = "status",
                        Type = new AvroEnum
                        {
                            Name = "Status",
                            Symbols = new List<string> { "ACTIVE", "INACTIVE", "BANNED" },
                        },
                        Doc = "The status of the user.",
                    },
                    new AvroField
                    {
                        Name = "emails",
                        Type = new AvroArray
                        {
                            Items = AvroPrimitiveType.String,
                        },
                        Doc = "A list of email addresses.",
                    },
                    new AvroField
                    {
                        Name = "metadata",
                        Type = new AvroMap
                        {
                            Values = AvroPrimitiveType.String,
                        },
                        Doc = "Metadata associated with the user.",
                    },
                    new AvroField
                    {
                        Name = "address",
                        Type = new AvroRecord
                        {
                            Name = "Address",
                            Fields = new List<AvroField>
                            {
                                new AvroField { Name = "street", Type = AvroPrimitiveType.String },
                                new AvroField { Name = "city", Type = AvroPrimitiveType.String },
                                new AvroField { Name = "zipcode", Type = AvroPrimitiveType.String },
                            },
                        },
                        Doc = "The address of the user.",
                    },
                    new AvroField
                    {
                        Name = "profilePicture",
                        Type = new AvroFixed
                        {
                            Name = "ProfilePicture",
                            Size = 256,
                        },
                        Doc = "A fixed-size profile picture.",
                    },
                    new AvroField
                    {
                        Name = "contact",
                        Type = new AvroUnion
                        {
                            Types = new List<AvroSchema>
                            {
                                AvroPrimitiveType.Null,
                                new AvroRecord
                                {
                                    Name = "PhoneNumber",
                                    Fields = new List<AvroField>
                                    {
                                        new AvroField { Name = "countryCode", Type = AvroPrimitiveType.Int },
                                        new AvroField { Name = "number", Type = AvroPrimitiveType.String },
                                    },
                                },
                            },
                        },
                        Doc = "The contact information of the user, which can be either null or a phone number.",
                    },
                },
            };

            // Act
            var actual = new AsyncApiStringReader().ReadFragment<AvroSchema>(input, AsyncApiVersion.AsyncApi2_0, out var diagnostic);

            // Assert
            actual.Should()
                  .BeEquivalentTo(expected);
        }
    }
}
