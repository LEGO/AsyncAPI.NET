// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using LEGO.AsyncAPI.Models;
    using NUnit.Framework;

    public class AsyncApiOAuthFlow_Should
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiOAuthFlow = new AsyncApiOAuthFlow();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiOAuthFlow.SerializeV2(null); });
        }
    }
}
