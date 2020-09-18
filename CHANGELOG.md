# 0.0.5

- Fixed
  - Text double escaping in logs
- Removed
  - Ability to \`steal\` the configuration from [conductor-dotnet-client](https://github.com/courosh12/conductor-dotnet-client)
- Changed
  - If an unknown log level is encountered instead of throwing a `ArgumentOutOfRangeException`, `??` is used as the log level.