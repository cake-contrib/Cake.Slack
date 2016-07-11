using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Slack
{
    /// <summary>
    /// Contains aliases related to Slack API
    /// </summary>
    [CakeAliasCategory("Slack")]
    // ReSharper disable once UnusedMember.Global
    public static class SlackAliases
    {
        /// <summary>
        /// Gets a <see cref="SlackProvider"/> instance that can be used for communicating with Slack API.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="SlackProvider"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImportAttribute("Cake.Slack.Chat")]
        // ReSharper disable once UnusedMember.Global
        public static SlackProvider Slack(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return new SlackProvider(context);
        }
    }
}
