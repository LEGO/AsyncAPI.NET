![](docs/lego-async-mark.drawio.png)

# AsyncAPI.NET

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/LEGO/AsyncAPI.NET/ci.yml?label=Build%20%26%20Test&style=for-the-badge)  


The AsyncAPI.NET SDK contains a useful object model for the AsyncAPI specification in .NET along with common serializers to extract raw AsyncAPI JSON and YAML documents from the model as well.

[CHANGELOG](https://github.com/LEGO/AsyncAPI.NET/blob/main/CHANGELOG.md)  
[Wiki and getting started guide](https://github.com/LEGO/AsyncAPI.NET/wiki)

## Installation

Install the NuGet packages:
### AsyncAPI.NET
[![Nuget](https://img.shields.io/nuget/v/AsyncAPI.NET?label=AsyncAPI.NET&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET/)  
[![Nuget](https://img.shields.io/nuget/vpre/AsyncAPI.NET?label=AsyncAPI.NET&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET/)  

### AsyncAPI.NET.Readers
[![Nuget](https://img.shields.io/nuget/v/AsyncAPI.NET.Readers?label=AsyncAPI.NET.Readers&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET.Readers/)  
[![Nuget](https://img.shields.io/nuget/vpre/AsyncAPI.NET.Readers?label=AsyncAPI.NET.Readers&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET.Readers/)  

### AsyncAPI.NET.Bindings
[![Nuget](https://img.shields.io/nuget/v/AsyncAPI.NET.Bindings?label=AsyncAPI.NET.Bindings&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET.Bindings/)  

## Example Usage

Main classes to know:

* AsyncApiStringReader
* AsyncApiStringWriter
  * There is an extension on the AsyncApiDocument type which allows Serializing as well (`new AsyncApiDocument().SerializeAsJson()` or `new AsyncApiDocument().SerializeAsYaml()`

### Writing

```csharp
var myFirstAsyncApi = new AsyncApiDocument
{
  Info = new AsyncApiInfo
  {
    Title = "my first asyncapi"
  },
  Channels = new Dictionary<string, AsyncApiChannel>
  {
    {
	"users", new AsyncApiChannel
	{
	    Subscribe = new AsyncApiOperation
	    {
		OperationId = "users",
		Description = "my users channel"
	    }
	}
    }
  }
};
var yaml = myFirstAsyncApi.SerializeAsYaml();
//asyncapi: '2.5.0'
//  info:
//    title: my first asyncapi
//  channels:
//    users:
//      subscribe:
//        operationId: users
//        description: my users channel
```

### Reading

```csharp
var httpClient = new HttpClient
{
  BaseAddress = new Uri("https://raw.githubusercontent.com/asyncapi/spec/"),
};

var stream = await httpClient.GetStreamAsync("master/examples/streetlights-kafka.yml");
var asyncApiDocument = new AsyncApiStreamReader().Read(stream, out var diagnostic);
```

## Attribution

* [OpenAPI.Net](https://github.com/microsoft/OpenAPI.NET) - [MIT License](https://github.com/microsoft/OpenAPI.NET/blob/vnext/LICENSE)
* [YamlDotNet](https://github.com/aaubry/YamlDotNet) - [MIT License](https://github.com/aaubry/YamlDotNet/blob/master/LICENSE.txt)

## Contribution

This project welcomes contributions and suggestions.
Do you want to contribute to the project? Find out how [here](CONTRIBUTING.md).

## License
[Modified Apache 2.0 (Section 6)](https://github.com/LEGO/AsyncAPI.NET/blob/main/LICENSE)
