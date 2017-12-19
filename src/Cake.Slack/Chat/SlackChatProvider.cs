using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;

// ReSharper disable UnusedMember.Global
namespace Cake.Slack.Chat
{
    /// <summary>
    /// Contains <see href="https://api.slack.com/">Slack API</see> Chat functionality.
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
        /// <param name="text">Text of the message to send. For message formatting see <see href="https://api.slack.com/docs/formatting">Formatting | Slack</see></param>
        /// <returns>Returns success/error/timestamp <see cref="SlackChatMessageResult"/></returns>
        /// <example>
        /// <code>
        ///     Information("Sending message to Slack...");
        /// 
        ///     var postMessageResult = Slack.Chat.PostMessage(
        ///         "token", 
        ///         "Cake", 
        ///         "It's not a party without Cake!"
        ///         );
        ///     
        ///     if(postMessageResult.Ok)
        ///     {
        ///         Information("Message {0} succesfully sent.", postMessageResult.TimeStamp);
        ///     }
        ///     else
        ///     {
        ///         Error("Failed to send message: {0}.", postMessageResult.Error);
        ///     }
        /// </code>
        /// </example>
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
        /// <param name="text">Text of the message to send. For message formatting see <see href="https://api.slack.com/docs/formatting">Formatting | Slack</see></param>
        /// <returns>Returns success/error/timestamp <see cref="SlackChatMessageResult"/></returns>
        /// <param name="messageSettings">Lets you override default settings like UserName, IconUrl or if it should ThrowOnFail</param>
        /// <example>
        /// <code>
        ///     Information("Sending message to Slack...");
        ///     
        ///     var settings = new SlackChatMessageSettings { Token = "token" };
        ///     var postMessageResult = Slack.Chat.PostMessage(
        ///         "Cake", 
        ///         "It's not a party without Cake!",
        ///         settings
        ///         );
        ///     
        ///     if(postMessageResult.Ok)
        ///     {
        ///         Information("Message {0} succesfully sent.", postMessageResult.TimeStamp);
        ///     }
        ///     else
        ///     {
        ///         Error("Failed to send message: {0}.", postMessageResult.Error);
        ///     }
        /// </code>
        /// </example>
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
        /// <param name="token">SlackProvider auth token</param>
        /// <param name="channel">Channel to send message to. Can be a public channel, private group or IM channel. Can be an encoded ID, or a name.</param>
        /// <param name="text">Text of the message to send. For message formatting see <see href="https://api.slack.com/docs/formatting">Formatting | Slack</see></param>
        /// <param name="messageAttachments">Lets you send a message attachment see <see href="https://api.slack.com/docs/attachments">Attachments | Slack</see></param>
        /// <returns>Returns success/error/timestamp <see cref="SlackChatMessageResult"/></returns>
        /// <example>
        /// <code>
        ///     Information("Sending message to Slack...");
        ///     
        ///     var postMessageResult = Slack.Chat.PostMessage(
        ///         "token", 
        ///         "Cake", 
        ///         "It's not a party without Cake!",
        ///         new new [] { new SlackChatMessageAttachment() }
        ///         );
        ///     
        ///     if(postMessageResult.Ok)
        ///     {
        ///         Information("Message {0} succesfully sent.", postMessageResult.TimeStamp);
        ///     }
        ///     else
        ///     {
        ///         Error("Failed to send message: {0}.", postMessageResult.Error);
        ///     }
        /// </code>
        /// </example>
        [CakeAliasCategory("Chat")]
        public SlackChatMessageResult PostMessage(
            string token,
            string channel,
            string text,
            ICollection<SlackChatMessageAttachment> messageAttachments
            )
        {
            return _context.PostMessage(
                channel,
                text,
                messageAttachments,
                new SlackChatMessageSettings { Token = token }
                );
        }

        /// <summary>
        /// Post message to Slack Channel
        /// </summary>
        /// <param name="channel">Channel to send message to. Can be a public channel, private group or IM channel. Can be an encoded ID, or a name.</param>
        /// <param name="text">Text of the message to send. For message formatting see <see href="https://api.slack.com/docs/formatting">Formatting | Slack</see></param>
        /// <param name="messageAttachments">Lets you send a message attachment see <see href="https://api.slack.com/docs/attachments">Attachments | Slack</see></param>
        /// <param name="messageSettings">Lets you override default settings like UserName, IconUrl or if it should ThrowOnFail</param>
        /// <returns>
        /// Returns success/error/timestamp <see cref="SlackChatMessageResult" />
        /// </returns>
        /// <exception cref="System.ArgumentNullException">messageSettings</exception>
        /// <example>
        /// <code>
        ///     Information("Sending message to Slack...");
        ///     
        ///     var settings = new SlackChatMessageSettings { Token = "token" };
        ///     var postMessageResult = Slack.Chat.PostMessage(
        ///         "Cake", 
        ///         "It's not a party without Cake!",
        ///         new new [] { new SlackChatMessageAttachment() },
        ///         settings
        ///         );
        ///     
        ///     if(postMessageResult.Ok)
        ///     {
        ///         Information("Message {0} succesfully sent.", postMessageResult.TimeStamp);
        ///     }
        ///     else
        ///     {
        ///         Error("Failed to send message: {0}.", postMessageResult.Error);
        ///     }
        /// </code>
        /// </example>
        [CakeAliasCategory("Chat")]
        public SlackChatMessageResult PostMessage(
            string channel,
            string text,
            ICollection<SlackChatMessageAttachment> messageAttachments,
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
