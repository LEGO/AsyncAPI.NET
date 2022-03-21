namespace LEGO.AsyncAPI.Tests.Models
{
    using LEGO.AsyncAPI.Models.Any;
    using Xunit;

    public class AsyncAPIString_Should
    {
        [Fact]
        public void HaveValueNull_WhenCreatedWithDefaultConstructor()
        {
            var sut = new AsyncAPIString();
            var value = sut.Value;
            Assert.Null(value);
        }

        [Fact]
        public void TriggerConditionalNullCheckResult_WhenCastFromNullObject()
        {
            object null_value = null;
            var value = ((AsyncAPIString)null_value)?.Value;
            Assert.Null(value);
        }

        [Fact]
        public void HaveNullValue_WhenCastFromNullString()
        {
            string null_value = null;
            var value = ((AsyncAPIString)null_value).Value;
            Assert.Null(value);
        }
    }
}
