using System;
using LEGO.AsyncAPI.Models;
using Xunit;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiSecurityRequirement_Should
    {
        [Fact]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiInfo = new AsyncApiSecurityRequirement();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiInfo.SerializeV2(null); });
        }
    }
}