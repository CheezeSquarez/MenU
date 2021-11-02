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
using System.Threading.Tasks;


namespace MenU.ViewModels
{
    class SaveChangesViewModel : BaseViewModel
    {
        public SaveChangesViewModel() 
        { 
            this.proxy = MenUWebAPI.CreateProxy();
            acc = ((App)App.Current).User;
            username = acc.Username;
            email = acc.Email;
            firstName = acc.FirstName;
            lastName = acc.LastName;
            dateOfBirth = acc.DateOfBirth;
            error = "";
            salt = ((App)App.Current).User.Salt;
            iterations = ((App)App.Current).User.Iterations;
        }

        #region Attributes
        MenUWebAPI proxy;
        private string username;
        private string email;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private Account acc;
        private string error;
        private readonly string salt;
        private readonly int iterations;
        #endregion

        #region Properties and Events
        public string Username
        {
            get => username;
            set => SetValue(ref username, value);
        }
        public string Email
        {
            get => email;
            set => SetValue(ref email, value);
        }
        public string FirstName
        {
            get => firstName;
            set => SetValue(ref firstName, value);
        }
        public string LastName
        {
            get => lastName;
            set => SetValue(ref lastName, value);
        }
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set => SetValue(ref dateOfBirth, value);
        }
        public string Error
        {
            get => error;
            set => SetValue(ref error, value);
        }
        #endregion

        #region Commands and Methods
        
        public Command ChangePass => new Command(ChangePassMethod);
        private async Task<bool> Authenticate(string message)
        {
            string pass = await App.Current.MainPage.DisplayPromptAsync("Confirm password", message);
            (Account, int) loginResultTuple = await proxy.LoginAsync(((App)App.Current).User.Username, pass);
            if (loginResultTuple.Item1 != null)
                return true;
            return false;

        }
        private async void ChangePassMethod()
        {
            bool authReturn = await Authenticate("Enter current password");
            if(authReturn)
            {
                string newPass = await App.Current.MainPage.DisplayPromptAsync("New Password", "Enter your new password:", "OK"); //Prompts the user for the new pass and to confirm it
                bool validPass = false;
                while (!validPass)
                {
                    if (!Validation.Ispassword(newPass))
                    {
                        await App.Current.MainPage.DisplayAlert("Invalid Password", $"Password must contain at least one uppercase & one lowercase letter, one number and cannot contain the following characters: {Validation.INVALID_PASSWORD_CHARS}", "OK");
                        newPass = await App.Current.MainPage.DisplayPromptAsync("New Password", "Enter your new password:", "OK"); //Prompts the user for the new pass and to confirm it
                    }
                    else
                        validPass = true;
                }
                
                if (validPass)
                {
                    string confirmPass = await App.Current.MainPage.DisplayPromptAsync("Confirm New Password", "Confirm your new password:", "OK");
                    if (newPass == confirmPass) //Checks if new pass and confirmation match
                    {
                        (bool, int) responseTuple = await proxy.ChangePass(newPass, ((App)App.Current).User.AccountId); //Requests to Update it via the server
                        if (!responseTuple.Item1) //Checks if the request went through and worked
                        {
                            Error = App.ErrorHandler(responseTuple.Item2, Error);
                        }
                    }
                    else
                    {
                        Error = App.ErrorHandler(28, Error);
                    }
                }
                
            }
            else
                Error = App.ErrorHandler(28, Error);



            if (Error == "")
                await App.Current.MainPage.DisplayAlert("Success!", "Your new password has been saved!", "Great!"); //Displays success message

        }
        public Command AcceptChanges => new Command(AcceptChangesMethod);
        private async void AcceptChangesMethod()
        {
            bool validInfo = true;
            if(!Validation.IsUsername(Username))
            {
                validInfo = false;
                await App.Current.MainPage.DisplayAlert("Invalid Username", $"Username cannot contain {Validation.INVALID_CHARS}, two '.' in a row, or start/end with '.'. Username cannot be empty", "OK");

            }

            if (validInfo)
            {
                bool authReturn = await Authenticate("Enter password to confirm changes");
                if (authReturn)
                {
                    (Account, int) ret = await proxy.UpdateAccountInfo(Username, firstName, lastName);
                    if (ret.Item1 != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Account Info Has Been Saved!", "", "OK");
                        ((App)App.Current).User = ret.Item1;
                    }
                    else
                        Error = App.ErrorHandler(ret.Item2, Error);
                }
                else
                    Error = App.ErrorHandler(28, Error);
            }
            
        }
        #endregion
    }
}
