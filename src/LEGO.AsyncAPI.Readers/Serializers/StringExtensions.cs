namespace LEGO.AsyncAPI.Readers.Serializers
{
    using System.Text;

    internal static class StringExtensions
    {
        public static string RemoveNonAsciiSymbols(this string input)
        {
            return Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()),
                    Encoding.UTF8.GetBytes(input)));
        }
    }
}
