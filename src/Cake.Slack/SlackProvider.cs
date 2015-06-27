using Cake.Core;
using Cake.Slack.Chat;

namespace Cake.Slack
{
    /// <summary>
    /// Contains functionality related to Slack API
    /// </summary>
    public sealed class SlackProvider
    {
        private readonly SlackChatProvider _chat;

        /// <summary>
        /// The Slack Chat functionality.
        /// </summary>
        public SlackChatProvider Chat { get { return _chat; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlackProvider"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SlackProvider(ICakeContext context)
        {
            _chat = new SlackChatProvider(context);
        }
    }
}
