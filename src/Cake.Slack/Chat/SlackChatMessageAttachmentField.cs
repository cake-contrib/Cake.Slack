using Cake.Core.Annotations;

namespace Cake.Slack.Chat
{
    /// <summary>
    /// Field for message attachment
    /// </summary>
    [CakeAliasCategory("Slack")]
    public sealed class SlackChatMessageAttachmentField
    {
        /// <summary>
        /// Required Field Title
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Text value of the field. May contain standard message markup and must be escaped as normal
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Optional flag indicating whether the `value` is short enough to be displayed side-by-side with other values
        /// </summary>
        /// <value>
        ///   <c>true</c> if short; otherwise, <c>false</c>.
        /// </value>
        public bool Short { get; set; }
    }
}