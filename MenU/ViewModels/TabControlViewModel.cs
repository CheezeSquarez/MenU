﻿using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace MenU.ViewModels
{
    public class TabControlViewModel : BaseViewModel
    {
        public TabControlViewModel()
        {
            SelectedIndex = 0;
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => SetValue(ref selectedIndex, value);
        }
    }
}