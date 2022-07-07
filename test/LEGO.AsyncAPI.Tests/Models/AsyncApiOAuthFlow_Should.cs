using System;
using LEGO.AsyncAPI.Models;
using Xunit;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiOAuthFlow_Should
    {
        [Fact]
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
