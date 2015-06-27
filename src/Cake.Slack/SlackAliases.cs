using System;
using Cake.Common.Build;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Slack
{
    /// <summary>
    /// Contains aliases related to SlackProvider API
    /// </summary>
    public static class SlackAliases
    {
        /// <summary>
        /// Gets a <see cref="SlackProvider"/> instance that can be used for communicating with SlackProvider API.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.BuildSystem"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
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
