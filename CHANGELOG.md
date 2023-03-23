# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added 

- Add [moderation layer](https://platform.openai.com/docs/guides/chat/adding-a-moderation-layer)

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
