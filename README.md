# Cake.Slack
Cake AddIn that extends Cake with Slack messaging features
[![Build status](https://ci.appveyor.com/api/projects/status/1tbi1x5b3i7wktv6?svg=true)](https://ci.appveyor.com/project/WCOMAB/cake-slack)

## Usage

```csharp
#addin "Cake.Slack"
var slackToken = EnvironmentVariable("SLACK_TOKEN");
var slackChannel = "#cake";
var postMessageResult = SlackChatPostMessage(
            token:slackToken,
            channel:slackChannel,
            text:"This _is_ a `message` from *CakeBuild* :thumbsup:\r\n```Here is some code```"
    );

if (postMessageResult.Ok)
{
    Information("Message {0} succcessfully sent", postMessageResult.TimeStamp);
}
else
{
    Error("Failed to send message: {0}", postMessageResult.Error);
}
```
Cake output will be similar to below:
```
Message 1420896696.000057 succcessfully sent
``` 
This will result in an message in your Slack channel similar to below:
![Sample message](https://github.com/WCOMAB/Cake.Slack/raw/master/samplemessage.png)