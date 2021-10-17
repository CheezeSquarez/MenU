﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenU.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeInfo : ContentPage
    {
        public ChangeInfo()
        {
            SaveChangesViewModel context = new SaveChangesViewModel();
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}