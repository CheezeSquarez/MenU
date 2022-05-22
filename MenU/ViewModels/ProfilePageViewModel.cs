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
using System.Linq;


namespace MenU.ViewModels
{
    class ProfilePageViewModel : BaseViewModel
    {
        public ProfilePageViewModel() 
        { 
            this.proxy = MenUWebAPI.CreateProxy();
            acc = ((App)App.Current).User;
            if(acc != null)
            {
                Random random = new Random();
                FName = acc.FirstName;
                LName = acc.LastName;
                Username = acc.Username;
                birthday = acc.DateOfBirth;
                if (acc.AccountType == 2)
                    PfpImgSource = MenUWebAPI.DEFAULT_IMG_URI + "pfp/R" + acc.AccountId + ".jpg?" + random.Next();
                else
                    PfpImgSource = MenUWebAPI.DEFAULT_IMG_URI + "pfp/A" + acc.AccountId + ".jpg?" + random.Next();
                Reviews = new ObservableCollection<Review>(acc.Reviews);
            }
            else
            {
                FName = "";
                LName = "";
                Username = "";
                birthday = DateTime.Now;
                Reviews = new ObservableCollection<Review>();
            }
            ((App)App.Current).UserChanged += this.LoadUserValues;
            
        }

        #region Attributes
        MenUWebAPI proxy;
        private string fName;
        private string lName;
        private string username;
        private DateTime birthday;
        private ObservableCollection<Review> Reviews;
        private Account acc;
        private string pfpImgSource;
        #endregion

        #region Properties and Events
        public string FName
        {
            get => fName;
            set => SetValue(ref fName, value);
        }
        public string PfpImgSource
        {
            get => pfpImgSource;
            set => SetValue(ref pfpImgSource, value);
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
        public Command ChangeInfoClicked => new Command(() => { Push?.Invoke(new ChangeInfo()); });
        public Command Logout => new Command(LogoutMethod);
        private async void LogoutMethod()
        {
            string token = await SecureStorage.GetAsync("auth_token");
            await proxy.LogOutAsync(token);
            SecureStorage.Remove("auth_token");
            ((App)App.Current).User = null;
            App.Current.MainPage = new NavigationPage(new Login());
        }
        public Command Test => new Command(async () =>await proxy.Test());
        private void PushToChangeInfo()
        {
            
        }
        public ICommand ReviewClicked => new Command<string>((s) => Push?.Invoke(new ReviewPage(s)));
        public void LoadUserValues()
        {
            acc = ((App)App.Current).User;
            if(acc != null)
            {
                Random random = new Random();
                FName = acc.FirstName;
                LName = acc.LastName;
                Username = acc.Username;
                birthday = acc.DateOfBirth;
                Reviews = new ObservableCollection<Review>(acc.Reviews);
                if (acc.AccountType == 2)
                    PfpImgSource = MenUWebAPI.DEFAULT_IMG_URI + "pfp/R" + acc.AccountId + ".jpg?" + random.Next();
                else
                    PfpImgSource = MenUWebAPI.DEFAULT_IMG_URI + "pfp/A" + acc.AccountId + ".jpg?" + random.Next();
            }
            
        }

        #endregion
    }
}
