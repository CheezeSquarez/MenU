using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MenU.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public LoginViewModel() { proxy = MenUWebAPI.CreateProxy(); }
        public LoginViewModel(string error) : this()
        {
            if (error != "")
                this.Error = error;
        }

        #region Attributes

        private string error = "";
        private bool keepLoggedIn = false;
        private string pass = "";
        private MenUWebAPI proxy;
        private string username = "";

        #endregion Attributes

        #region Properties And Events

        public event Action<Page> Push;

        public string Error
        {
            get => error;
            set => SetValue(ref error, value);
        }

        public bool KeepLoggedIn
        {
            get => keepLoggedIn;
            set => SetValue(ref keepLoggedIn, value);
        }

        public string Pass
        {
            get => pass;
            set => SetValue(ref pass, value);
        }

        public string Username
        {
            get => username;
            set => SetValue(ref username, value);
        }

        #endregion Properties And Events

        #region Commands and Methods

        public ICommand ForgotPasswordCommand => new Command(() => { });
        public ICommand LoginCommand => new Command(LoginMethod);
        public ICommand ToSignUpCommand => new Command(() => Push?.Invoke(new SignUp()));

        private async void LoginMethod()
        {
            
            (Account, int) result = await proxy.LoginAsync(Username, Pass); // Logs in with the API
            Account acc = result.Item1;

            if (acc != null) // Checks if login was successful
            {
                ((App)App.Current).User = acc;
                if (KeepLoggedIn) // Checkes if the stay logged in box was checked
                {
                    (string, int) tokenResult = await proxy.CreateToken(); // Requests a new token from the API
                    if (tokenResult.Item2 == 200)
                    {
                        // saves token in secure storage
                        await SecureStorage.SetAsync("auth_token", tokenResult.Item1);
                        Push?.Invoke(new ProfilePage());
                        return;
                    }
                    Error = App.ErrorHandler(tokenResult.Item2, Error);
                }
                else
                {
                    Push?.Invoke(new ProfilePage());
                    return;
                }

            }
            Error = App.ErrorHandler(result.Item2, Error);
        }
        
            

        #endregion Commands and Methods
    }
}