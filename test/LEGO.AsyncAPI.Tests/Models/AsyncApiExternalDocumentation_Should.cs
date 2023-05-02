// Copyright (c) The LEGO Group. All rights reserved.

using System;
using LEGO.AsyncAPI.Models;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiExternalDocumentation_Should
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiExternalDocumentation = new AsyncApiExternalDocumentation();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiExternalDocumentation.SerializeV2(null); });
        }
    }
}
