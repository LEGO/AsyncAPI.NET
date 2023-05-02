// Copyright (c) The LEGO Group. All rights reserved.

using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiSecurityRequirement_Should
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiSecurityRequirement = new AsyncApiSecurityRequirement();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiSecurityRequirement.SerializeV2(null); });
        }

        [Test]
        public void SerializeV2_Serializes()
        {
            var asyncApiSecurityRequirement = new AsyncApiSecurityRequirement();
            asyncApiSecurityRequirement.Add(new AsyncApiSecurityScheme { Type = SecuritySchemeType.ApiKey }, new List<string> { "string" });

            var output = asyncApiSecurityRequirement.SerializeAsYaml(AsyncApiVersion.AsyncApi2_0);
        }
    }
}
