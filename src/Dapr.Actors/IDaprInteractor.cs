﻿// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Dapr.Actors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapr.Actors.Communication;
    using Dapr.Actors.Runtime;

    /// <summary>
    /// Interface for interacting with Dapr runtime.
    /// </summary>
    internal interface IDaprInteractor
    {
        /// <summary>
        /// Invokes an Actor method on Dapr without remoting.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="methodName">Method name to invoke.</param>
        /// <param name="jsonPayload">Serialized body.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<Stream> InvokeActorMethodWithoutRemotingAsync(string actorType, string actorId, string methodName, string jsonPayload, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves Actor state. This is temporary until the Dapr runtime implements the Batch state update.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="keyName">state name.</param>
        /// <param name="data">State to be saved.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SaveStateAsync(string actorType, string actorId, string keyName, string data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes Actor state. This is temporary until the Dapr runtime implements the Batch state update.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="keyName">state name.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveStateAsync(string actorType, string actorId, string keyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves state batch to Dapr.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="data">Json data with state changes as per the dapr spec for transaction state update.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SaveStateTransactionallyAsync(string actorType, string actorId, string data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves a state to Dapr.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="keyName">Name of key to get value for.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<string> GetStateAsync(string actorType, string actorId, string keyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Invokes Actor method.
        /// </summary>
        /// <param name="serializersManager">Serializers manager for remoting calls.</param>
        /// <param name="remotingRequestRequestMessage">Actor Request Message.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<IActorResponseMessage> InvokeActorMethodWithRemotingAsync(ActorMessageSerializersManager serializersManager, IActorRequestMessage remotingRequestRequestMessage, CancellationToken cancellationToken = default);

        /// <summary>
        /// Register a reminder.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="reminderName">Name of reminder to register.</param>
        /// <param name="data">Json reminder data as per the Dapr spec.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task RegisterReminderAsync(string actorType, string actorId, string reminderName, string data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unregisters a reminder.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="reminderName">Name of reminder to unregister.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task UnregisterReminderAsync(string actorType, string actorId, string reminderName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Registers a timer.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="timerName">Name of timer to register.</param>
        /// <param name="data">Json reminder data as per the Dapr spec.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task RegisterTimerAsync(string actorType, string actorId, string timerName, string data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unegisters a timer.
        /// </summary>
        /// <param name="actorType">Type of actor.</param>
        /// <param name="actorId">ActorId.</param>
        /// <param name="timerName">Name of timer to register.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task UnregisterTimerAsync(string actorType, string actorId, string timerName, CancellationToken cancellationToken = default);
    }
}
