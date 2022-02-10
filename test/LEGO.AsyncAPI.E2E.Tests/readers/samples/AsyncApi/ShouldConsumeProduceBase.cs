using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LEGO.AsyncAPI.E2E.Tests.readers.samples.AsyncApi;

public class ShouldConsumeProduceBase<T>
{
    protected IAsyncApiWriter<T> _asyncApiWriter;
    protected IAsyncApiReader<T> _asyncApiAsyncApiReader;
    private readonly string _SampleFolderPath;

    protected ShouldConsumeProduceBase(Type child)
    {
        _SampleFolderPath = GetPath(child);
        _asyncApiAsyncApiReader = new AsyncApiAsyncApiReaderNewtonJson<T>();
        _asyncApiWriter = new AsyncApiWriterNewtonJson<T>();
    }

    protected Stream? GetStream(string filename)
    {
        var combine = Path.Combine(_SampleFolderPath, filename).Replace("/", ".");
        return GetType().Assembly.GetManifestResourceStream(combine);
    }

    protected string GetString(string filename)
    {
        var combine = Path.Combine(_SampleFolderPath, filename).Replace("/", ".");
        Stream? stream = GetType().Assembly.GetManifestResourceStream(combine);
        if (stream == null)
        {
            throw new FileNotFoundException("The stream is null because the file was not found");
        }

        return new StreamReader(stream).ReadToEnd();
    }
    
    private static string GetPath(Type child)
    {
        var split = Regex.Split(child.FullName, "(.*?)\\.Should.*?");
        return split[1];
    }
}