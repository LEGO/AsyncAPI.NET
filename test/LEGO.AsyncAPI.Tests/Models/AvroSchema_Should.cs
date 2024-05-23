﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System.Collections.Generic;
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using NUnit.Framework;

    public class AvroSchema_Should
    {
        [Test]
        public void SerializeV2_SerializesCorrectly()
        {
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

            var schema = new AvroSchema
            {
                Type = AvroSchemaType.Record,
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
                        Order = "ascending",
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
                            Types = new List<AvroFieldType>
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

            var actual = schema.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);

            // Assert
            actual.Should()
                  .BePlatformAgnosticEquivalentTo(expected);
        }
    }
}
