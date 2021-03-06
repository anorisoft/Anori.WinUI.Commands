﻿// -----------------------------------------------------------------------
// <copyright file="ICanExecuteSubject.cs" company="AnoriSoft">
// Copyright (c) AnoriSoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.Interfaces
{
    /// <summary>
    ///     CanExecute Subject Interface.
    /// </summary>
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.ICanExecuteChangedSubjectBase" />
    /// <seealso cref="Anori.WinUI.Commands.Interfaces.ICanExecute" />
    public interface ICanExecuteSubject : ICanExecuteChangedSubjectBase, ICanExecute
    {
    }
}