﻿// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace IDemoActorInterface
{
    using System.Threading.Tasks;
    using Dapr.Actors;

    /// <summary>
    /// Interface for Actor method.
    /// </summary>
    public interface IDemoActor : IActor
    {
        /// <summary>
        /// Method to save data.
        /// </summary>
        /// <param name="data">DAta to save.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task<string> SaveData(MyData data);

        /// <summary>
        /// Method to get data.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task<MyData> GetData();

        /// <summary>
        /// Registers a reminder.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task RegisterReminder();

        /// <summary>
        /// Unregisters the registered reminder.
        /// </summary>
        /// <returns>Task representing the operation.</returns>
        Task UnregisterReminder();

        /// <summary>
        /// Registers a timer.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task RegisterTimer();

        /// <summary>
        /// Unregisters the registered timer.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task UnregisterTimer();
    }

    /// <summary>
    /// Data Used by the Sample Actor.
    /// </summary>
    public class MyData
    {
        /// <summary>
        /// Gets or sets the value for PropertyA.
        /// </summary>
        public string PropertyA { get; set; }

        /// <summary>
        /// Gets or sets the value for PropertyB.
        /// </summary>
        public string PropertyB { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var propAValue = this.PropertyA == null ? "null" : this.PropertyA;
            var propBValue = this.PropertyB == null ? "null" : this.PropertyB;
            return $"PropertyA: {propAValue}, PropertyB: {propBValue}";
        }
    }
}
