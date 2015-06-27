using System;
using Cake.Core;

namespace Cake.Slack.Chat
{
    /// <summary>
    /// Contains SlackProvider Chat functionality.
    /// </summary>
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
        public SlackChatMessageResult PostMessage(
            string channel,
            string text,
            SlackChatMessageSettings messageSettings
            )
        {
            if (messageSettings == null)
            {
                throw new ArgumentNullException("messageSettings");
            }

            return _context.PostMessage(
                channel,
                text,
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
