using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Cake.Slack.Chat
{
    /// <summary>
    /// Class that lets you override default API settings
    /// </summary>
    public sealed class SlackChatMessageSettings
    {
        /// <summary>
        /// Name of bot.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// URL to an image to use as the icon for this message
        /// </summary>
        public Uri IconUrl { get; set; }
        /// <summary>
        /// Optional flag for if should throw exception on failure
        /// </summary>
        public bool? ThrowOnFail { get; set; }
    }
}