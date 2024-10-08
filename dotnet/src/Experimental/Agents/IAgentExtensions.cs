﻿// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Microsoft.SemanticKernel.Experimental.Agents;

/// <summary>
/// Convenience actions for <see cref="IAgent"/>.
/// </summary>
public static class IAgentExtensions
{
    /// <summary>
    /// Invoke agent with user input
    /// </summary>
    /// <param name="agent">the agent</param>
    /// <param name="input">the user input</param>
    /// <param name="cancellationToken">a cancel token</param>
    /// <returns>chat messages</returns>
    public static async IAsyncEnumerable<IChatMessage> InvokeAsync(
        this IAgent agent,
        string input,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IAgentThread thread = await agent.NewThreadAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await foreach (var message in thread.InvokeAsync(agent, input, cancellationToken))
            {
                yield return message;
            }
        }
        finally
        {
            await thread.DeleteAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
