using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi
{
    public class ShouldConsumeProduceBase<T>
    {
        protected readonly IStringWriter<T> asyncApiWriter;
        protected readonly IStreamReader<T> asyncApiAsyncApiReader;
        private readonly string sampleFolderPath;

        protected ShouldConsumeProduceBase(Type child)
        {
            sampleFolderPath = GetPath(child);
            asyncApiAsyncApiReader = new JsonStreamReader<T>();
            asyncApiWriter = new JsonStringWriter<T>();
        }

        protected Stream GetStream(string filename)
        {
            return GenerateStreamFromString(GetString(filename));
        }
    
        protected Stream GetStreamWithMockedExtensions(string filename)
        {
            return GenerateStreamFromString(GetStringWithMockedExtensions(filename));
        }

        protected string GetString(string filename)
        {
            return ReadFile(sampleFolderPath, filename);
        }

        protected string GetStringWithMockedExtensions(string filename)
        {
            var body = GetString(filename);
            var extensionsBody = ReadFile("LEGO/AsyncAPI/E2E/Tests/readers/samples/AsyncApi", "MockExtensions.json");
            return new StringBuilder(body.Substring(0, body.Length - 2).TrimEnd('\r')).AppendLine(",").AppendLine(extensionsBody).Append("}").ToString();
        }

        private string ReadFile(string sampleFolderPath, string filename)
        {
            var combine = Path.Combine(sampleFolderPath, filename).Replace("/", ".").Replace("\\",".");
            var stream = GetType().Assembly.GetManifestResourceStream(combine);
            if (stream == null)
            {
                throw new FileNotFoundException("The stream is null because the file was not found");
            }

            return new StreamReader(stream).ReadToEnd();
        }

        private static string GetPath(Type child)
        {
            var split = Regex.Split(child.FullName ?? throw new ArgumentNullException(nameof(child)), "(.*?)\\.Should.*?");
            return split[1];
        }
    
        protected static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
