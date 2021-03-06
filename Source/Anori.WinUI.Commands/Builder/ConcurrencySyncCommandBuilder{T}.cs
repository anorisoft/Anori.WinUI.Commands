﻿// -----------------------------------------------------------------------
// <copyright file="ConcurrencySyncCommandBuilder{T}.cs" company="AnoriSoft">
// Copyright (c) AnoriSoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;

    using Anori.WinUI.Commands.CanExecuteObservers;
    using Anori.WinUI.Commands.Commands;
    using Anori.WinUI.Commands.Exceptions;
    using Anori.WinUI.Commands.Interfaces;
    using Anori.WinUI.Commands.Interfaces.Builders;

    using JetBrains.Annotations;

    /// <summary>
    ///     The Concurrency Synchronize Command Builder class.
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.Builders.IConcurrencySyncCommandBuilder{T}" />
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.Builders.IConcurrencySyncCanExecuteBuilder{T}" />
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.Builders.IActivatableConcurrencySyncCommandBuilder{T}" />
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.Builders.IActivatableConcurrencySyncCanExecuteBuilder{T}" />
    internal sealed class ConcurrencySyncCommandBuilder<T> : IConcurrencySyncCommandBuilder<T>,
                                                             IConcurrencySyncCanExecuteBuilder<T>,
                                                             IActivatableConcurrencySyncCommandBuilder<T>,
                                                             IActivatableConcurrencySyncCanExecuteBuilder<T>
    {
        /// <summary>
        ///     The execute.
        /// </summary>
        private readonly Action<T, CancellationToken> execute;

        /// <summary>
        ///     The observes.
        /// </summary>
        private readonly List<ICanExecuteChangedSubject> observes = new List<ICanExecuteChangedSubject>();

        /// <summary>
        ///     The cancel action.
        /// </summary>
        private Action cancelAction;

        /// <summary>
        ///     The can execute function.
        /// </summary>
        private Predicate<T> canExecuteFunction;

        /// <summary>
        ///     The can execute expression.
        /// </summary>
        private ICanExecuteSubject canExecuteSubject;

        /// <summary>
        ///     The completed action.
        /// </summary>
        private Action completedAction;

        /// <summary>
        ///     The error action.
        /// </summary>
        private Action<Exception> errorAction;

        /// <summary>
        ///     The is automatic actiate.
        /// </summary>
        private bool isAutoActivate = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConcurrencySyncCommandBuilder{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <exception cref="ArgumentNullException">execute is null.</exception>
        public ConcurrencySyncCommandBuilder([NotNull] Action<T, CancellationToken> execute) =>
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Command.
        /// </returns>
        IActivatableConcurrencySyncCommand<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.Build(
            Action<IActivatableConcurrencySyncCommand<T>> setCommand) =>
            this.BuildActivatable(setCommand);

        /// <summary>
        ///     Observeses the property.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.
            ObservesProperty<TType>(Expression<Func<TType>> expression) =>
            this.ObservesProperty(expression);

        /// <summary>
        ///     Observeses the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.Observes(
            ICanExecuteChangedSubject observer)
        {
            this.observes.Add(observer);
            return this;
        }

        /// <summary>
        ///     Observeses the command manager.
        /// </summary>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.
            ObservesCommandManager() =>
            this.ObservesCommandManager();

        /// <summary>
        ///     Automatics the activate.
        /// </summary>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.
            AutoActivate() =>
            this.AutoActivate();

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.OnError(
            Action<Exception> error) =>
            this.OnError(error);

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.OnCompleted(
            Action completed)
        {
            return this.OnCompleted(completed);
        }

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.OnCancel(
            Action cancel) =>
            this.OnCancel(cancel);

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns>
        ///     Activatable Concurrency Sync Command.
        /// </returns>
        IActivatableConcurrencySyncCommand<T> IActivatableConcurrencySyncCanExecuteBuilder<T>.Build() =>
            this.BuildActivatable();

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCommandBuilder<T> IActivatableConcurrencySyncCommandBuilder<T>.OnError(
            Action<Exception> error) =>
            this.OnError(error);

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCommandBuilder<T> IActivatableConcurrencySyncCommandBuilder<T>.OnCompleted(
            Action completed) =>
            this.OnCompleted(completed);

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCommandBuilder<T> IActivatableConcurrencySyncCommandBuilder<T>.OnCancel(
            Action cancel) =>
            this.OnCancel(cancel);

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Command.
        /// </returns>
        IActivatableConcurrencySyncCommand<T> IActivatableConcurrencySyncCommandBuilder<T>.Build(
            Action<IActivatableConcurrencySyncCommand<T>> setCommand) =>
            this.BuildActivatable(setCommand);

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCommandBuilder<T>.CanExecute(
            Predicate<T> canExecute) =>
            this.CanExecute(canExecute);

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCommandBuilder<T>.ObservesCanExecute(
            Expression<Func<bool>> canExecute) =>
            this.ObservesCanExecute(canExecute);

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="fallback">if set to <c>true</c> [fallback].</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCommandBuilder<T>.ObservesCanExecute(
            Expression<Func<bool>> canExecute,
            bool fallback) =>
            this.ObservesCanExecute(canExecute, fallback);

        /// <summary>
        ///     Automatics the activate.
        /// </summary>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCommandBuilder<T>.AutoActivate() =>
            this.AutoActivate();

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns>
        ///     Activatable Concurrency Sync Command.
        /// </returns>
        IActivatableConcurrencySyncCommand<T> IActivatableConcurrencySyncCommandBuilder<T>.Build() =>
            this.BuildActivatable();

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IActivatableConcurrencySyncCommandBuilder<T>.CanExecute(
            ICanExecuteSubject canExecute) =>
            this.ActivatableCanExecute(canExecute);

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCanExecuteBuilder<T>.OnCompleted(Action completed) =>
            this.OnCompleted(completed);

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCanExecuteBuilder<T>.OnCancel(Action cancel) =>
            this.OnCancel(cancel);

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCanExecuteBuilder<T>.OnError(Action<Exception> error) =>
            this.OnError(error);

        /// <summary>
        ///     Observeses the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCanExecuteBuilder<T>.Observes(
            ICanExecuteChangedSubject observer)
        {
            this.observes.Add(observer);
            return this;
        }

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns>
        ///     Concurrency Sync Command.
        /// </returns>
        IConcurrencySyncCommand<T> IConcurrencySyncCanExecuteBuilder<T>.Build(
            Action<IConcurrencySyncCommand<T>> setCommand) =>
            this.Build(setCommand);

        /// <summary>
        ///     Activatables this instance.
        /// </summary>
        /// <returns>
        ///     Activatable Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCanExecuteBuilder<T>.Activatable() =>
            this.Activatable();

        /// <summary>
        ///     Observeses the property.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCanExecuteBuilder<T>.ObservesProperty<TType>(
            Expression<Func<TType>> expression) =>
            this.ObservesProperty(expression);

        /// <summary>
        ///     Observeses the command manager.
        /// </summary>
        /// <returns>
        ///     Concurrency Sync Command.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCanExecuteBuilder<T>.ObservesCommandManager() =>
            this.ObservesCommandManager();

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns>
        ///     Concurrency Sync Command.
        /// </returns>
        IConcurrencySyncCommand<T> IConcurrencySyncCanExecuteBuilder<T>.Build() => this.Build();

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns>
        ///     Concurrency Sync Command Builder.
        /// </returns>
        IConcurrencySyncCommandBuilder<T> IConcurrencySyncCommandBuilder<T>.OnCompleted(Action completed) =>
            this.OnCompleted(completed);

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns>
        ///     Concurrency Sync Command Builder.
        /// </returns>
        IConcurrencySyncCommandBuilder<T> IConcurrencySyncCommandBuilder<T>.OnCancel(Action cancel) =>
            this.OnCancel(cancel);

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>
        ///     Concurrency Sync Command Builder.
        /// </returns>
        IConcurrencySyncCommandBuilder<T> IConcurrencySyncCommandBuilder<T>.OnError(Action<Exception> error) =>
            this.OnError(error);

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns>
        ///     Concurrency Sync Command.
        /// </returns>
        IConcurrencySyncCommand<T> IConcurrencySyncCommandBuilder<T>.Build(
            Action<IConcurrencySyncCommand<T>> setCommand) =>
            this.Build(setCommand);

        /// <summary>
        ///     Activateables this instance.
        /// </summary>
        /// <returns>
        ///     Activatable Concurrency Sync Command Builder.
        /// </returns>
        IActivatableConcurrencySyncCommandBuilder<T> IConcurrencySyncCommandBuilder<T>.Activatable() =>
            this.Activatable();

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCommandBuilder<T>.CanExecute(Predicate<T> canExecute) =>
            this.CanExecute(canExecute);

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCommandBuilder<T>.ObservesCanExecute(
            Expression<Func<bool>> canExecute) =>
            this.ObservesCanExecute(canExecute);

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="fallback">if set to <c>true</c> [fallback].</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCommandBuilder<T>.ObservesCanExecute(
            Expression<Func<bool>> canExecute,
            bool fallback) =>
            this.ObservesCanExecute(canExecute, fallback);

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns>
        ///     Concurrency Sync Command.
        /// </returns>
        IConcurrencySyncCommand<T> IConcurrencySyncCommandBuilder<T>.Build() => this.Build();

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
        IConcurrencySyncCanExecuteBuilder<T> IConcurrencySyncCommandBuilder<T>.
            CanExecute(ICanExecuteSubject canExecute) =>
            this.CanExecute(canExecute);

        /// <summary>
        ///     Activatables the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>Activatable Concurrency Sync Can Execute Command Builder.</returns>
        private IActivatableConcurrencySyncCanExecuteBuilder<T> ActivatableCanExecute(ICanExecuteSubject canExecute)
        {
            this.canExecuteSubject = canExecute;
            return this;
        }

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>Concurrency Sync Command Builder.</returns>
        private ConcurrencySyncCommandBuilder<T> OnError(Action<Exception> error)
        {
            this.errorAction = error;
            return this;
        }

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns>Concurrency Sync Command Builder.</returns>
        private ConcurrencySyncCommandBuilder<T> OnCompleted(Action completed)
        {
            this.completedAction = completed;
            return this;
        }

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns>Concurrency Sync Command Builder.</returns>
        private ConcurrencySyncCommandBuilder<T> OnCancel(Action cancel)
        {
            this.cancelAction = cancel;
            return this;
        }

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>
        ///     Concurrency Sync Can Execute Command Builder.
        /// </returns>
