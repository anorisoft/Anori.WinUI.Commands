﻿// -----------------------------------------------------------------------
// <copyright file="PropertyObservableTestViewModel.cs" company="AnoriSoft">
// Copyright (c) AnoriSoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WinUI.Commands.GUITest
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Anori.WinUI.Commands.Builder;

    using JetBrains.Annotations;

    public abstract class PropertyObservableTestViewModelBase : INotifyPropertyChanged
    {
        private bool condition1;

        private bool condition2;

      

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand TestAndCommand { get; protected set; }

        public ICommand TestOrCommand { get; protected set; }

        public bool Condition1
        {
            get => this.condition1;
            set
            {
                if (value == this.condition1)
                {
                    return;
                }

                this.condition1 = value;
                this.OnPropertyChanged();
            }
        }

        public bool Condition2
        {
            get => this.condition2;
            set
            {
                if (value == this.condition2)
                {
                    return;
                }

                this.condition2 = value;
                this.OnPropertyChanged();
            }
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}