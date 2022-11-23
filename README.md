# AsyncAPI.NET
[![Build & Test](https://github.com/LEGO/AsyncAPI.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/LEGO/AsyncAPI.NET/actions/workflows/ci.yml)


Serialization and Deserialization of json/yaml AsyncAPI specifications into dotnet classes.


## Getting started

1. Read Async API specification [here](https://github.com/asyncapi/spec/blob/master/spec/asyncapi.md)
2. Explore [example projects](https://github.com/LEGO/AsyncAPI.NET/tree/main/examples). 
3. Add dependency to LEGO.AsyncAPI.Readers Nuget package in order to transform your Async API specs into AsyncApiDocument C# object.
	- Download Nuget.exe
	- Run	
	nuget.exe sources Add -Name "LEGO GitHub Package Registry" -Source https://nuget.pkg.github.com/lego/index.json -username {your lego email} -password {your Github PAT with packages-read permission and SSO enabled}
	- Add latest release version of package LEGO.AsyncAPI.Readers to your .Net project.
4. Contribute following the [contribution guide](CONTRIBUTION.md).


## Contribution

Do you want to contribute to the project? Find out how [here](CONTRIBUTION.md).

## License

AsyncAPI.Net is copyright LEGO Group.
