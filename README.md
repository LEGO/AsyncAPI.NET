![](docs/lego-async-mark.drawio.png)

# AsyncAPI.NET
[![Build & Test](https://github.com/LEGO/AsyncAPI.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/LEGO/AsyncAPI.NET/actions/workflows/ci.yml)

The AsyncAPI.NET SDK contains a useful object model for the AsyncAPI specification in .NET along with common serializers to extract raw AsyncAPI JSON and YAML documents from the model as well.

## Installation
Install the NuGet packages
* [Document](link missing)
* [Readers](link missing)

## Example Usage
Main classes to know
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
//asyncapi: '2.3.0'
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
* [SharpYaml](https://github.com/xoofx/SharpYaml) - [MIT License](https://github.com/xoofx/SharpYaml/blob/master/LICENSE.txt)

## Contribution
This project welcomes contributions and suggestions.
Do you want to contribute to the project? Find out how [here](CONTRIBUTION.md).


## License
AsyncAPI.Net is copyright LEGO Group.
