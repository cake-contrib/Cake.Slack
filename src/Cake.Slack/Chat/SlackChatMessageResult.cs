using System.Text;
using Cake.Core.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
namespace Cake.Slack.Chat
{
    /// <summary>
    /// The result of SlackProvider Chat API post
    /// </summary>
    [CakeAliasCategory("Slack")]
    public sealed class SlackChatMessageResult
    {
        /// <summary>
        /// Indicating success or failure, <see cref="Error"/> for info on failure 
        /// </summary>
        public bool Ok { get; }

        /// <summary>
        /// Encoded channel ID
        /// </summary>
        public string Channel { get; }

        /// <summary>
        /// Timestamp of the message
        /// </summary>
        public string TimeStamp { get; }

        /// <summary>
        /// Error message on failure
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ok">Indicating success or failure</param>
        /// <param name="channel">Encoded channel ID</param>
        /// <param name="timeStamp">Timestamp of the message</param>
        /// <param name="error">Error message on failure</param>
        public SlackChatMessageResult(bool ok, string channel, string timeStamp, string error)
        {
            Ok = ok;
            Channel = channel;
            TimeStamp = timeStamp;
            Error = error;
        }

        /// <summary>
        /// Converst this instance of value to a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{ Ok = ");
            builder.Append(Ok);
            builder.Append(", Channel = ");
            builder.Append(Channel);
            builder.Append(", TimeStamp = ");
            builder.Append(TimeStamp);
            builder.Append(", Error = ");
            builder.Append(Error);
            builder.Append(" }");
            return builder.ToString();
        }
    }
}