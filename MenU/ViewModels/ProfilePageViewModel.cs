﻿using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using MenU.Models;
using System.Linq;

namespace MenU.ViewModels
{
    class ProfilePageViewModel : BaseViewModel
    {
        public ProfilePageViewModel() 
        { 
            this.proxy = MenUWebAPI.CreateProxy();
            acc = ((App)App.Current).User;
            FName = acc.FirstName;
            LName = acc.LastName;
            Username = acc.Username;
            birthday = acc.DateOfBirth;
            Reviews = new ObservableCollection<Review>(acc.Reviews);
        }

        #region Attributes
        MenUWebAPI proxy;
        private string fName;
        private string lName;
        private string username;
        private DateTime birthday;
        private ObservableCollection<Review> Reviews;
        private Account acc;
        #endregion

        #region Properties and Events
        public string FName
        {
            get => fName;
            set => SetValue(ref fName, value);
        }
        public string LName
        {
            get => lName;
            set => SetValue(ref lName, value);
        }
        public string Username
        {
            get => username;
            set => SetValue(ref username, value);
        }
        public string Birthday
        {
            get => birthday.ToString("dd/MM/yyyy");
        }
        public int ReviewCount
        {
            get => Reviews.Count;
        }
        #endregion

        #region Commands and Methods
        public event Action<Page> Push;
        public Command ChangeInfo => new Command(() => Push?.Invoke(new ChangeInfo()));
        public ICommand ReviewClicked => new Command<string>((s) => Push?.Invoke(new ReviewPage(s)));

        #endregion
    }
}