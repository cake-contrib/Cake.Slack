using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Slack.LitJson;

namespace Cake.Slack.Chat
{
    internal static class SlackChatApi
    {
        private static readonly SlackChatMessageAttachment[] EmptySlackChatMessageAttachments = new SlackChatMessageAttachment[0];

        private const string PostMessageUri = "https://slack.com/api/chat.postMessage";

        internal static async Task<SlackChatMessageResult> PostMessage(
            this ICakeContext context,
            string channel,
            string text,
            SlackChatMessageSettings messageSettings)
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings), "Invalid slack message specified");
            }

            SlackChatMessageResult result;

            if (!string.IsNullOrWhiteSpace(messageSettings.IncomingWebHookUrl))
            {
                result = await PostToIncomingWebHook(
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

                result = await context.PostToChatApi(
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

        internal static async Task<SlackChatMessageResult> PostMessage(
            this ICakeContext context,
            string channel,
            string text,
            ICollection<SlackChatMessageAttachment> messageAttachments,
            SlackChatMessageSettings messageSettings)
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings), "Invalid slack message specified");
            }

            if (messageAttachments == null)
            {
                throw new ArgumentNullException(nameof(messageAttachments), "Invalid slack messsage attachment");
            }

            SlackChatMessageResult result;
            if (!string.IsNullOrWhiteSpace(messageSettings.IncomingWebHookUrl))
            {
                result = await PostToIncomingWebHook(
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

                result = await context.PostToChatApi(
                    PostMessageUri,
                    messageParams
                );
            }
            return result;
        }

        private static async Task<SlackChatMessageResult> PostToIncomingWebHook(
            ICakeContext context,
            string channel,
            string text,
            ICollection<SlackChatMessageAttachment> messageAttachments,
            SlackChatMessageSettings messageSettings)
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

            var json = ToJson(
                new
                {
                    channel,
                    text,
                    username = messageSettings.UserName ?? "CakeBuild",
                    attachments = messageAttachments,
                    icon_url =
                        messageSettings.IconUrl?.AbsoluteUri ??
                        "https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png",
                    link_names = messageSettings.LinkNames.ToString().ToLower(),
                });

            context.Debug("Parameter: {0}", json);

            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponse = await client.PostAsync(messageSettings.IncomingWebHookUrl, content);

                var response = await httpResponse.Content.ReadAsStringAsync();
                context.Debug($"Status Code: {httpResponse.StatusCode}");
                context.Debug($"Response: {response}");

                var parsedResult = new SlackChatMessageResult(
                    StringComparer.OrdinalIgnoreCase.Equals(response, "ok"),
                    channel,
                    string.Empty,
                    StringComparer.OrdinalIgnoreCase.Equals(response, "ok") ? string.Empty : response
                );

                context.Debug("Result parsed: {0}", parsedResult);
                return parsedResult;
            }
        }

        private static async Task<SlackChatMessageResult> PostToChatApi(
            this ICakeContext context,
            string apiUri,
            Dictionary<string, string> apiParameters)
        {
            using (var client = new HttpClient()
            {
                BaseAddress = new Uri(apiUri)
            })
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
                                    (StringComparer.CurrentCultureIgnoreCase.Equals(key, "token"))
                                        ? "*redacted*"
                                        : string.Join(
                                            ",",
                                            apiParameters[key] ?? string.Empty
                                        )
                                );
                                return sb;
                            },
                            r => r.ToString()
                        )
                );

                var json = ToJson(apiParameters);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponse = await client.PostAsync(apiUri, content);
                var response = await httpResponse.Content.ReadAsStringAsync();
                context.Debug($"Status Code: {httpResponse.StatusCode}");
                context.Debug($"Response: {response}");

                var result = JsonMapper.ToObject(response);
                context.Debug($"Result: {result}");

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

        private static Dictionary<string, string> GetMessageParams(
            string token,
            string channel,
            string text,
            ICollection<SlackChatMessageAttachment> messageAttachments,
            SlackChatMessageSettings messageSettings)
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings), "Invalid slack message settings specified");
            }

            if (messageAttachments == null)
            {
                throw new ArgumentNullException(nameof(messageAttachments),
                    "Invalid slack message attachments specified");
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

            var messageParams = new Dictionary<string, string>
            {
                {"token", token},
                {"channel", channel},
                {"text", text},
                {"username", messageSettings.UserName ?? "CakeBuild"},
                {
                    "icon_url",
                    messageSettings.IconUrl?.AbsoluteUri ??
                    "https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png"
                },
                {"link_names", messageSettings.LinkNames.ToString().ToLower()},
            };

            if (messageAttachments.Count > 0)
            {
                messageParams.Add("attachments", ToJson(messageAttachments));
            }

            return messageParams;
        }

        private static bool? GetBoolean(this JsonData data, string key)
        {
            return (data != null && data.Keys.Contains(key))
                ? (bool)data[key]
                : null as bool?;
        }

        private static string GetString(this JsonData data, string key)
        {
            return (data != null && data.Keys.Contains(key))
                ? (string)data[key]
                : null;
        }

        private static string ToJson(object obj)
        {
            var jsonWriter = new JsonWriter
            {
                LowerCaseProperties = true
            };
            JsonMapper.ToJson(obj, jsonWriter);
            return jsonWriter.ToString();
        }
    }
}