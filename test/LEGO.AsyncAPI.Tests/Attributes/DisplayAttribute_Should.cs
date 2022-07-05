using System;
using LEGO.AsyncAPI.Attributes;
using Xunit;

namespace LEGO.AsyncAPI.Tests.Attributes
{
    public class DisplayAttribute_Should
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithNullOrWhitespace_Throw(string name)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => { new DisplayAttribute(name); });
        }
    }
}
