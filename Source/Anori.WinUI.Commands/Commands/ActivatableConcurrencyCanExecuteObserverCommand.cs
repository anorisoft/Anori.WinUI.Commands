﻿// -----------------------------------------------------------------------
// <copyright file="ActivatableConcurrencyCanExecuteObserverCommand.cs" company="AnoriSoft">
// Copyright (c) AnoriSoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Anori.WinUI.Commands.Interfaces;
    using Anori.WinUI.Common;

    using JetBrains.Annotations;

    /// <summary>
    ///     Activatable Concurrency CanExecute Observer Command.
    /// </summary>
    /// <seealso cref="Anori.WinUI.Commands.Commands.ConcurrencyCommandBase" />
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.IActivatableConcurrencySyncCommand" />
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.ICanExecuteChangedObserver" />
    internal sealed class ActivatableConcurrencyCanExecuteObserverCommand : ConcurrencyCommandBase,
                                                                            IActivatableConcurrencySyncCommand,
                                                                            ICanExecuteChangedObserver
    {
        /// <summary>
        ///     The observers.
        /// </summary>
        private readonly List<ICanExecuteChangedSubjectBase> observers = new List<ICanExecuteChangedSubjectBase>();

        /// <summary>
        ///     The is active.
        /// </summary>
        private bool isActive;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="autoActivate">if set to <c>true</c> [automatic activate].</param>
        /// <param name="observers">The observers.</param>
        /// <exception cref="ArgumentNullException">
        ///     observer
        ///     or
        ///     observer is null.
        /// </exception>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            bool autoActivate,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : base(execute)
        {
            if (observers == null)
            {
                throw new ArgumentNullException(nameof(observers));
            }

            this.observers.AddIfNotContains(observers);

            if (autoActivate)
            {
                this.Activate();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="autoActivate">if set to <c>true</c> [automatic activate].</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        /// <param name="observers">The observers.</param>
        /// <exception cref="ArgumentNullException">observers is null.</exception>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            bool autoActivate,
            [CanBeNull] Action completed,
            [CanBeNull] Action<Exception> error,
            [CanBeNull] Action cancel,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : base(execute, completed, error, cancel)
        {
            if (observers == null)
            {
                throw new ArgumentNullException(nameof(observers));
            }

            this.observers.AddIfNotContains(observers);

            if (autoActivate)
            {
                this.Activate();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="autoActivate">if set to <c>true</c> [automatic activate].</param>
        /// <param name="canExecuteSubject">The can execute subject.</param>
        /// <param name="observers">The observers.</param>
        /// <exception cref="ArgumentNullException">
        ///     canExecuteSubject
        ///     or
        ///     observers is null.
        /// </exception>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            bool autoActivate,
            [NotNull] ICanExecuteSubject canExecuteSubject,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : base(execute, canExecuteSubject)
        {
            if (canExecuteSubject == null)
            {
                throw new ArgumentNullException(nameof(canExecuteSubject));
            }

            if (observers == null)
            {
                throw new ArgumentNullException(nameof(observers));
            }

            this.observers.Add(canExecuteSubject);

            this.observers.AddIfNotContains(observers);

            if (autoActivate)
            {
                this.Activate();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="autoActivate">if set to <c>true</c> [automatic activate].</param>
        /// <param name="canExecuteSubject">The can execute subject.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        /// <param name="observers">The observers.</param>
        /// <exception cref="ArgumentNullException">
        ///     canExecuteSubject
        ///     or
        ///     observers is null.
        /// </exception>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            bool autoActivate,
            [NotNull] ICanExecuteSubject canExecuteSubject,
            [CanBeNull] Action completed,
            [CanBeNull] Action<Exception> error,
            [CanBeNull] Action cancel,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : base(execute, canExecuteSubject, completed, error, cancel)
        {
            if (canExecuteSubject == null)
            {
                throw new ArgumentNullException(nameof(canExecuteSubject));
            }

            if (observers == null)
            {
                throw new ArgumentNullException(nameof(observers));
            }

            this.observers.Add(canExecuteSubject);

            this.observers.AddIfNotContains(observers);

            if (autoActivate)
            {
                this.Activate();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public ActivatableConcurrencyCanExecuteObserverCommand([NotNull] Action<CancellationToken> execute)
            : this(execute, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            [CanBeNull] Action completed,
            [CanBeNull] Action<Exception> error,
            [CanBeNull] Action cancel)
            : this(execute, false, completed, error, cancel)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecuteSubject">The can execute subject.</param>
        /// <param name="observers">The observers.</param>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            [NotNull] ICanExecuteSubject canExecuteSubject,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : this(execute, false, canExecuteSubject, observers)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecuteSubject">The can execute subject.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        /// <param name="observers">The observers.</param>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            [NotNull] ICanExecuteSubject canExecuteSubject,
            [CanBeNull] Action completed,
            [CanBeNull] Action<Exception> error,
            [CanBeNull] Action cancel,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : this(execute, false, canExecuteSubject, completed, error, cancel, observers)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecuteSubject">The can execute subject.</param>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            [NotNull] ICanExecuteSubject canExecuteSubject)
            : this(execute, false, canExecuteSubject)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecuteSubject">The can execute subject.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            [NotNull] ICanExecuteSubject canExecuteSubject,
            [CanBeNull] Action completed,
            [CanBeNull] Action<Exception> error,
            [CanBeNull] Action cancel)
            : this(execute, false, canExecuteSubject, completed, error, cancel)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        /// <param name="observers">The observers.</param>
        /// <exception cref="ArgumentNullException">
        ///     observers
        ///     or
        ///     observers is null.
        /// </exception>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            [NotNull] Func<bool> canExecute,
            [CanBeNull] Action completed,
            [CanBeNull] Action<Exception> error,
            [CanBeNull] Action cancel,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : this(execute, false, canExecute, completed, error, cancel, observers)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="autoActivate">if set to <c>true</c> [automatic activate].</param>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="observers">The observers.</param>
        /// <exception cref="ArgumentNullException">observers is null.</exception>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            bool autoActivate,
            [NotNull] Func<bool> canExecute,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : base(execute, canExecute)
        {
            if (observers == null)
            {
                throw new ArgumentNullException(nameof(observers));
            }

            this.observers.AddIfNotContains(observers);

            if (autoActivate)
            {
                this.Activate();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatableConcurrencyCanExecuteObserverCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="autoActivate">if set to <c>true</c> [automatic activate].</param>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="completed">The completed.</param>
        /// <param name="error">The error.</param>
        /// <param name="cancel">The cancel.</param>
        /// <param name="observers">The observers.</param>
        /// <exception cref="ArgumentNullException">observers is null.</exception>
        public ActivatableConcurrencyCanExecuteObserverCommand(
            [NotNull] Action<CancellationToken> execute,
            bool autoActivate,
            [NotNull] Func<bool> canExecute,
            [CanBeNull] Action completed,
            [CanBeNull] Action<Exception> error,
            [CanBeNull] Action cancel,
            [NotNull] [ItemNotNull] params ICanExecuteChangedSubject[] observers)
            : base(execute, canExecute, completed, error, cancel)
        {
            if (observers == null)
            {
                throw new ArgumentNullException(nameof(observers));
            }

            this.observers.AddIfNotContains(observers);

            if (autoActivate)
            {
                this.Activate();
            }
        }

        /// <summary>
        ///     Notifies that the value for <see cref="P:Anori.WinUI.Common.IActivated.IsActive" /> property has changed.
        /// </summary>
        public event EventHandler<EventArgs<bool>> IsActiveChanged;

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public override event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Gets a value indicating whether the object is active.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the object is active; otherwise <see langword="false" />.
        /// </value>
        public bool IsActive
        {
            get => this.isActive;
            private set
            {
                if (this.isActive == value)
                {
                    return;
                }

                this.isActive = value;
                this.IsActiveChanged.Raise(this, value);
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Activates this instance.
        /// </summary>
        /// <returns>Activatable Concurrency Sync Command.</returns>
        IActivatableConcurrencySyncCommand IActivatable<IActivatableConcurrencySyncCommand>.Activate() =>
            this.Activate();

        /// <summary>
        ///     Deactivates this instance.
        /// </summary>
        /// <returns>
        ///     Activatable Concurrency Sync Command.
        /// </returns>
        IActivatableConcurrencySyncCommand IActivatable<IActivatableConcurrencySyncCommand>.Deactivate() =>
            this.Deactivate();

        /// <summary>
        ///     Called when [can execute changed].
        /// </summary>
        public void RaisePropertyChanged() => this.CanExecuteChanged.RaiseEmpty(this);

        /// <summary>
        ///     Activates this instance.
        /// </summary>
        /// <returns>Activatable Concurrency CanExecute Observer Command.</returns>
        public ActivatableConcurrencyCanExecuteObserverCommand Activate()
        {
            if (this.IsActive)
            {
                return this;
            }

            this.Subscribe();
            this.IsActive = true;
            return this;
        }

        /// <summary>
        ///     Deactivates this instance.
        /// </summary>
        /// <returns>Activatable Concurrency CanExecute Observer Command.</returns>
        public ActivatableConcurrencyCanExecuteObserverCommand Deactivate()
        {
            if (!this.IsActive)
            {
                return this;
            }

            this.Unsubscribe();
            this.IsActive = false;
            return this;
        }

        /// <summary>
        ///     Raises the can execute command.
        /// </summary>
        public override void RaiseCanExecuteCommand() => this.RaisePropertyChanged();

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.Unsubscribe();
            }
        }

        /// <summary>
        ///     Subscribes this instance.
        /// </summary>
        private void Subscribe() => this.observers.ForEach(observer => observer.Add(this));

        /// <summary>
        ///     Unsubscribes this instance.
        /// </summary>
        private void Unsubscribe() => this.observers.ForEach(observer => observer.Remove(this));
    }
}