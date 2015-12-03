using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Slack.Chat
{
    /// <summary>
    /// Contains SlackProvider Chat functionality.
    /// </summary>
    [CakeAliasCategory("Slack")]
    public sealed class SlackChatProvider
    {
        private readonly ICakeContext _context;
        
        /// <summary>
        /// Post message to Slack Channel
        /// </summary>
        /// <param name="token">SlackProvider auth token</param>
        /// <param name="channel">Channel to send message to. Can be a public channel, private group or IM channel. Can be an encoded ID, or a name.</param>
        /// <param name="text">Text of the message to send. For message formatting see https://api.slack.com/docs/formatting</param>
        /// <returns>Returns success/error/timestamp <see cref="SlackChatMessageResult"/></returns>
        [CakeAliasCategory("Chat")]
        public SlackChatMessageResult PostMessage(
            string token,
            string channel,
            string text
            )
        {
            return _context.PostMessage(
                channel,
                text,
                new SlackChatMessageSettings {Token = token}
                );
        }

        /// <summary>
        /// Post message to Slack Channel
        /// </summary>
        /// <param name="channel">Channel to send message to. Can be a public channel, private group or IM channel. Can be an encoded ID, or a name.</param>
        /// <param name="text">Text of the message to send. For message formatting see https://api.slack.com/docs/formatting</param>
        /// <returns>Returns success/error/timestamp <see cref="SlackChatMessageResult"/></returns>
        /// <param name="messageSettings">Lets you override default settings like UserName, IconUrl or if it should ThrowOnFail</param>
        [CakeAliasCategory("Chat")]
        public SlackChatMessageResult PostMessage(
            string channel,
            string text,
            SlackChatMessageSettings messageSettings
            )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings));
            }

            return _context.PostMessage(
                channel,
                text,
                messageSettings
                );
        }

        /// <summary>
        /// Post message to Slack Channel
        /// </summary>
        /// <param name="channel">Channel to send message to. Can be a public channel, private group or IM channel. Can be an encoded ID, or a name.</param>
        /// <param name="text">Text of the message to send. For message formatting see https://api.slack.com/docs/formatting</param>
        /// <param name="messageAttachments">Lets you send a message attachment see https://api.slack.com/docs/attachments</param>
        /// <param name="messageSettings">Lets you override default settings like UserName, IconUrl or if it should ThrowOnFail</param>
        /// <returns>
        /// Returns success/error/timestamp <see cref="SlackChatMessageResult" />
        /// </returns>
        /// <exception cref="System.ArgumentNullException">messageSettings</exception>
        [CakeAliasCategory("Chat")]
        public SlackChatMessageResult PostMessage(
            string channel,
            string text,
            SlackChatMessageAttachment[] messageAttachments,
            SlackChatMessageSettings messageSettings
            )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException(nameof(messageSettings));
            }

            if(messageAttachments == null)
            {
                throw new ArgumentNullException(nameof(messageAttachments));
            }

            return _context.PostMessage(
                channel,
                text,
                messageAttachments,
                messageSettings
                );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlackChatProvider"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SlackChatProvider(ICakeContext context)
        {
            _context = context;
        }
    }
}
