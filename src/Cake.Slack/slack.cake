#r "Cake.Slack.dll"
try 
{
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
}
catch(Exception ex)
{
    Error("{0}", ex);
}
Console.ReadLine();