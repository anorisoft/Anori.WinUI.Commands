﻿// -----------------------------------------------------------------------
// <copyright file="IActivatableSyncCommand.cs" company="Anorisoft">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.Interfaces
{
    using Anori.WinUI.Common;

    /// <summary>
    ///     Activatable Sync Command Interface.
    /// </summary>
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.ISyncCommand" />
    /// <seealso cref="Anori.WinUI.Common.IActivatable{Anori.WinUI.Commands.Interfaces.IActivatableSyncCommand}" />
    public interface IActivatableSyncCommand : ISyncCommand, IActivatable<IActivatableSyncCommand>
    {
    }
}