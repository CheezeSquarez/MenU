using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using MenU.Models;

namespace MenU.ViewModels
{
    class ProfilePageViewModel : BaseViewModel
    {
        public ProfilePageViewModel() { this.proxy = MenUWebAPI.CreateProxy(); }

        #region Attributes
        MenUWebAPI proxy;
        public string fName;
        public string lName;
        public string username;
        public DateTime birthday;
        public ObservableCollection<Review> Reviews;
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
            get => birthday.ToString();
        }
        #endregion

        #region Commands and Methods

        #endregion
    }
}
