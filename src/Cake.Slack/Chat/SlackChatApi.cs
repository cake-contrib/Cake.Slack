using System;
using System.Collections.Generic;
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
        private static readonly SlackChatMessageAttachment[] EmptySlackChatMessageAttachments = new SlackChatMessageAttachment[0];
        private const string PostMessageUri = "https://slack.com/api/chat.postMessage";

        internal static SlackChatMessageResult PostMessage(
            this ICakeContext context,
            string channel,
            string text,
            SlackChatMessageSettings messageSettings
            )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings), "Invalid slack message specified");
            }

            SlackChatMessageResult result;
            
            if (!string.IsNullOrWhiteSpace(messageSettings.IncomingWebHookUrl))
            {
                result = PostToIncomingWebHook(
                    context,
                    channel,
                    text,
                    EmptySlackChatMessageAttachments,
                    messageSettings
                    );
            }
            else
            {
                var messageParams = GetMessageParams(
                    messageSettings.Token,
                    channel,
                    text,
                    EmptySlackChatMessageAttachments,
                    messageSettings
                    );

                result = context.PostToChatApi(
                    PostMessageUri,
                    messageParams
                    );
            }

            if (!result.Ok && messageSettings.ThrowOnFail == true)
            {
                throw new CakeException(result.Error ?? "Failed to send message, unknown error");
            }

            return result;
        }

        internal static SlackChatMessageResult PostMessage(
            this ICakeContext context,
            string channel,
            string text,
            ICollection<SlackChatMessageAttachment> messageAttachments,
            SlackChatMessageSettings messageSettings
            )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings), "Invalid slack message specified");
            }

            if(messageAttachments == null)
            {
                throw new ArgumentNullException(nameof(messageAttachments), "Invalid slack messsage attachment");
            }

            SlackChatMessageResult result;
            if (!string.IsNullOrWhiteSpace(messageSettings.IncomingWebHookUrl))
            {
                result = PostToIncomingWebHook(
                    context,
                    channel,
                    text,
                    messageAttachments,
                    messageSettings
                    );
            }
            else
            {
                var messageParams = GetMessageParams(
                    messageSettings.Token,
                    channel,
                    text,
                    messageAttachments,
                    messageSettings
                    );

                result = context.PostToChatApi(
                    PostMessageUri,
                    messageParams
                    );

            }
            return result;
        }

        private static SlackChatMessageResult PostToIncomingWebHook(
            ICakeContext context,
            string channel,
            string text,
            ICollection<SlackChatMessageAttachment> messageAttachments,
            SlackChatMessageSettings messageSettings
        )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings), "Invalid slack message specified");
            }

            if (string.IsNullOrWhiteSpace(messageSettings.IncomingWebHookUrl))
            {
                throw new NullReferenceException("Invalid IncomingWebHookUrl supplied.");
            }

            if (messageAttachments == null)
            {
                throw new ArgumentNullException(nameof(messageAttachments), "Invalid attachment supplied");
            }

            context.Verbose(
                "Posting to incoming webhook {0}...",
                string.Concat(
                    messageSettings
                        .IncomingWebHookUrl
                        .TrimEnd('/')
                        .Reverse()
                        .SkipWhile(c => c != '/')
                        .Reverse()
                    )
                );

            var postJson = ToJson(
                new
                {
                    channel,
                    text,
                    username = messageSettings.UserName ?? "CakeBuild",
                    attachments = messageAttachments,
                    icon_url = messageSettings.IconUrl?.AbsoluteUri ?? "https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png"
                });

            context.Debug("Parameter: {0}", postJson);

            using (var client = new WebClient())
            {
                var postBytes = Encoding.UTF8.GetBytes(postJson);

                var resultBytes = client.UploadData(
                        messageSettings.IncomingWebHookUrl,
                        "POST",
                        postBytes
                        );

                var result = Encoding.UTF8.GetString(
                    resultBytes
                    );

                var parsedResult = new SlackChatMessageResult(
                    StringComparer.OrdinalIgnoreCase.Equals(result, "ok"),
                    channel,
                    string.Empty,
                    StringComparer.OrdinalIgnoreCase.Equals(result, "ok") ? string.Empty : result
                    );

                context.Debug("Result parsed: {0}", parsedResult);
                return parsedResult;
            }
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
                    "Parameters: {0}",
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
            ICollection<SlackChatMessageAttachment> messageAttachments,
            SlackChatMessageSettings messageSettings
            )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings), "Invalid slack message settings specified");
            }

            if (messageAttachments == null)
            {
                throw new ArgumentNullException(nameof(messageAttachments), "Invalid slack message attachments specified");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token), "Invalid Message Token specified");
            }

            if (string.IsNullOrWhiteSpace(channel))
            {
                throw new ArgumentNullException(nameof(channel), "Invalid Message Channel specified");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text), "Invalid Message Text specified");
            }

            var messageParams = new NameValueCollection
            {

                {"token", token},
                {"channel", channel},
                {"text", text},
                {"username", messageSettings.UserName ?? "CakeBuild"},
                {
                    "icon_url",
                    messageSettings.IconUrl?.AbsoluteUri ?? "https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png"
                }
            };

            if (messageAttachments.Count > 0)
            {
                messageParams.Add("attachments", ToJson(messageAttachments));
            }

            return messageParams;
        }

        private static string GetString(this JsonData data, string key)
        {
            return (data != null && data.Keys.Contains(key))
                ? (string)data[key]
                : null;
        }

        private static bool? GetBoolean(this JsonData data, string key)
        {
            return (data != null && data.Keys.Contains(key))
                ? (bool)data[key]
                : null as bool?;
        }

        private static string ToJson(object obj)
        {
            var jsonWriter = new JsonWriter { LowerCaseProperties = true };
            JsonMapper.ToJson(obj, jsonWriter);
            return jsonWriter.ToString();
        }
    }
}