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
