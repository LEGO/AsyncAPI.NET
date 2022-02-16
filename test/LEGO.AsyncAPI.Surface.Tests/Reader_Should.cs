namespace LEGO.AsyncAPI.Surface.Tests
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Moq;
    using Newtonsoft.Json.Linq;
    using Stubs;
    using Xunit;

    public class Reader_Should : IDisposable
    {
        private readonly Reader _sut;
        private readonly Mock<IAsyncApiSchemaValidator> _asyncApiSchemaValidator = new();
        private readonly Mock<IAsyncApiReader<AsyncApiDocument>> _asyncApiReader = new();
        private readonly string _documentId;

        private Stream _stream;
        private StreamWriter _streamWriter;

        private const string ValidJson = "{\"xyz\":\"xyz_value\"}";
        private const string InvalidJson = "{\"xyz\":\"xyz_value\"";

        public Reader_Should()
        {
            _asyncApiSchemaValidator.Setup(validator =>
                validator.ValidateAsync(It.IsAny<JObject>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => new ValidatorResult { IsValid = true });

            _documentId = Guid.NewGuid().ToString();

            _sut = new Reader(_asyncApiSchemaValidator.Object, _asyncApiReader.Object);
        }

        public void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Dispose();
            }

            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        [Fact]
        public async Task Read_JObject_InvalidValidation_ThenResultWithValidationError()
        {
            //Arrange
            var jObject = JObject.Parse(ValidJson);
            var cancellationToken = new CancellationToken();

            WithInvalidValidatorResult();

            //Act
            var result = await _sut.ReadAsync(jObject, cancellationToken);

            //Assert
            Assert.True(result.HasValidationError);
        }

        [Fact]
        public async Task Read_JObject_AsyncApiReaderThrows_ThenResultWithError()
        {
            //Arrange
            var jObject = JObject.Parse(ValidJson);
            var cancellationToken = new CancellationToken();
            var error = new Exception("something is broken");

            _asyncApiReader.Setup(reader => reader.Consume(It.IsAny<JObject>())).Throws(error);

            //Act
            var result = await _sut.ReadAsync(jObject, cancellationToken);

            //Assert
            Assert.True(result.HasError);
            Assert.False(result.HasValidationError);
            Assert.Equal(error, result.DiagnosticObject.Error);
        }

        [Fact]
        public async Task Read_JObject_ThenResultWithNoErrors()
        {
            //Arrange
            var jObject = JObject.Parse(ValidJson);
            var cancellationToken = new CancellationToken();

            WithReturningAsyncApiDocument();

            //Act
            var result = await _sut.ReadAsync(jObject, cancellationToken);

            //Assert
            Assert.False(result.HasError);
            Assert.Equal(_documentId, result.Document.Id);
        }

        [Fact]
        public async Task Read_String_NotValidJson_ThenResultWithError()
        {
            //Arrange
            var cancellationToken = new CancellationToken();

            //Act
            var result = await _sut.ReadAsync(InvalidJson, cancellationToken);

            //Assert
            Assert.True(result.HasError);
        }

        [Fact]
        public async Task Read_String_InvalidValidation_ThenResultWithValidationError()
        {
            //Arrange
            var cancellationToken = new CancellationToken();

            WithInvalidValidatorResult();

            //Act
            var result = await _sut.ReadAsync(ValidJson, cancellationToken);

            //Assert
            Assert.True(result.HasValidationError);
        }

        [Fact]
        public async Task Read_String_AsyncApiReaderThrows_ThenResultWithError()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var error = new Exception("something is broken");

            _asyncApiReader.Setup(reader => reader.Consume(It.IsAny<JObject>())).Throws(error);

            //Act
            var result = await _sut.ReadAsync(ValidJson, cancellationToken);

            //Assert
            Assert.True(result.HasError);
            Assert.False(result.HasValidationError);
            Assert.Equal(error, result.DiagnosticObject.Error);
        }

        [Fact]
        public async Task Read_String_ThenResultWithNoErrors()
        {
            //Arrange
            var cancellationToken = new CancellationToken();

            WithReturningAsyncApiDocument();

            //Act
            var result = await _sut.ReadAsync(ValidJson, cancellationToken);

            //Assert
            Assert.False(result.HasError);
            Assert.Equal(_documentId, result.Document.Id);
        }

        [Fact]
        public async Task Read_Stream_NotValidJson_ThenResultWithError()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            _stream = GetStreamWithJson(InvalidJson);

            //Act
            var result = await _sut.ReadAsync(_stream, cancellationToken);

            //Assert
            Assert.True(result.HasError);
        }

        [Fact]
        public async Task Read_Stream_InvalidValidation_ThenResultWithValidationError()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            _stream = GetStreamWithJson(ValidJson);
            
            WithInvalidValidatorResult();

            //Act
            var result = await _sut.ReadAsync(_stream, cancellationToken);

            //Assert
            Assert.True(result.HasValidationError);
        }

        [Fact]
        public async Task Read_Stream_AsyncApiReaderThrows_ThenResultWithError()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            _stream = GetStreamWithJson(ValidJson);
            var error = new Exception("something is broken");

            _asyncApiReader.Setup(reader => reader.Consume(It.IsAny<JObject>())).Throws(error);

            //Act
            var result = await _sut.ReadAsync(_stream, cancellationToken);

            //Assert
            Assert.True(result.HasError);
            Assert.False(result.HasValidationError);
            Assert.Equal(error, result.DiagnosticObject.Error);
        }

        [Fact]
        public async Task Read_Stream_ThenResultWithNoErrors()
        {
            //Arrange
            var cancellationToken = new CancellationToken();

            WithReturningAsyncApiDocument();

            //Act
            var result = await _sut.ReadAsync(ValidJson, cancellationToken);

            //Assert
            Assert.False(result.HasError);
            Assert.Equal(_documentId, result.Document.Id);
        }

        [Fact]
        public async Task Read_JsonDocument_InvalidValidation_ThenResultWithValidationError()
        {
            //Arrange
            var cancellationToken = new CancellationToken();

            var jsonDocument = JsonDocument.Parse(ValidJson);

            WithInvalidValidatorResult();

            //Act
            var result = await _sut.ReadAsync(jsonDocument, cancellationToken);

            //Assert
            Assert.True(result.HasValidationError);
        }

        [Fact]
        public async Task Read_JsonDocument_AsyncApiReaderThrows_ThenResultWithError()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var jsonDocument = JsonDocument.Parse(ValidJson);
            var error = new Exception("something is broken");

            _asyncApiReader.Setup(reader => reader.Consume(It.IsAny<JObject>())).Throws(error);

            //Act
            var result = await _sut.ReadAsync(jsonDocument, cancellationToken);

            //Assert
            Assert.True(result.HasError);
            Assert.False(result.HasValidationError);
            Assert.Equal(error, result.DiagnosticObject.Error);
        }

        [Fact]
        public async Task Read_JsonDocument_ThenResultWithNoErrors()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var jsonDocument = JsonDocument.Parse(ValidJson);
            
            WithReturningAsyncApiDocument();

            //Act
            var result = await _sut.ReadAsync(jsonDocument, cancellationToken);

            //Assert
            Assert.False(result.HasError);
            Assert.Equal(_documentId, result.Document.Id);
        }

        private void WithInvalidValidatorResult()
        {
            _asyncApiSchemaValidator.Setup(validator =>
                    validator.ValidateAsync(It.IsAny<JObject>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => new ValidatorResult { IsValid = false });
        }

        private void WithReturningAsyncApiDocument()
        {
            _asyncApiReader.Setup(reader => reader.Consume(It.IsAny<JObject>())).Returns(() => new AsyncApiDocument { Id = _documentId });
        }

        private Stream GetStreamWithJson(string json)
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            streamWriter.Write(json);
            streamWriter.Flush();
            stream.Position = 0;
            _streamWriter = streamWriter;
            return stream;
        }
    }
}
