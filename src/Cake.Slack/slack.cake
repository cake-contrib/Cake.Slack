#r "Cake.Slack.dll"
var slackChannel    = "#cake";
var slackToken      = EnvironmentVariable("SLACK_TOKEN");
var slackhookuri    = EnvironmentVariable("slackhookuri");

try
{
    var postMessageResult = Slack.Chat.PostMessage(
                token:slackToken,
                channel:slackChannel,
                text:"This _is_ a `message` from *CakeBuild* using token:thumbsup:\r\n```Here is some code```"
        );

    if (postMessageResult.Ok)
    {
        Information("Message {0} succcessfully sent", postMessageResult.TimeStamp);
    }
    else
    {
        Error("Failed to send message: {0}", postMessageResult.Error);
    }
}
catch(Exception ex)
{
    Error("{0}", ex);
}

try
{
    var postMessageResult = Slack.Chat.PostMessage(
                channel:slackChannel,
                text:"This _is_ a `message` from *CakeBuild* using incoming web hook:thumbsup:\r\n```Here is some code```",
                messageSettings:new SlackChatMessageSettings { IncomingWebHookUrl = slackhookuri }
        );

    if (postMessageResult.Ok)
    {
        Information("Message {0} succcessfully sent", postMessageResult.TimeStamp);
    }
    else
    {
        Error("Failed to send message: {0}", postMessageResult.Error);
    }
}
catch(Exception ex)
{
    Error("{0}", ex);
}

Console.ReadLine();