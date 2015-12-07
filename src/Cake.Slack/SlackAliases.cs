using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Slack
{
    /// <summary>
    /// Contains aliases related to Slack API
    /// </summary>
    [CakeAliasCategory("Slack")]
    public static class SlackAliases
    {
        /// <summary>
        /// Gets a <see cref="SlackProvider"/> instance that can be used for communicating with Slack API.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="SlackProvider"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImportAttribute("Cake.Slack.Chat")]
        public static SlackProvider Slack(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return new SlackProvider(context);
        }
    }
}
