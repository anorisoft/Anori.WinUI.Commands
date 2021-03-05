﻿// -----------------------------------------------------------------------
// <copyright file="IActivatableAsyncCommand{T}.cs" company="Anorisoft">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.Interfaces
{
    using Anori.WinUI.Common;

    /// <summary>
    ///     Activatable Async Command Interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.IAsyncCommand{T}" />
    /// <seealso cref="Anori.WinUI.Common.IActivatable{Anori.WinUI.Commands.Interfaces.IActivatableAsyncCommand{T}}" />
    public interface IActivatableAsyncCommand<in T> : IAsyncCommand<T>, IActivatable<IActivatableAsyncCommand<T>>
    {
    }
}