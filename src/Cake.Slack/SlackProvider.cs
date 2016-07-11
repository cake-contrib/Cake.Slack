using Cake.Core;
using Cake.Slack.Chat;

namespace Cake.Slack
{
    /// <summary>
    /// Contains functionality related to Slack API
    /// </summary>
    public sealed class SlackProvider
    {
        /// <summary>
        /// The Slack Chat functionality.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public SlackChatProvider Chat { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlackProvider"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SlackProvider(ICakeContext context)
        {
            Chat = new SlackChatProvider(context);
        }
    }
}
