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
        private async Task<bool> Authenticate(string message)
        { 
            string response = await App.Current.MainPage.DisplayPromptAsync(message, "Please enter your current password", "OK");
            string hashed = GeneralProcessing.PasswordToHash(response, salt, iterations);
            if (hashed == ((App)App.Current).User.Pass)
                return true;
            return false;
        }
        public Command ChangePass => new Command(ChangePassMethod);
        private async void ChangePassMethod()
        {
            bool authReturn = await this.Authenticate("Reset Password");
            if(authReturn)
            {
                string newPass = await App.Current.MainPage.DisplayPromptAsync("New Password", "Enter your new password:", "OK"); //Prompts the user for the new pass and to confirm it
                string confirmPass = await App.Current.MainPage.DisplayPromptAsync("Confirm New Password", "Confirm your new password:", "OK");
                if (newPass == confirmPass) //Checks if new pass and confirmation match
                {
                    string newPassHashed = GeneralProcessing.PasswordToHash(newPass, salt, iterations); //Hashes the new pass
                    (bool, int) responseTuple = await proxy.ChangePass(newPassHashed, ((App)App.Current).User.AccountId); //Requests to Update it via the server
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
            else
                Error = App.ErrorHandler(28, Error);



            if (Error == "")
                await App.Current.MainPage.DisplayAlert("Success!", "Your new password has been saved!", "Great!"); //Displays success message

        }
        public Command AcceptChanges => new Command(AcceptChangesMethod);
        private async void AcceptChangesMethod()
        {
            bool auth = await this.Authenticate("Enter Password to Confirm Changes");
            if (auth)
            {
                (bool, int) ret = await proxy.UpdateAccountInfo(Username, firstName, lastName);
                if (ret.Item1)
                {
                    await App.Current.MainPage.DisplayAlert("Account Info Has Been Saved!","","OK");
                }
                else
                    Error = App.ErrorHandler(ret.Item2, Error);
            }
            else 
                Error = App.ErrorHandler(28, Error);
        }
        #endregion
    }
}
