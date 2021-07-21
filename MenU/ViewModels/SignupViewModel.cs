using System.ComponentModel;
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
using Xamarin.Essentials;

namespace MenU.ViewModels
{
    class SignupViewModel : BaseViewModel
    {
        #region Attributes
        private string username = "";
        private string password = "";
        private string email = "";
        private string passwordConfirm = "";
        private string firstName = "";
        private string lastName = "";
        private DateTime dateOfBirth = DateTime.Now.Date;
        private string error;
        private MenUWebAPI proxy;
        #endregion

        #region Properties And Events
        public string Username
        {
            get => username;
            set => SetValue(ref username, value);
        }
        public string Password
        {
            get => password;
            set => SetValue(ref password, value);
        }
        public string Email
        {
            get => email;
            set => SetValue(ref email, value);
        }
        public string PasswordConfirm
        {
            get => passwordConfirm;
            set => SetValue(ref passwordConfirm, value);
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

        public event Action Pop;
        #endregion

        public ICommand SignUpCommand => new Command(SignUpMethod);

        private async void SignUpMethod()
        {
            (string, int) result = await proxy.GenerateSalt(); //Gets a unique salt from the API
            string salt = result.Item1;
            if (salt != "") //Checks for an empty salt
            {
                // Hashes the password with the salt a random amount of times
                Random rndm = new Random();
                string hashedPass = Password + salt;
                int iterations = rndm.Next(50000, 70000);
                for (int i = 0; i < iterations; i++)
                {
                    hashedPass = GeneralProcessing.ComputeSha256Hash(hashedPass);
                }

                //Creates an Account object to send to the API
                Account acc = new Account() { DateOfBirth = this.DateOfBirth, Email = this.Email, FirstName = this.FirstName, LastName = this.LastName, Username = this.Username, Salt = salt, Iterations = iterations, Pass = hashedPass };
                
                (bool isSuccess, int) result2 = await proxy.SignUpAsync(acc); // Signs up server side
                if (result2.isSuccess) // Checks if the sign up was successful
                {
                    Pop?.Invoke();
                    return;
                }

                Error = App.ErrorHandler(result2.Item2, Error);
            }
            Error = App.ErrorHandler(result.Item2, Error);
        }

        public SignupViewModel() { proxy = MenUWebAPI.CreateProxy(); }
    }
}