#pragma warning disable S4144 // Methods should not have identical implementations
        private IConcurrencySyncCanExecuteBuilder<T> CanExecute(ICanExecuteSubject canExecute)
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            this.canExecuteSubject = canExecute;
            return this;
        }

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>Concurrency Sync Command Builder.</returns>
        /// <exception cref="CommandBuilderException">
        ///     Command Builder Exception.
        /// </exception>
        /// <exception cref="ArgumentNullException">canExecute is null.</exception>
        [NotNull]
        private ConcurrencySyncCommandBuilder<T> CanExecute([NotNull] Predicate<T> canExecute)
        {
            if (this.canExecuteFunction != null)
            {
                throw new CommandBuilderException(Resources.ExceptionStrings.CanExecuteFunctionAlreadyDefined);
            }

            if (this.canExecuteSubject != null)
            {
                throw new CommandBuilderException(Resources.ExceptionStrings.CanExecuteExpressionAlreadyDefined);
            }

            this.canExecuteFunction = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            return this;
        }

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns>Activatable Concurrency Sync Can Execute Command Builder.</returns>
        /// <exception cref="ArgumentNullException">setCommand is null.</exception>
        private ActivatableConcurrencyCanExecuteObserverCommand<T> BuildActivatable(
            [NotNull] Action<ActivatableConcurrencyCanExecuteObserverCommand<T>> setCommand)
        {
            if (setCommand == null)
            {
                throw new ArgumentNullException(nameof(setCommand));
            }

            var command = this.BuildActivatable();
            setCommand(command);
            return command;
        }

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns>Activatable Concurrency Sync Command Builder.</returns>
        /// <exception cref="NoCanExecuteException">No Can Execute Exception.</exception>
        [NotNull]
        private ActivatableConcurrencyCanExecuteObserverCommand<T> BuildActivatable()
        {
            if (this.observes.Any())
            {
                if (this.canExecuteFunction != null)
                {
                    return new ActivatableConcurrencyCanExecuteObserverCommand<T>(
                        this.execute,
                        this.isAutoActivate,
                        this.canExecuteFunction,
                        this.completedAction,
                        this.errorAction,
                        this.cancelAction,
                        this.observes.ToArray());
                }

                if (this.canExecuteSubject != null)
                {
                    return new ActivatableConcurrencyCanExecuteObserverCommand<T>(
                        this.execute,
                        this.isAutoActivate,
                        this.canExecuteSubject,
                        this.completedAction,
                        this.errorAction,
                        this.cancelAction,
                        this.observes.ToArray());
                }

                throw new NoCanExecuteException();
            }

            if (this.canExecuteFunction != null)
            {
                return new ActivatableConcurrencyCanExecuteObserverCommand<T>(
                    this.execute,
                    this.isAutoActivate,
                    this.canExecuteFunction,
                    this.completedAction,
                    this.errorAction,
                    this.cancelAction);
            }

            if (this.canExecuteSubject != null)
            {
                return new ActivatableConcurrencyCanExecuteObserverCommand<T>(
                    this.execute,
                    this.isAutoActivate,
                    this.canExecuteSubject,
                    this.completedAction,
                    this.errorAction,
                    this.cancelAction);
            }

            return new ActivatableConcurrencyCanExecuteObserverCommand<T>(
                this.execute,
                this.isAutoActivate,
                this.completedAction,
                this.errorAction,
                this.cancelAction);
        }

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns>Concurrency Sync Can Execute Command Builder.</returns>
        /// <exception cref="ArgumentNullException">setCommand is null.</exception>
        private ConcurrencyCanExecuteObserverCommand<T> Build(
            [NotNull] Action<ConcurrencyCanExecuteObserverCommand<T>> setCommand)
        {
            if (setCommand == null)
            {
                throw new ArgumentNullException(nameof(setCommand));
            }

            var command = this.Build();
            setCommand(command);
            return command;
        }

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns>Concurrency Sync Command Builder.</returns>
        /// <exception cref="NoCanExecuteException">No Can Execute Exception.</exception>
        [NotNull]
        private ConcurrencyCanExecuteObserverCommand<T> Build()
        {
            if (this.observes.Any())
            {
                if (this.canExecuteFunction != null)
                {
                    return new ConcurrencyCanExecuteObserverCommand<T>(
                        this.execute,
                        this.canExecuteFunction,
                        this.completedAction,
                        this.errorAction,
                        this.cancelAction,
                        this.observes.ToArray());
                }

                if (this.canExecuteSubject != null)
                {
                    return new ConcurrencyCanExecuteObserverCommand<T>(
                        this.execute,
                        this.canExecuteSubject,
                        this.completedAction,
                        this.errorAction,
                        this.cancelAction,
                        this.observes.ToArray());
                }

                throw new NoCanExecuteException();
            }

            if (this.canExecuteFunction != null)
            {
                return new ConcurrencyCanExecuteObserverCommand<T>(
                    this.execute,
                    this.canExecuteFunction,
                    this.completedAction,
                    this.errorAction,
                    this.cancelAction);
            }

            if (this.canExecuteSubject != null)
            {
                return new ConcurrencyCanExecuteObserverCommand<T>(
                    this.execute,
                    this.canExecuteSubject,
                    this.completedAction,
                    this.errorAction,
                    this.cancelAction);
            }

            return new ConcurrencyCanExecuteObserverCommand<T>(
                this.execute,
                this.completedAction,
                this.errorAction,
                this.cancelAction);
        }

        /// <summary>
        ///     Observeses the property.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>Concurrency Sync Command Builder.</returns>
        [NotNull]
        private ConcurrencySyncCommandBuilder<T> ObservesProperty<TType>([NotNull] Expression<Func<TType>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            this.observes.Add(new PropertyObserverFactory().ObservesProperty(expression));
            return this;
        }

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>Concurrency Sync Command Builder.</returns>
        /// <exception cref="ArgumentNullException">canExecute is null.</exception>
        /// <exception cref="CommandBuilderException">
        ///     Command Builder Exception.
        /// </exception>
        [NotNull]
        private ConcurrencySyncCommandBuilder<T> ObservesCanExecute([NotNull] Expression<Func<bool>> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            if (this.canExecuteSubject != null)
            {
                throw new CommandBuilderException(Resources.ExceptionStrings.CanExecuteExpressionAlreadyDefined);
            }

            if (this.canExecuteFunction != null)
            {
                throw new CommandBuilderException(Resources.ExceptionStrings.CanExecuteFunctionAlreadyDefined);
            }

            this.canExecuteSubject = CanExecuteObserver.Create(canExecute);
            return this;
        }

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="fallback">if set to <c>true</c> [fallback].</param>
        /// <returns>Concurrency Sync Command Builder.</returns>
        /// <exception cref="CommandBuilderException">
        ///     Command Builder Exception.
        /// </exception>
        [NotNull]
        private ConcurrencySyncCommandBuilder<T> ObservesCanExecute(
            [NotNull] Expression<Func<bool>> canExecute,
            bool fallback)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            if (this.canExecuteSubject != null)
            {
                throw new CommandBuilderException(Resources.ExceptionStrings.CanExecuteExpressionAlreadyDefined);
            }

            if (this.canExecuteFunction != null)
            {
                throw new CommandBuilderException(Resources.ExceptionStrings.CanExecuteFunctionAlreadyDefined);
            }

            this.canExecuteSubject = CanExecuteObserver.Create(canExecute, fallback);
            return this;
        }

        /// <summary>
        ///     Observeses the command manager.
        /// </summary>
        /// <returns>Concurrency Sync Command Builder.</returns>
        [NotNull]
        private ConcurrencySyncCommandBuilder<T> ObservesCommandManager()
        {
            if (this.observes.Contains(CommandManagerObserver.Observer))
            {
                throw new CommandBuilderException(Resources.ExceptionStrings.CanExecuteFunctionAlreadyDefined);
            }

            this.observes.Add(CommandManagerObserver.Observer);
            return this;
        }

        /// <summary>
        ///     Automatics the activate.
        /// </summary>
        /// <returns>Concurrency Sync Command Builder.</returns>
        [NotNull]
        private ConcurrencySyncCommandBuilder<T> AutoActivate()
        {
            this.isAutoActivate = true;
            return this;
        }

        /// <summary>
        ///     Activatables this instance.
        /// </summary>
        /// <returns>Concurrency Sync Command Builder.</returns>
        [NotNull]
        private ConcurrencySyncCommandBuilder<T> Activatable()
        {
            this.isAutoActivate = false;
            return this;
        }
    }
}