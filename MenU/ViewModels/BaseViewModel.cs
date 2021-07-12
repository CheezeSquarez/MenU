﻿using System.ComponentModel;
using System.Windows.Input;
using MenU.Services;
using Xamarin.Forms;
using MenU.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Text;
using System.Collections.ObjectModel;
using MenU.Views;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace MenU.ViewModels
{
    abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(
               [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingField,
              T value,
              [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(
                         backingField, value)) return;
            backingField = value;
            OnPropertyChanged(propertyName);
        }
    }
}
