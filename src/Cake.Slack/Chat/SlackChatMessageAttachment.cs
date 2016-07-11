using Cake.Core.Annotations;
using System.Collections.Generic;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Cake.Slack.Chat
{
    /// <summary>
    /// Class to allow for message attachments
    /// </summary>
    [CakeAliasCategory("Slack")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class SlackChatMessageAttachment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SlackChatMessageAttachment"/> class.
        /// </summary>
        public SlackChatMessageAttachment()
        {
            Color = "#000000";
        }

        /// <summary>
        /// Required Text summary of the attachment that is shown by clients that understand attachments but choose not to show them.
        /// </summary>
        /// <value>
        /// The fallback.
        /// </value>
        public string Fallback { get; set; }

        /// <summary>
        /// Optional Text that should appear within the attachment
        /// </summary>
        /// <value>
        /// The Text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Optional Text that should appear above the formatted data
        /// </summary>
        /// <value>
        /// The Pretext.
        /// </value>
        public string Pretext { get; set; }

        /// <summary>
        /// Color displayed with the attachment
        /// </summary>
        /// <value>
        /// The Color.
        /// </value>
        public string Color { get ; set; }

        /// <summary>
        /// The message title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// The message title link.
        /// </summary>
        /// <value>
        /// The title link.
        /// </value>
        public string Title_Link { get; set; }

        /// <summary>
        /// The message author's name.
        /// </summary>
        /// <value>
        /// The author_name.
        /// </value>
        public string Author_Name { get; set; }

        /// <summary>
        /// Link to the message author
        /// </summary>
        /// <value>
        /// The author_link.
        /// </value>
        public string Author_Link { get; set; }

        /// <summary>
        /// The message author's icon url
        /// </summary>
        /// <value>
        /// The author_icon.
        /// </value>
        public string Author_Icon { get; set; }

        /// <summary>
        /// Url to an image to display in message.
        /// </summary>
        /// <value>
        /// The image_url.
        /// </value>
        public string Image_Url { get; set; }

        /// <summary>
        /// Url to thumbprint to display in message.
        /// </summary>
        /// <value>
        /// The thumb_url.
        /// </value>
        public string Thumb_Url { get; set; }

        /// <summary>
        /// Collection of <see cref="SlackChatMessageAttachmentField"/>
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public IList<SlackChatMessageAttachmentField> Fields { get; set; }
    }
}
