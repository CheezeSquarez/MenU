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
    class StartupPageViewModel : BaseViewModel
    {
        #region Attributes
        private string message = "";
        private MenUWebAPI proxy;
        #endregion

        #region Properties
        public event Action<Page> Push;
        public string Message
        {
            get => message;
            set => SetValue(ref message, value);
        }
        #endregion

        #region Methods

        private async void StartUp()
        {
            //Retrieves auth token from secure storage
            string token = await SecureStorage.GetAsync("auth_token");
            if (token != null)
            {
                (Account, int) result = await proxy.LoginAsync(token); //Logs in using auth token
                Account acc = result.Item1;
                if (acc != null) //Checks if login was successful
                    ((App)App.Current).User = acc;
                else //If the login was not successful, it will push a new login page with an error
                {
                    Push?.Invoke(new Login(App.ErrorHandler(result.Item2, "")));
                    return;
                }
                    


            }
            Push?.Invoke(new ProfilePage());

        }
        public StartupPageViewModel() 
        { 
            proxy = MenUWebAPI.CreateProxy();
            this.StartUp();
        }
        #endregion
    }

}
