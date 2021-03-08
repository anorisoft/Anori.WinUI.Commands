﻿// -----------------------------------------------------------------------
// <copyright file="ConcurrencyCommandBase{T}.cs" company="AnoriSoft">
// Copyright (c) AnoriSoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.Commands
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Anori.WinUI.Commands.Interfaces;
    using Anori.WinUI.Common;

    using JetBrains.Annotations;

    /// <summary>
    ///     Asynchronous Relay Command.
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    /// <seealso cref="Anori.WinUI.Commands.Commands.CommandBase" />
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.ISyncCommand{T}" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.Windows.Input.ICommand" />
    /// <seealso cref="System.IDisposable" />
    internal abstract class ConcurrencyCommandBase<T> : CommandBase,
                                                        ISyncCommand<T>,
                                                        IDisposable,
                                                        INotifyPropertyChanged
    {
        /// <summary>
        ///     The cancel.
        /// </summary>
        [CanBeNull]
        private readonly Action cancel;

        /// <summary>
        ///     The cancel command.
        /// </summary>
        private readonly DirectCommand cancelCommand;

        /// <summary>
        ///     The can execute.
        /// </summary>
        [CanBeNull]
        private readonly Predicate<T> canExecute;

        /// <summary>
        ///     The completed.
        /// </summary>
        [CanBeNull]
        private readonly Action completed;

        /// <summary>
        ///     The error.
        /// </summary>
        [CanBeNull]
        private readonly Action<Exception> error;

        /// <summary>
        ///     The execute.
        /// </summary>
        [NotNull]
        private readonly Action<T, CancellationToken> execute;

        /// <summary>
        ///     The finally task scheduler.
        /// </summary>
        [NotNull]
        private readonly TaskScheduler finallyTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        ///     The post actions task scheduler.
        /// </summary>
        [NotNull]
        private readonly TaskScheduler postTaskScheduler = TaskScheduler.Default;

        /// <summary>
        ///     The task factory.
        /// </summary>
        [NotNull]
        private readonly TaskFactory taskFactory = new TaskFactory();

        /// <summary>
        ///     The actions task scheduler.
        /// </summary>
        [NotNull]
        private readonly TaskScheduler taskScheduler = TaskScheduler.Default;

        /// <summary>
        ///     The cancellation token source.
        /// </summary>
        [CanBeNull]
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        ///     The exception.
        /// </summary>
        [CanBeNull]
        private Exception exception;

        /// <summary>
        ///     The is execute.
        /// </summary>
        private bool isExecuting;

        /// <summary>
        ///     The task.
        /// </summary>
        [CanBeNull]
        private Task task;

        /// <summary>
        ///     The is canceled.
        /// </summary>
        private bool wasCanceled;

        /// <summary>
        ///     The has errors.
        /// </summary>
        private bool wasFaulty;

        /// <summary>
        ///     The was successfuly.
        /// </summary>
        private bool wasSuccessfuly;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConcurrencyCommandBase{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        protected ConcurrencyCommandBase(
            [NotNull] Action<T, CancellationToken> execute,
            [NotNull] Predicate<T> canExecute,
            [CanBeNull] Action completed = null,
            [CanBeNull] Action<Exception> error = null,
            [CanBeNull] Action cancel = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.completed = completed;
            this.error = error;
            this.cancel = cancel;
            this.cancelCommand = new DirectCommand(this.Cancel, () => this.IsExecuting);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConcurrencyCommandBase{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        protected ConcurrencyCommandBase(
            [NotNull] Action<T, CancellationToken> execute,
            [CanBeNull] Action completed = null,
            [CanBeNull] Action<Exception> error = null,
            [CanBeNull] Action cancel = null)
        {
            this.execute = execute;
            this.completed = completed;
            this.error = error;
            this.cancel = cancel;
            this.cancelCommand = new DirectCommand(this.Cancel, () => this.IsExecuting);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConcurrencyCommandBase{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecuteSubject">The can execute subject.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        /// <exception cref="ArgumentNullException">canExecuteSubject is null.</exception>
        protected ConcurrencyCommandBase(
            [NotNull] Action<T, CancellationToken> execute,
            [NotNull] ICanExecuteSubject canExecuteSubject,
            [CanBeNull] Action completed = null,
            [CanBeNull] Action<Exception> error = null,
            [CanBeNull] Action cancel = null)
        {
            this.execute = execute;
            if (canExecuteSubject == null)
            {
                throw new ArgumentNullException(nameof(canExecuteSubject));
            }

            this.canExecute = t => canExecuteSubject.CanExecute();
            this.completed = completed;
            this.error = error;
            this.cancel = cancel;
            this.cancelCommand = new DirectCommand(this.Cancel, () => this.IsExecuting);
        }

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets the cancel command.
        /// </summary>
        /// <value>
        ///     The cancel command.
        /// </value>
        public ISyncCommand CancelCommand => this.cancelCommand;

        /// <summary>
        ///     Gets the exception.
        /// </summary>
        /// <value>
        ///     The exception.
        /// </value>
        public Exception Exception
        {
            get => this.exception;
            private set => this.SetProperty(ref this.exception, value);
        }

        /// <summary>
        ///     Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool WasFaulty
        {
            get => this.wasFaulty;
            private set => this.SetProperty(ref this.wasFaulty, value);
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is executing.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is executing; otherwise, <c>false</c>.
        /// </value>
        public bool IsExecuting
        {
            get => this.isExecuting;
            private set => this.SetProperty(ref this.isExecuting, value);
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is canceled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is canceled; otherwise, <c>false</c>.
        /// </value>
        public bool WasCanceled
        {
            get => this.wasCanceled;
            private set => this.SetProperty(ref this.wasCanceled, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [was successful].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [was successful]; otherwise, <c>false</c>.
        /// </value>
        public bool WasSuccessfuly
        {
            get => this.wasSuccessfuly;
            set => this.SetProperty(ref this.wasSuccessfuly, value);
        }

        /// <summary>
        ///     Gets a value indicating whether this instance has can execute.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has can execute; otherwise, <c>false</c>.
        /// </value>
        protected override bool HasCanExecute => this.canExecute != null;

        /// <summary>
        ///     Determines whether this instance can execute.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///     <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute(T parameter)
        {
            if (this.IsExecuting)
            {
                return false;
            }

            if (this.canExecute != null && !this.canExecute(parameter))
            {
                return false;
            }

            if (this.task == null)
            {
                return true;
            }

            if (this.task.IsFinished())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Execute(T parameter)
        {
            try
            {
                this.OnBegin();
                this.task?.Dispose();
                this.cancellationTokenSource = new CancellationTokenSource();
                var token = this.cancellationTokenSource.Token;
                this.task = this.taskFactory
                    .StartNew(
                        () => this.OnAction(parameter, token),
                        token,
                        TaskCreationOptions.DenyChildAttach,
                        this.taskScheduler)
                    .ContinueWith(this.OnPostAction, this.postTaskScheduler)
                    .ContinueWith(t => this.OnFinally(), this.finallyTaskScheduler);
            }
            catch (TaskCanceledException ex)
            {
                this.OnFinally();
                Debug.WriteLine(ex);
            }
            catch (AggregateException ex)
            {
                this.OnFinally();
                foreach (var e in ex.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.OnFinally();
                throw;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Cancels this instance.
        /// </summary>
        public void Cancel() => this.cancellationTokenSource?.Cancel();

        /// <summary>
        ///     Raises the can execute cancel command.
        /// </summary>
        public void RaiseCanExecuteCancelCommand() => this.cancelCommand.RaiseCanExecuteChanged();

        /// <summary>
        ///     Raises the can execute command.
        /// </summary>
        public abstract void RaiseCanExecuteCommand();

        /// <summary>
        ///     Executes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="token">The token.</param>
        public void Execute(T parameter, CancellationToken token) => this.execute(parameter, token);

        /// <summary>
        ///     Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///     <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
        /// </returns>
        protected sealed override bool CanExecute(object parameter) => this.CanExecute((T)parameter);

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.cancellationTokenSource != null)
                {
                    this.cancellationTokenSource.Cancel();
                    this.cancellationTokenSource.Dispose();
                    this.cancellationTokenSource = null;
                }

                if (this.task != null)
                {
                    this.task.Dispose();
                    this.task = null;
                }
            }
        }

        /// <summary>
        ///     Handle the internal invocation of <see cref="ICommand.Execute(object)" />.
        /// </summary>
        /// <param name="parameter">Command Parameter.</param>
        protected sealed override void Execute(object parameter) => this.Execute((T)parameter);

        /// <summary>
        ///     Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        ///     Sets the property.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="storage">The storage.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     Result of SetProperty as Boolean.
        /// </returns>
        [NotifyPropertyChangedInvocator]
        protected bool SetProperty<TValue>(
            ref TValue storage,
            TValue value,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     Called when [execute].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="token">The token.</param>
        private void OnAction(T parameter, CancellationToken token) => this.execute(parameter, token);

        /// <summary>
        ///     Called when [begin].
        /// </summary>
        private void OnBegin()
        {
            Debug.WriteLine("OnBegin");
            this.WasCanceled = false;
            this.WasFaulty = false;
            this.WasSuccessfuly = false;
            this.Exception = null;
            this.IsExecuting = true;
            this.RaiseCanExecuteCancelCommand();
            this.RaiseCanExecuteCommand();
        }

        /// <summary>
        ///     Called when [canceled].
        /// </summary>
        private void OnCanceled()
        {
            Debug.WriteLine("OnCanceled");
            this.WasCanceled = true;
            this.cancel.Raise();
        }

        /// <summary>
        ///     Called when [faulted].
        /// </summary>
        /// <param name="ex">The exception.</param>
        private void OnFaulted(Exception ex)
        {
            Debug.WriteLine("OnFaulted");
            this.WasFaulty = true;
            this.Exception = ex;
            this.error.Raise(ex);
        }

        /// <summary>
        ///     Called when [finally].
        /// </summary>
        private void OnFinally()
        {
            Debug.WriteLine("OnFinally");
            this.IsExecuting = false;
            this.RaiseCanExecuteCancelCommand();
            this.RaiseCanExecuteCommand();
        }

        /// <summary>
        ///     Called when [post execute].
        /// </summary>
        /// <param name="t">The t.</param>
        private void OnPostAction(Task t)
        {
            switch (t.Status)
            {
                case TaskStatus.Canceled:
                    this.OnCanceled();
                    break;

                case TaskStatus.Faulted:
                    this.OnFaulted(t.Exception);
                    break;

                case TaskStatus.RanToCompletion:
                    this.OnRanToCompletion();
                    break;
            }
        }

        /// <summary>
        ///     Called when [ran to completion].
        /// </summary>
        private void OnRanToCompletion()
        {
            Debug.WriteLine("OnRanToCompletion");
            this.WasSuccessfuly = true;
            this.completed.Raise();
        }
    }
}