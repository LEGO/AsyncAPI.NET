using System;
using LEGO.AsyncAPI.Models;
using Xunit;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiExternalDocumentation_Should
    {
        [Fact]
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
