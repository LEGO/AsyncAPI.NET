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
[![Nuget](https://img.shields.io/nuget/vpre/AsyncAPI.NET?label=AsyncAPI.NET-Preview&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET/)  

### AsyncAPI.Readers
[![Nuget](https://img.shields.io/nuget/v/AsyncAPI.NET.Readers?label=AsyncAPI.Readers&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET.Readers/)  
[![Nuget](https://img.shields.io/nuget/vpre/AsyncAPI.NET.Readers?label=AsyncAPI.Readers-Preview&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET.Readers/)  

### AsyncAPI.Bindings
[![Nuget](https://img.shields.io/nuget/v/AsyncAPI.NET.Bindings?label=AsyncAPI.Bindings&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET.Bindings/)  
[![Nuget](https://img.shields.io/nuget/vpre/AsyncAPI.NET.Bindings?label=AsyncAPI.Bindings-Preview&style=for-the-badge)](https://www.nuget.org/packages/AsyncAPI.NET.Bindings/)  
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
         Title = "my first asyncapi",
     },
     Channels = new Dictionary<string, AsyncApiChannel>
     {
         {
             "users", new AsyncApiChannel
             {
                 Subscribe = new AsyncApiOperation
                 {
                     OperationId = "users",
                     Description = "my users channel",
                     Message = new List<AsyncApiMessage>
                     {
                       new AsyncApiMessageReference("#/components/messages/MyMessage"),
                     },
                 },
             }
         },
     },
     Components = new AsyncApiComponents
     {
         Messages = new Dictionary<string, AsyncApiMessage>
         {
             {
                 "MyMessage", new AsyncApiMessage
                 {
                     Name = "Hello!",
                 }
             },
         },
     },
 };

var yaml = myFirstAsyncApi.SerializeAsYaml(AsyncApi);

//asyncapi: 2.6.0
//  info:
//    title: my first asyncapi
//channels:
//  users:
//    subscribe:
//      operationId: users
//      description: my users channel
//      message:
//        $ref: '#/components/messages/MyMessage'
//components:
//  messages:
//    MyMessage:
//      name: Hello!
```


### Reading

There are 3 reader types
1. AsyncApiStringReader
2. AsyncApiTextReader
3. AsyncApiStreamReader

All 3 supports both json and yaml.

#### StreamReader
```csharp
var httpClient = new HttpClient
{
  BaseAddress = new Uri("https://raw.githubusercontent.com/asyncapi/spec/"),
};

var stream = await httpClient.GetStreamAsync("master/examples/streetlights-kafka.yml");
var asyncApiDocument = new AsyncApiStreamReader().Read(stream, out var diagnostic);
```

#### StringReader
```csharp
var yaml =
	"""
	asyncapi: 2.6.0
	  info:
	    title: my first asyncapi
	channels:
	  users:
	    subscribe:
	      operationId: users
	      description: my users channel
	      message:
	        $ref: '#/components/messages/MyMessage'
	components:
	  messages:
	    MyMessage:
	      name: Hello!
	""";

var asyncApiDocument = new AsyncApiStringReader().Read(yaml, out var diagnostic);
```
All readers will write warnings and errors to the diagnostics.


### Reference Resolution
Internal references are resolved by default. This includes component and non-component references e.g `#/components/messages/MyMessage` and `#/servers/0`.  
External references can be resolved by setting `ReferenceResolution` to `ResolveAllReferences`.
The default implementation will resolve anything prefixed with `file://`, `http://` & `https://`, however a custom implementation can be made, by inhereting from the `IStreamLoader` interface and setting the `ExternalReferenceLoader` in the `AsyncApiReaderSettings`.
External references are always force converted to Json during resolution. This means that both yaml and json is supported - but not other serialization languages.

```csharp
var settings = new AsyncApiReaderSettings { ReferenceResolution = ReferenceResolution.ResolveAllReferences };
var document = new AsyncApiStringReader(settings).Read(json, out var diagnostics);
```



Reference resolution can be disabled by setting `ReferenceResolution` to `DoNotResolveReferences`. 

### Bindings
To add support for reading bindings, simply add the bindings you wish to support, to the `Bindings` collection of `AsyncApiReaderSettings`.
There is a nifty helper to add different types of bindings, or like in the example `All` of them.

```csharp
var settings = new AsyncApiReaderSettings();
settings.Bindings = BindingsCollection.All;
var asyncApiDocument = new AsyncApiStringReader(settings).Read(yaml, out var diagnostic);
```

## Attribution

* [OpenAPI.Net](https://github.com/microsoft/OpenAPI.NET) - [MIT License](https://github.com/microsoft/OpenAPI.NET/blob/vnext/LICENSE)
* [YamlDotNet](https://github.com/aaubry/YamlDotNet) - [MIT License](https://github.com/aaubry/YamlDotNet/blob/master/LICENSE.txt)

## Contribution

This project welcomes contributions and suggestions.
Do you want to contribute to the project? Find out how [here](CONTRIBUTING.md).

## License
[Modified Apache 2.0 (Section 6)](https://github.com/LEGO/AsyncAPI.NET/blob/main/LICENSE.txt)
