# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.7.1] - 2023-07-05

### Fixed

- Fix request to add `HttpCompletionOption.ResponseHeadersRead` to Relent streaming API.

## [0.7.0] - 2023-07-05

### Added

- Add Streaming API sample.

### Changed

- Change streaming API to use `IAsyncEnumerable`.
- Change to `UniTask` from `Task` in Relent API.

## [0.6.0] - 2023-06-14

### Added

- Add 16k context turbo model option.
- Add function calling support.
- Add optional `name` property to `Message`.
- Add verbose log option.

## Changed

- Update chat models at 2023-06-13.
- Add dependencies to `package.json`.

## [0.5.0] - 2023-04-08

### Changed

- Simplify package structure.
- Update Relent version to 0.2.0.

## [0.4.0] - 2023-03-25

### Added

- Add extensions of `IChatMemory`.

### Changed

- Improve interface of `IChatMemory` for async operation.
- Be `IChatMemory` implementations thread-safe.

### Fixed

- Fix capacity of `FiniteQueueChatMemory`.

## [0.3.1] - 2023-03-25

### Changed

- Improve accessibility of `Messaage`.

## [0.3.0] - 2023-03-23

### Added

- Add `TiktokenSharp` that is a tokenizer to calculate token length of text in local.

## [0.2.2] - 2023-03-21

### Added

- Add option to specify instance of `HttpClient`.
- Add option to receive response as `Stream`.

### Changed

- Improve null check of response content.

## [0.2.1] - 2023-03-20

### Added

- Add GPT-4 all models.

## [0.2.0] - 2023-03-20

### Added

- Add memory management of chat completion API by `IChatMemory`.
- Add [resilient error handling implementation](https://github.com/mochi-neko/ChatGPT-API-unity/blob/main/Assets/Mochineko/ChatGPT_API.Relent/RelentChatCompletionAPIConnection.cs) of chat completion API by [Relent](https://github.com/mochi-neko/Relent).
- Add GPT-4 model enum.

### Changed

- Change name and arguments of completion method.

### Fixed

- Fix deserialization error of `"usage"` in response body.

## [0.1.1] - 2023-03-06

### Added

- Add optional request parameters.

### Fixed

- Fix error handling of API response.

## [0.1.0] - 2023-03-04

### Added

- Implement ChatGPT chat completion API bindings to C#.
- Implement sample component of ChatGPT API.
