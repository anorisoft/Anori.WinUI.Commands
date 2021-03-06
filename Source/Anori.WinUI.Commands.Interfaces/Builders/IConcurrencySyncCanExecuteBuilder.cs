﻿// -----------------------------------------------------------------------
// <copyright file="IConcurrencySyncCanExecuteBuilder.cs" company="AnoriSoft">
// Copyright (c) AnoriSoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.Interfaces.Builders
{
    using System;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    /// <summary>
    ///     The Concurrency Synchronize Can Execute Builder interface.
    /// </summary>
    public interface IConcurrencySyncCanExecuteBuilder
    {
        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns>Concurrency Sync Command.</returns>
        [NotNull]
        IConcurrencySyncCommand Build();

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns>Concurrency Sync Command.</returns>
        [NotNull]
        IConcurrencySyncCommand Build([NotNull] Action<IConcurrencySyncCommand> setCommand);

        /// <summary>
        ///     Activatables this instance.
        /// </summary>
        /// <returns>Activatable Concurrency Sync Can Execute Command Builder.</returns>
        [NotNull]
        IActivatableConcurrencySyncCanExecuteBuilder Activatable();

        /// <summary>
        ///     Observeses the property.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>Concurrency Sync Can Execute Command Builder.</returns>
        [NotNull]
        IConcurrencySyncCanExecuteBuilder ObservesProperty<TType>([NotNull] Expression<Func<TType>> expression);

        /// <summary>
        ///     Observeses the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>Concurrency Sync Can Execute Command Builder.</returns>
        [NotNull]
        IConcurrencySyncCanExecuteBuilder Observes([NotNull] ICanExecuteChangedSubject observer);

        /// <summary>
        ///     Observeses the command manager.
        /// </summary>
        /// <returns>Concurrency Sync Can Execute Command Builder.</returns>
        [NotNull]
        IConcurrencySyncCanExecuteBuilder ObservesCommandManager();

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        [NotNull]
        IConcurrencySyncCanExecuteBuilder OnError([NotNull] Action<Exception> error);

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        [NotNull]
        IConcurrencySyncCanExecuteBuilder OnCompleted([NotNull] Action completed);

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns>Concurrency Sync Can Execute Command Builder.</returns>
        [NotNull]
        IConcurrencySyncCanExecuteBuilder OnCancel([NotNull] Action cancel);
    }
}