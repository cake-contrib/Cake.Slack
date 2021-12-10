# Cake.Slack

Cake AddIn that extends Cake with Slack messaging features
[![Build status](https://ci.appveyor.com/api/projects/status/43v2xctpy6gh2tvj/branch/develop?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-slack/branch/develop)

## Usage

### Post message

### Using token

```csharp
#addin "Cake.Slack"
var slackToken = EnvironmentVariable("SLACK_TOKEN");
var slackChannel = "#cake";
var postMessageResult = Slack.Chat.PostMessage(
            token:slackToken,
            channel:slackChannel,
            text:"This _is_ a `message` from *CakeBuild* :thumbsup:\r\n```Here is some code```"
    );

if (postMessageResult.Ok)
{
    Information("Message {0} successfully sent", postMessageResult.TimeStamp);
}
else
{
    Error("Failed to send message: {0}", postMessageResult.Error);
}
```
Cake output will be similar to below:
```
Message 1420896696.000057 successfully sent
```
This will result in an message in your Slack channel similar to below:

![Sample message](https://github.com/WCOMAB/Cake.Slack/raw/master/samplemessage.png)

### Using incoming web hook url

```csharp
#addin "Cake.Slack"
var slackhookuri = EnvironmentVariable("slackhookuri");
var slackChannel = "#cake";
var postMessageResult = Slack.Chat.PostMessage(
            channel:slackChannel,
            text:"This _is_ a `message` from *CakeBuild* :thumbsup:\r\n```Here is some code```",
            messageSettings:new SlackChatMessageSettings { IncomingWebHookUrl = slackhookuri }
    );

if (postMessageResult.Ok)
{
    Information("Message successfully sent");
}
else
{
    Error("Failed to send message: {0}", postMessageResult.Error);
}
```
Cake output will be similar to below:
```
Message successfully sent
```
This will result in an message in your Slack channel similar to below:

![Sample message](https://github.com/WCOMAB/Cake.Slack/raw/master/samplemessage.png)

### Using message attachments

```csharp
#addin "Cake.Slack"
var slackWebHookUrl = EnvironmentVariable("slackWebHookUrl");
var slackChannel = "#cake";
var slackAssemblyFieldAttachment = new SlackChatMessageAttachmentField[]
{
            new SlackChatMessageAttachmentField
            {
                Title =  "Message Attachment Title",
            	Value =  "Message Attachment Value"
            }
};
var postMessageResult = Slack.Chat.PostMessage(
	channel:slackChannel,
	text:"Starting Cake Build...",
	messageAttachments:new SlackChatMessageAttachment[]
	{
	            new SlackChatMessageAttachment
	            {
	                        Text = "Cake Text",
	                        Pretext = "Cake Pretext",
	                        Color = "#67A0E1",
	                        Fields = slackAssemblyFieldAttachment
	            }
     },
	messageSettings:new SlackChatMessageSettings { IncomingWebHookUrl = slackWebHookUrl });

if (postMessageResult.Ok)
{
    Information("Message successfully sent");
}
else
{
    Error("Failed to send message: {0}", postMessageResult.Error);
}
```
This will result in a message in your Slack channel similar to below:

![Sample message attachment](https://github.com/WCOMAB/Cake.Slack/raw/master/samplemessageattachment.png)
## Discussion

For questions and to discuss ideas & feature requests, use the [GitHub discussions on the Cake GitHub repository](https://github.com/cake-build/cake/discussions), under the [Extension Q&A](https://github.com/cake-build/cake/discussions/categories/extension-q-a) category.

[![Join in the discussion on the Cake repository](https://img.shields.io/badge/GitHub-Discussions-green?logo=github)](https://github.com/cake-build/cake/discussions)