﻿// -----------------------------------------------------------------------
// <copyright file="ICanExecute.cs" company="Anorisoft">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Anori.WinUI.Commands.Interfaces
{
    using JetBrains.Annotations;

    /// <summary>
    ///     CanExecute Interface.
    /// </summary>
    public interface ICanExecute
    {
        /// <summary>
        ///     Gets the can execute.
        /// </summary>
        /// <value>
        ///     The can execute.
        /// </value>
        [NotNull]
        Func<bool> CanExecute { get; }
    }
}