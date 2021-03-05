﻿// -----------------------------------------------------------------------
// <copyright file="ConcurrencyAsyncCommandBuilder.cs" company="AnoriSoft">
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
    using System.Threading.Tasks;

    using Anori.WinUI.Commands.CanExecuteObservers;
    using Anori.WinUI.Commands.Commands;
    using Anori.WinUI.Commands.Exceptions;
    using Anori.WinUI.Commands.Interfaces;
    using Anori.WinUI.Commands.Interfaces.Builders;

    using JetBrains.Annotations;

    public sealed class ConcurrencyAsyncCommandBuilder : IConcurrencyAsyncCommandBuilder,
                                                         IConcurrencyAsyncCanExecuteBuilder,
                                                         IActivatableConcurrencyAsyncCommandBuilder,
                                                         IActivatableConcurrencyAsyncCanExecuteBuilder
    {
        /// <summary>
        ///     The execute
        /// </summary>
        private readonly Func<CancellationToken, Task> execute;

        /// <summary>
        ///     The observes
        /// </summary>
        private readonly List<ICanExecuteChangedSubject> observes = new List<ICanExecuteChangedSubject>();

        /// <summary>
        ///     The cancel action
        /// </summary>
        private Func<Task> cancelAction;

        /// <summary>
        ///     The can execute function
        /// </summary>
        private Func<bool> canExecuteFunction;

        /// <summary>
        ///     The can execute expression
        /// </summary>
        private ICanExecuteSubject canExecuteSubject;

        /// <summary>
        ///     The completed action
        /// </summary>
        private Func<Task> completedAction;

        /// <summary>
        ///     The error action
        /// </summary>
        private Func<Exception, Task> errorAction;

        /// <summary>
        ///     The is automatic actiate
        /// </summary>
        private bool isAutoActiate;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SyncCommandBuilder" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <exception cref="ArgumentNullException">execute</exception>
        public ConcurrencyAsyncCommandBuilder([NotNull] Func<CancellationToken, Task> execute) =>
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCommand IActivatableConcurrencyAsyncCanExecuteBuilder.Build(
            Action<IActivatableConcurrencyAsyncCommand> setCommand) =>
            this.BuildActivatable(setCommand);

        /// <summary>
        ///     Observeses the property.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCanExecuteBuilder.
            ObservesProperty<TType>(Expression<Func<TType>> expression) =>
            this.ObservesProperty(expression);

        /// <summary>
        ///     Observeses the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCanExecuteBuilder.Observes(
            ICanExecuteChangedSubject observer)
        {
            this.observes.Add(observer);
            return this;
        }

        /// <summary>
        ///     Observeses the command manager.
        /// </summary>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCanExecuteBuilder.
            ObservesCommandManager() =>
            this.ObservesCommandManager();

        /// <summary>
        ///     Automatics the activate.
        /// </summary>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCanExecuteBuilder.AutoActivate() =>
            this.AutoActivate();

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCanExecuteBuilder.OnError(
            Func<Exception, Task> error) =>
            this.OnError(error);

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCanExecuteBuilder.OnCompleted(
            Func<Task> completed) =>
            this.OnCompleted(completed);

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCanExecuteBuilder.OnCancel(
            Func<Task> cancel) =>
            this.OnCancel(cancel);

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCommand IActivatableConcurrencyAsyncCanExecuteBuilder.Build() =>
            this.BuildActivatable();

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCommandBuilder IActivatableConcurrencyAsyncCommandBuilder.OnError(
            Func<Exception, Task> error) =>
            this.OnError(error);

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCommandBuilder IActivatableConcurrencyAsyncCommandBuilder.OnCompleted(
            Func<Task> completed) =>
            this.OnCompleted(completed);

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCommandBuilder IActivatableConcurrencyAsyncCommandBuilder.OnCancel(
            Func<Task> cancel) =>
            this.OnCancel(cancel);

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCommand IActivatableConcurrencyAsyncCommandBuilder.Build() =>
            this.BuildActivatable();

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCommand IActivatableConcurrencyAsyncCommandBuilder.Build(
            Action<IActivatableConcurrencyAsyncCommand> setCommand) =>
            this.BuildActivatable(setCommand);

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCommandBuilder.CanExecute(
            Func<bool> canExecute) =>
            this.CanExecute(canExecute);

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCommandBuilder.CanExecute(
            ICanExecuteSubject canExecute)
        {
            this.canExecuteSubject = canExecute;
            return this;
        }

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCommandBuilder.ObservesCanExecute(
            Expression<Func<bool>> canExecute) =>
            this.ObservesCanExecute(canExecute);

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="fallback">if set to <c>true</c> [fallback].</param>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCommandBuilder.ObservesCanExecute(
            Expression<Func<bool>> canExecute,
            bool fallback) =>
            this.ObservesCanExecute(canExecute, fallback);

        /// <summary>
        ///     Automatics the activate.
        /// </summary>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IActivatableConcurrencyAsyncCommandBuilder.AutoActivate() =>
            this.AutoActivate();

        /// <summary>
        ///     Observeses the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCanExecuteBuilder.Observes(
            ICanExecuteChangedSubject observer)
        {
            this.observes.Add(observer);
            return this;
        }

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns></returns>
        IConcurrencyAsyncCommand IConcurrencyAsyncCanExecuteBuilder.
            Build(Action<IConcurrencyAsyncCommand> setCommand) =>
            this.Build(setCommand);

        /// <summary>
        ///     Observeses the property.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCanExecuteBuilder.ObservesProperty<TType>(
            Expression<Func<TType>> expression) =>
            this.ObservesProperty(expression);

        /// <summary>
        ///     Observeses the command manager.
        /// </summary>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCanExecuteBuilder.ObservesCommandManager() =>
            this.ObservesCommandManager();

        /// <summary>
        ///     Activatables this instance.
        /// </summary>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCanExecuteBuilder.Activatable() =>
            this.Activatable();

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCanExecuteBuilder.OnError(Func<Exception, Task> error) =>
            this.OnError(error);

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCanExecuteBuilder.OnCompleted(Func<Task> completed) =>
            this.OnCompleted(completed);

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCanExecuteBuilder.OnCancel(Func<Task> cancel) =>
            this.OnCancel(cancel);

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns></returns>
        IConcurrencyAsyncCommand IConcurrencyAsyncCanExecuteBuilder.Build() => this.Build();

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCommandBuilder.CanExecute(ICanExecuteSubject canExecute)
        {
            this.canExecuteSubject = canExecute;
            return this;
        }

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        IConcurrencyAsyncCommandBuilder IConcurrencyAsyncCommandBuilder.OnError(Func<Exception, Task> error) =>
            this.OnError(error);

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns></returns>
        IConcurrencyAsyncCommandBuilder IConcurrencyAsyncCommandBuilder.OnCompleted(Func<Task> completed) =>
            this.OnCompleted(completed);

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns></returns>
        IConcurrencyAsyncCommandBuilder IConcurrencyAsyncCommandBuilder.OnCancel(Func<Task> cancel) =>
            this.OnCancel(cancel);

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns></returns>
        IConcurrencyAsyncCommand IConcurrencyAsyncCommandBuilder.Build() => this.Build();

        /// <summary>
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns></returns>
        IConcurrencyAsyncCommand IConcurrencyAsyncCommandBuilder.Build(Action<IConcurrencyAsyncCommand> setCommand) =>
            this.Build(setCommand);

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCommandBuilder.CanExecute(Func<bool> canExecute) =>
            this.CanExecute(canExecute);

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCommandBuilder.ObservesCanExecute(
            Expression<Func<bool>> canExecute) =>
            this.ObservesCanExecute(canExecute);

        /// <summary>
        ///     Observeses the can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="fallback">if set to <c>true</c> [fallback].</param>
        /// <returns></returns>
        IConcurrencyAsyncCanExecuteBuilder IConcurrencyAsyncCommandBuilder.ObservesCanExecute(
            Expression<Func<bool>> canExecute,
            bool fallback) =>
            this.ObservesCanExecute(canExecute, fallback);

        /// <summary>
        ///     Activatables this instance.
        /// </summary>
        /// <returns></returns>
        IActivatableConcurrencyAsyncCommandBuilder IConcurrencyAsyncCommandBuilder.Activatable() => this.Activatable();

        /// <summary>
        ///     Determines whether this instance can execute the specified can execute.
        /// </summary>
        /// <param name="canExecute">The can execute.</param>
        /// <returns></returns>
        /// <exception cref="CommandBuilderException">
        /// </exception>
        [NotNull]
        public ConcurrencyAsyncCommandBuilder CanExecute([NotNull] Func<bool> canExecute)
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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">setCommand</exception>
        [NotNull]
        private ActivatableConcurrencyAsyncCanExecuteObserverCommand BuildActivatable(
            [NotNull] Action<ActivatableConcurrencyAsyncCanExecuteObserverCommand> setCommand)
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
        ///     Builds the specified set command.
        /// </summary>
        /// <param name="setCommand">The set command.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">setCommand</exception>
        [NotNull]
        private ConcurrencyAsyncCanExecuteObserverCommand Build(
            [NotNull] Action<ConcurrencyAsyncCanExecuteObserverCommand> setCommand)
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
        /// <returns></returns>
        [NotNull]
        private ActivatableConcurrencyAsyncCanExecuteObserverCommand BuildActivatable()
        {
            if (this.observes.Any())
            {
                if (this.canExecuteFunction != null)
                {
                    return new ActivatableConcurrencyAsyncCanExecuteObserverCommand(
                        this.execute,
                        this.isAutoActiate,
                        this.canExecuteFunction,
                        this.completedAction,
                        this.errorAction,
                        this.cancelAction,
                        this.observes.ToArray());
                }

                if (this.canExecuteSubject != null)
                {
                    return new ActivatableConcurrencyAsyncCanExecuteObserverCommand(
                        this.execute,
                        this.isAutoActiate,
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
                return new ActivatableConcurrencyAsyncCanExecuteObserverCommand(
                    this.execute,
                    this.isAutoActiate,
                    this.canExecuteFunction,
                    this.completedAction,
                    this.errorAction,
                    this.cancelAction);
            }

            if (this.canExecuteSubject != null)
            {
                return new ActivatableConcurrencyAsyncCanExecuteObserverCommand(
                    this.execute,
                    this.isAutoActiate,
                    this.canExecuteSubject,
                    this.completedAction,
                    this.errorAction,
                    this.cancelAction);
            }

            return new ActivatableConcurrencyAsyncCanExecuteObserverCommand(
                this.execute,
                this.isAutoActiate,
                this.completedAction,
                this.errorAction,
                this.cancelAction);
        }

        /// <summary>
        ///     Builds this instance.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        private ConcurrencyAsyncCanExecuteObserverCommand Build()
        {
            if (this.observes.Any())
            {
                if (this.canExecuteFunction != null)
                {
                    return new ConcurrencyAsyncCanExecuteObserverCommand(
                        this.execute,
                        this.canExecuteFunction,
                        this.completedAction,
                        this.errorAction,
                        this.cancelAction,
                        this.observes.ToArray());
                }

                if (this.canExecuteSubject != null)
                {
                    return new ConcurrencyAsyncCanExecuteObserverCommand(
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
                return new ConcurrencyAsyncCanExecuteObserverCommand(
                    this.execute,
                    this.canExecuteFunction,
                    this.completedAction,
                    this.errorAction,
                    this.cancelAction);
            }

            if (this.canExecuteSubject != null)
            {
                return new ConcurrencyAsyncCanExecuteObserverCommand(
                    this.execute,
                    this.canExecuteSubject,
                    this.completedAction,
                    this.errorAction,
                    this.cancelAction);
            }

            return new ConcurrencyAsyncCanExecuteObserverCommand(
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
        /// <returns></returns>
        [NotNull]
        private ConcurrencyAsyncCommandBuilder ObservesProperty<TType>([NotNull] Expression<Func<TType>> expression)
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
        /// <returns></returns>
        /// <exception cref="CommandBuilderException">
        /// </exception>
        [NotNull]
        private ConcurrencyAsyncCommandBuilder ObservesCanExecute([NotNull] Expression<Func<bool>> canExecute)
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
        /// <returns></returns>
        /// <exception cref="CommandBuilderException">
        /// </exception>
        [NotNull]
        private ConcurrencyAsyncCommandBuilder ObservesCanExecute(
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
        /// <returns></returns>
        [NotNull]
        private ConcurrencyAsyncCommandBuilder ObservesCommandManager()
        {
            if (this.observes.Contains(CommandManagerObserver.Observer))
            {
                throw new CommandBuilderException(Resources.ExceptionStrings.CanExecuteFunctionAlreadyDefined);
            }

            this.observes.Add(CommandManagerObserver.Observer);
            return this;
        }

        /// <summary>
        ///     Activatables this instance.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        private ConcurrencyAsyncCommandBuilder Activatable() => this;

        /// <summary>
        ///     Automatics the activate.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        private ConcurrencyAsyncCommandBuilder AutoActivate()
        {
            this.isAutoActiate = true;
            return this;
        }

        /// <summary>
        ///     Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        private ConcurrencyAsyncCommandBuilder OnError(Func<Exception, Task> error)
        {
            this.errorAction = error;
            return this;
        }

        /// <summary>
        ///     Called when [completed].
        /// </summary>
        /// <param name="completed">The completed.</param>
        /// <returns></returns>
        private ConcurrencyAsyncCommandBuilder OnCompleted(Func<Task> completed)
        {
            this.completedAction = completed;
            return this;
        }

        /// <summary>
        ///     Called when [cancel].
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <returns></returns>
        private ConcurrencyAsyncCommandBuilder OnCancel(Func<Task> cancel)
        {
            this.cancelAction = cancel;
            return this;
        }
    }
}