# RememberMe

A Contact List for Social Media accounts that reminds the User if a Contact has not been contacted in a while  
Supports E-Mail, Discord, Telegram and generic Entries.

## Installation

- Just unpack the zip file and run the `RememberMe.exe` file

## Configuration

The Configuration is stored in the `appsettings.json` file which will be located in the same directory as the `RememberMe.exe` file

```json
{
  "Files": {
    "ConfigFilePath": "",
    "ConfigFileName": "data.json"
  },
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 7
        }
      }
    ],

    "Properties": {
      "Application": "Snoval.Dev.RememberMe"
    }
  }
}
```
- Files
  - ConfigFilePath: The path to the Config File  
  if empty, the default Path will be `%AppData%\snoval.dev.rememberme`  
  Be aware that Variables such as `%AppData%` are not supported in the `appsettings.json` File
  - ConfigFileName: The name of the Config File  
    if empty, the default Name will be `data.json`
- Serilog
  - Using: The Sinks to use
  - MinimumLevel: The minimum level of logging
  - WriteTo: The Sinks to write to
    - Name: The name of the Sink
    - Args: The arguments for the Sink
      - path: The path to the log file
      - rollingInterval: The interval to roll the log file
      - retainedFileCountLimit: The number of log files to keep
  - Properties: The Properties to include in the log

Refer to the [Serilog Documentation](https://serilog.net/) for more information


## Contributors

- [SkippyLynn](https://bsky.app/profile/skippylynn.bsky.social)
  - Created the Splash Screen