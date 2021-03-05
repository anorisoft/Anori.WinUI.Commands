﻿// -----------------------------------------------------------------------
// <copyright file="IActivatableConcurrencyAsyncCommand{T}.cs" company="Anorisoft">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.Interfaces
{
    using Anori.WinUI.Common;

    /// <summary>
    ///     Activatable Concurrency Async Command Interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.IConcurrencyAsyncCommand{T}" />
    /// <seealso cref="Anori.WinUI.Common.IActivatable{Anori.WinUI.Commands.Interfaces.IActivatableConcurrencyAsyncCommand{T}}" />
    public interface IActivatableConcurrencyAsyncCommand<in T> : IConcurrencyAsyncCommand<T>,
                                                                 IActivatable<IActivatableConcurrencyAsyncCommand<T>>
    {
    }
}