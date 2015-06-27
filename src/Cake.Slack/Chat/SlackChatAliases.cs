using System;
using Cake.Core;
using Cake.Core.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
namespace Cake.Slack.Chat
{
    /// <summary>
    /// Contains SlackProvider Chat Aliases
    /// </summary>
    [Obsolete("Should now use SlackProvider property instead now SlackProvider.Chat")]
    [CakeAliasCategory("SlackProvider.Chat")]
    public static class SlackChatAliases
    {
        /// <summary>
        /// Post message to Slack Channel
        /// </summary>
        /// <param name="context">Cake context</param>
        /// <param name="token">SlackProvider auth token</param>
        /// <param name="channel">Channel to send message to. Can be a public channel, private group or IM channel. Can be an encoded ID, or a name.</param>
        /// <param name="text">Text of the message to send. For message formatting see https://api.slack.com/docs/formatting</param>
        /// <returns>Returns success/error/timestamp <see cref="SlackChatMessageResult"/></returns>
        [CakeMethodAlias]
        [Obsolete("Should now use SlackProvider property instead now SlackProvider.Chat.PostMessage()")]
        public static SlackChatMessageResult SlackChatPostMessage(
            this ICakeContext context,
            string token,
            string channel,
            string text
            )
        {
            return context.SlackChatPostMessage(
                token,
                channel,
                text,
                new SlackChatMessageSettings()
                );
        }


        /// <summary>
        /// Post message to Slack Channel
        /// </summary>
        /// <param name="context">Cake context</param>
        /// <param name="token">SlackProvider auth token</param>
        /// <param name="channel">Channel to send message to. Can be a public channel, private group or IM channel. Can be an encoded ID, or a name.</param>
        /// <param name="text">Text of the message to send. For message formatting see https://api.slack.com/docs/formatting</param>
        /// <param name="messageSettings">Lets you override default settings like UserName, IconUrl or if it should ThrowOnFail</param>
        /// <returns>Returns success/error/timestamp <see cref="SlackChatMessageResult"/></returns>
        [Obsolete("Should now use SlackProvider property instead now SlackProvider.Chat.PostMessage()")]
        [CakeMethodAlias]
        public static SlackChatMessageResult SlackChatPostMessage(
            this ICakeContext context,
            string token,
            string channel,
            string text,
            SlackChatMessageSettings messageSettings
            )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException("messageSettings", "Invalid message settings supplied");
            }

            messageSettings.Token = token;

            return context.PostMessage(
                channel,
                text,
                messageSettings
                );
        }
    }
}