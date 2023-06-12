# [4.0.0](https://github.com/LEGO/AsyncAPI.NET/compare/v3.0.2...v4.0.0) (2023-06-12)


### Bug Fixes

* add setter to BindingParsers collection ([211646e](https://github.com/LEGO/AsyncAPI.NET/commit/211646e95b82b3e32563fe75c57656cd6882267b))


* feat(JsonSchema)!: type as flag rather than list (#115) ([d44efb0](https://github.com/LEGO/AsyncAPI.NET/commit/d44efb048402c70377064b87bd962b0e455e08b3)), closes [#115](https://github.com/LEGO/AsyncAPI.NET/issues/115)
* feat(JsonSchema)!: changed out decimal for double to allow for bigger numbers ([ab00976](https://github.com/LEGO/AsyncAPI.NET/commit/ab009764a916171c8926c129384ce18b3162e71e))
* feat(Bindings)!: separate bindings and allow for custom bindings. (#107) ([d38c33f](https://github.com/LEGO/AsyncAPI.NET/commit/d38c33f14d6de73e2563e29534965b06d423edac)), closes [#107](https://github.com/LEGO/AsyncAPI.NET/issues/107)


### Features

* **bindings:** add SNS AWS bindings ([#108](https://github.com/LEGO/AsyncAPI.NET/issues/108)) ([d48f166](https://github.com/LEGO/AsyncAPI.NET/commit/d48f1669ebfd9ad3f661b2b5928df1d622a4e7ba))
* **bindings:** add SQS AWS Bindings ([#113](https://github.com/LEGO/AsyncAPI.NET/issues/113)) ([4a93c7a](https://github.com/LEGO/AsyncAPI.NET/commit/4a93c7a26dbc0dd28914ac96575070deb0a6d2c1))


### BREAKING CHANGES

* this changes the type of Type in JsonSchema to be a Flags enum, rather than a List of enum.
* this changes the type of 3 properties of JsonSchema from `decimal` to `double`
* Bindings have been moved to a separate project

## [3.0.2](https://github.com/LEGO/AsyncAPI.NET/compare/v3.0.1...v3.0.2) (2023-03-30)


### Bug Fixes

* fixed typo in AsyncApiConstants.cs ([#101](https://github.com/LEGO/AsyncAPI.NET/issues/101)) ([146ee71](https://github.com/LEGO/AsyncAPI.NET/commit/146ee71082fb0eab4fc4231f3564bc8f7b73d779))

## [3.0.1](https://github.com/LEGO/AsyncAPI.NET/compare/v3.0.0...v3.0.1) (2023-03-13)


### Bug Fixes

* make optional properties of bindings nullable ([#89](https://github.com/LEGO/AsyncAPI.NET/issues/89)) ([499cf64](https://github.com/LEGO/AsyncAPI.NET/commit/499cf64a54fda10a7fc6f870d406e11b142faff4))

# [3.0.0](https://github.com/LEGO/AsyncAPI.NET/compare/v2.0.2...v3.0.0) (2023-02-27)


* feat!: add basic [REQUIRED] validation rules (#96) ([4e6fa07](https://github.com/LEGO/AsyncAPI.NET/commit/4e6fa070663e7c173ae7c731d327b9102fa67ba0)), closes [#96](https://github.com/LEGO/AsyncAPI.NET/issues/96)


### BREAKING CHANGES

* Adds validation and starts spitting out errors in diagnostics, that haven't been there before.
* Also changes SharpYaml to YamlDotNet, which does come with potental for breakage.
Therefor i declare this change as breaking.

## [2.0.2](https://github.com/LEGO/AsyncAPI.NET/compare/v2.0.1...v2.0.2) (2023-02-22)


### Bug Fixes

* payload schema reference resolution ([#94](https://github.com/LEGO/AsyncAPI.NET/issues/94)) ([70d4c23](https://github.com/LEGO/AsyncAPI.NET/commit/70d4c23634ae588da265fe79f5ae934d0bfe8c6a))

## [2.0.1](https://github.com/LEGO/AsyncAPI.NET/compare/v2.0.0...v2.0.1) (2023-02-08)


### Bug Fixes

* main project package id ([37ba867](https://github.com/LEGO/AsyncAPI.NET/commit/37ba8676eab24c35d2cdf8315381e96c60770221))

# [2.0.0](https://github.com/LEGO/AsyncAPI.NET/compare/v1.0.0...v2.0.0) (2023-02-08)


* fix!: rename to AsyncAPI.NET ([96cb30b](https://github.com/LEGO/AsyncAPI.NET/commit/96cb30b746f69600d8a21cdd263acb5a1251761d))


### Features

* upgrade to 2.6.0 ([#88](https://github.com/LEGO/AsyncAPI.NET/issues/88)) ([91a6184](https://github.com/LEGO/AsyncAPI.NET/commit/91a6184e314beb9a593d2bb7d397120722677348))


### BREAKING CHANGES

* as the LEGO prefix has been taken by others, the package has been renamed.


chore: update CHANGELOG.md
Update CHANGELOG.md

chore: update CHANGELOG.md
Update CHANGELOG.md

# 1.0.0 (2023-02-07)


### Bug Fixes

* **bindings:** remove pulsar binding ([#70](https://github.com/LEGO/AsyncAPI.NET/issues/70)) ([fd873d0](https://github.com/LEGO/AsyncAPI.NET/commit/fd873d049b8fa25adf67fb65bdba1e3b3882d3e7))
* change payload 'any' type to 'schema' to match specification. ([#77](https://github.com/LEGO/AsyncAPI.NET/issues/77)) ([3a7b96a](https://github.com/LEGO/AsyncAPI.NET/commit/3a7b96ad3c8d58462e13b4c0d161ecb81455e132))
* **readers:** set fixed payload schemaformats to json schema types ([#84](https://github.com/LEGO/AsyncAPI.NET/issues/84)) ([0f46359](https://github.com/LEGO/AsyncAPI.NET/commit/0f46359663d242d1ba0ec09378a355f4e38de4c4))
* **readers:** throw asyncapiexception, as this is handled by the main reader. ([#80](https://github.com/LEGO/AsyncAPI.NET/issues/80)) ([ef428b1](https://github.com/LEGO/AsyncAPI.NET/commit/ef428b1df7df80daf20772765345588827998b01))
* **writerextensions:** consistency ([#72](https://github.com/LEGO/AsyncAPI.NET/issues/72)) ([e86cf20](https://github.com/LEGO/AsyncAPI.NET/commit/e86cf203ac464fb7dc58184c253048f2814696be))


### Features

* **bindings:** add pulsar bindings ([#74](https://github.com/LEGO/AsyncAPI.NET/issues/74)) ([04e8af8](https://github.com/LEGO/AsyncAPI.NET/commit/04e8af8ffea3ef9520418169463590b222213d6a))
* **bindings:** add websockets binding ([#69](https://github.com/LEGO/AsyncAPI.NET/issues/69)) ([827f824](https://github.com/LEGO/AsyncAPI.NET/commit/827f82424679fef4be3547d3981427a333841d34))
* models and serialization should now be up to date with version 2.5 ([#76](https://github.com/LEGO/AsyncAPI.NET/issues/76)) ([cbf3253](https://github.com/LEGO/AsyncAPI.NET/commit/cbf325392b4b1fb726f72e6d2439f050b7138ef5))
