using System;
using Cake.Core.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
namespace Cake.Slack.Chat
{
    /// <summary>
    /// Class that lets you override default API settings
    /// </summary>
    
    [CakeAliasCategory("Slack")]
    public sealed class SlackChatMessageSettings
    {
        /// <summary>
        /// Slack token used for authentication.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// This can be used instead of token (https://api.slack.com/incoming-webhooks)
        /// </summary>
        public string IncomingWebHookUrl { get; set; }

        /// <summary>
        /// Name of bot.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// URL to an image to use as the icon for this message
        /// </summary>
        public Uri IconUrl { get; set; }

        /// <summary>
        /// Find and link channel names and usernames.
        /// </summary>
        public bool LinkNames { get; set; }

        /// <summary>
        /// Optional flag for if should throw exception on failure
        /// </summary>
        public bool? ThrowOnFail { get; set; }
    }
}