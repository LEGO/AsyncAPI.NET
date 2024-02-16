## [5.1.1](https://github.com/LEGO/AsyncAPI.NET/compare/v5.1.0...v5.1.1) (2024-02-16)


### Bug Fixes

* long values for missing retention properties ([93ba475](https://github.com/LEGO/AsyncAPI.NET/commit/93ba4755babd05a0d21f3530aab417eeed3b7073))

# [5.1.0](https://github.com/LEGO/AsyncAPI.NET/compare/v5.0.0...v5.1.0) (2024-02-15)


### Bug Fixes

* updated topic configuration data types ([eace86d](https://github.com/LEGO/AsyncAPI.NET/commit/eace86dde4fc704d4652d19e7073be3b37ade6c7))


### Features

* added new topic configuration properties ([4a6c6a8](https://github.com/LEGO/AsyncAPI.NET/commit/4a6c6a8fed3a970153bd511daad5e614dbcdf2df))

# [5.0.0](https://github.com/LEGO/AsyncAPI.NET/compare/v4.1.0...v5.0.0) (2023-12-14)


### Bug Fixes

* add type to references, always. ([#139](https://github.com/LEGO/AsyncAPI.NET/issues/139)) ([3031023](https://github.com/LEGO/AsyncAPI.NET/commit/30310232bb3869258486d9f7f85721d4e3fb46eb))
* added missing mapping for ordering ([#138](https://github.com/LEGO/AsyncAPI.NET/issues/138)) ([510426e](https://github.com/LEGO/AsyncAPI.NET/commit/510426e200b4fe97ad1b8e9a6e94a615593c2a3c))
* patternProperties should also be walked as a reference ([#133](https://github.com/LEGO/AsyncAPI.NET/issues/133)) ([dc544f1](https://github.com/LEGO/AsyncAPI.NET/commit/dc544f1c01be3b95ded08ee894453ce8529eafb3))


* chore(settings)!: make reader bindings IEnumerable to allow for simpler usage ([e1f8c87](https://github.com/LEGO/AsyncAPI.NET/commit/e1f8c8766767ce642546a911810064a5234f04c3))


### Features

* allow non-component references ([#132](https://github.com/LEGO/AsyncAPI.NET/issues/132)) ([71fe571](https://github.com/LEGO/AsyncAPI.NET/commit/71fe571a3db0b4fbc13f4573b4d4b53f4f6b0911))
* **bindings:** add high throughput fifo properties ([#135](https://github.com/LEGO/AsyncAPI.NET/issues/135)) ([44ffcf4](https://github.com/LEGO/AsyncAPI.NET/commit/44ffcf4ceaf06a5168597e1eeb9407f09d47ab23))


### BREAKING CHANGES

* changes how bindings are applied.

# [4.1.0](https://github.com/LEGO/AsyncAPI.NET/compare/v4.0.2...v4.1.0) (2023-09-27)


### Features

* **bindings:** update FilterPolicy to match AWS API ([#128](https://github.com/LEGO/AsyncAPI.NET/issues/128)) ([5b64654](https://github.com/LEGO/AsyncAPI.NET/commit/5b6465474ae09d42a27377bf04d58fdbd1dd8a59))

## [4.0.2](https://github.com/LEGO/AsyncAPI.NET/compare/v4.0.1...v4.0.2) (2023-08-01)


### Bug Fixes

* add missing properties to json schema ([#124](https://github.com/LEGO/AsyncAPI.NET/issues/124)) ([adcd017](https://github.com/LEGO/AsyncAPI.NET/commit/adcd017b3ff6875eddac9649c2c95c398e49dec0))
* nullref if type is not set on jsonschema when using enum. ([#123](https://github.com/LEGO/AsyncAPI.NET/issues/123)) ([e53db72](https://github.com/LEGO/AsyncAPI.NET/commit/e53db729813bd76c17a335baf9bf0d0efc34e0bc))
* parse const keyword in a schema object ([#121](https://github.com/LEGO/AsyncAPI.NET/issues/121)) ([22b329c](https://github.com/LEGO/AsyncAPI.NET/commit/22b329c6c8068e4ff2090cb6dd11bab2d5a254a5))

## [4.0.1](https://github.com/LEGO/AsyncAPI.NET/compare/v4.0.0...v4.0.1) (2023-07-11)


### Bug Fixes

* add ability to have 'false' as the value for 'additionalproperties' ([#118](https://github.com/LEGO/AsyncAPI.NET/issues/118)) ([9e4867f](https://github.com/LEGO/AsyncAPI.NET/commit/9e4867fbec9377964489e53c71f38a239e359cdf))
* async schema deserializer "additionalProperties" not deserializing JsonSchema correctly ([#120](https://github.com/LEGO/AsyncAPI.NET/issues/120)) ([3761f52](https://github.com/LEGO/AsyncAPI.NET/commit/3761f521570268febb8b00fde9896379acb7047b))

# [4.0.0](https://github.com/LEGO/AsyncAPI.NET/compare/v3.0.2...v4.0.0) (2023-06-12)


### Bug Fixes

* add setter to BindingParsers collection ([211646e](https://github.com/LEGO/AsyncAPI.NET/commit/211646e95b82b3e32563fe75c57656cd6882267b))


### Features

* **jsonschema**!: type as flag rather than list (#115) ([d44efb0](https://github.com/LEGO/AsyncAPI.NET/commit/d44efb048402c70377064b87bd962b0e455e08b3)), closes [#115](https://github.com/LEGO/AsyncAPI.NET/issues/115)
* **jsonschema**!: changed out decimal for double to allow for bigger numbers ([ab00976](https://github.com/LEGO/AsyncAPI.NET/commit/ab009764a916171c8926c129384ce18b3162e71e))
* **bindings**!: separate bindings and allow for custom bindings. (#107) ([d38c33f](https://github.com/LEGO/AsyncAPI.NET/commit/d38c33f14d6de73e2563e29534965b06d423edac)), closes [#107](https://github.com/LEGO/AsyncAPI.NET/issues/107)
* **bindings:** add SNS AWS bindings ([#108](https://github.com/LEGO/AsyncAPI.NET/issues/108)) ([d48f166](https://github.com/LEGO/AsyncAPI.NET/commit/d48f1669ebfd9ad3f661b2b5928df1d622a4e7ba))
* **bindings:** add SQS AWS Bindings ([#113](https://github.com/LEGO/AsyncAPI.NET/issues/113)) ([4a93c7a](https://github.com/LEGO/AsyncAPI.NET/commit/4a93c7a26dbc0dd28914ac96575070deb0a6d2c1))


### BREAKING CHANGES

* The type of `Type` in JsonSchema is now a Flags enum, rather than a List of enum. This provides an easier to use interface, for adding and checking types.
* 3 properties, previously of type `decimal` in the JsonSchema type have been changed to `double`.
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
