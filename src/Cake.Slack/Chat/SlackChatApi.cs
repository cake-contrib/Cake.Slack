using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Slack.LitJson;

namespace Cake.Slack.Chat
{
    internal static class SlackChatApi
    {
        internal static SlackChatMessageResult PostMessage(
            this ICakeContext context,
            string token,
            string channel,
            string text,
            SlackChatMessageSettings messageSettings
            )
        {
            const string postMessageUri = "https://slack.com/api/chat.postMessage";
            var messageParams = GetMessageParams(
                token,
                channel,
                text,
                messageSettings
                );

            var result = context.PostToChatApi(postMessageUri,
                messageParams
                );

            if (!result.Ok && messageSettings.ThrowOnFail == true)
            {
                throw new CakeException(result.Error ?? "Failed to send message, unknown error");
            }

            return result;
        }


        private static SlackChatMessageResult PostToChatApi(
            this ICakeContext context,
            string apiUri,
            NameValueCollection apiParameters
            )
        {
            using (var client = new WebClient())
            {
                context.Verbose("Posting to {0}", apiUri);

                context.Verbose(
                    "Parematers: {0}",
                    apiParameters
                        .Keys
                        .Cast<string>()
                        .Aggregate(
                            new StringBuilder(),
                            (sb, key) =>
                            {
                                sb.AppendFormat(
                                    "{0}={1}\r\n",
                                    key,
                                    (StringComparer.InvariantCultureIgnoreCase.Equals(key, "token"))
                                        ? "*redacted*"
                                        : string.Join(
                                            ",",
                                            apiParameters.GetValues(key) ?? new string[0]
                                            )
                                    );
                                return sb;
                            },
                            r => r.ToString()
                        )
                    );

                var resultBytes = client.UploadValues(
                    apiUri,
                    apiParameters
                    );
                var resultJson = Encoding.UTF8.GetString(
                    resultBytes
                    );

                context.Debug("Result json: {0}", resultJson);

                var result = JsonMapper.ToObject(resultJson);
                var parsedResult = new SlackChatMessageResult(
                    result.GetBoolean("ok") ?? false,
                    result.GetString("channel"),
                    result.GetString("ts"),
                    result.GetString("error")
                    );
                context.Debug("Result parsed: {0}", parsedResult);
                return parsedResult;
            }
        }

        private static NameValueCollection GetMessageParams(
            string token,
            string channel,
            string text,
            SlackChatMessageSettings messageSettings
            )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException("messageSettings", "Invalid slack message specified");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token", "Invalid Message Token specified");
            }

            if (string.IsNullOrWhiteSpace(channel))
            {
                throw new ArgumentNullException("channel", "Invalid Message Channel specified");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("text", "Invalid Message Text specified");
            }

            var messageParams = new NameValueCollection
            {
                {"token", token},
                {"channel", channel},
                {"text", text},
                {"username", messageSettings.UserName ?? "CakeBuild"},
                {
                    "icon_url",
                    messageSettings.IconUrl != null
                        ? messageSettings.IconUrl.AbsoluteUri
                        : "https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png"
                }
            };
            return messageParams;
        }

        private static string GetString(this JsonData data, string key)
        {
            return (data != null && data.Keys.Contains(key))
                ? (string) data[key]
                : null;
        }

        private static bool? GetBoolean(this JsonData data, string key)
        {
            return (data != null && data.Keys.Contains(key))
                ? (bool) data[key]
                : null as bool?;
        }
    }
}