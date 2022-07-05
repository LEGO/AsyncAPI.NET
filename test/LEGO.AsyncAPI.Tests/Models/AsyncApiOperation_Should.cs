using System;
using LEGO.AsyncAPI.Models;
using Xunit;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiOperation_Should
    {
        [Fact]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiInfo = new AsyncApiOperation();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiInfo.SerializeV2(null); });
        }
    }
}
