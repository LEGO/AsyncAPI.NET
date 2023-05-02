// Copyright (c) The LEGO Group. All rights reserved.

using System;
using LEGO.AsyncAPI.Models;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
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
