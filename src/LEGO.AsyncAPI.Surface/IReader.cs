namespace LEGO.AsyncAPI.Surface;

public interface IReader
{
    Task<ReaderReadResult> ReadAsync(Stream stream, CancellationToken cancellationToken);
}