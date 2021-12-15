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
        private const string OPENEYE = "icon_openEye.svg";
        private const string CLOSEDEYE = "icon_closedEye.svg";

        private string username = "";
        private string password = "";
        private string email = "";
        private string passwordConfirm = "";
        private string firstName = "";
        private string lastName = "";
        private bool isRestaurantOwner = false;
        private DateTime dateOfBirth = DateTime.Now.Date;
        private string error = "";
        private MenUWebAPI proxy;

        private string visibilityImage1 = CLOSEDEYE;
        private bool visibilityState1 = true;
        private string visibilityImage2 = CLOSEDEYE;
        private bool visibilityState2 = true;
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
        public bool IsRestaurantOwner
        {
            get => isRestaurantOwner;
            set => SetValue(ref isRestaurantOwner, value);
        }
        public string Error
        {
            get => error;
            set => SetValue(ref error, value);
        }
        public string VisibilityImage1
        {
            get => visibilityImage1;
            set => SetValue(ref visibilityImage1, value);
        }
        public bool VisibilityState1
        {
            get => visibilityState1;
            set => SetValue(ref visibilityState1, value);
        }
        public string VisibilityImage2
        {
            get => visibilityImage2;
            set => SetValue(ref visibilityImage2, value);
        }
        public bool VisibilityState2
        {
            get => visibilityState2;
            set => SetValue(ref visibilityState2, value);
        }

        public event Action Pop;
        #endregion

        public ICommand SignUpCommand => new Command(SignUpMethod);
        public ICommand VisibilityToggle1 => new Command(() =>
        {
            if (visibilityState1 == false)
            {
                VisibilityState1 = true;
                VisibilityImage1 = CLOSEDEYE;
            }
            else
            {
                VisibilityState1 = false;
                VisibilityImage1 = OPENEYE;
            }
        });
        public ICommand VisibilityToggle2 => new Command(() =>
        {
            if (visibilityState2 == false)
            {
                VisibilityState2 = true;
                VisibilityImage2 = CLOSEDEYE;
            }
            else
            {
                VisibilityState2 = false;
                VisibilityImage2 = OPENEYE;
            }
        });



        private async void SignUpMethod()
        {
            bool validAccount = true;
            if(!Validation.IsUsername(Username))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Username", $"Username cannot contain {Validation.INVALID_CHARS}, two '.' in a row, or start/end with '.'. Username cannot be empty", "OK");
                validAccount = false;
            }
            if(!Validation.IsEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Email", "Check email and try again", "OK");
                validAccount = false;
            }
            if (!Validation.Ispassword(Password))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Password", $"Password must contain at least one uppercase & one lowercase letter, one number and cannot contain the following characters: {Validation.INVALID_PASSWORD_CHARS}", "OK");
                validAccount = false;
            }
            if (!Validation.IsOfAge(DateOfBirth))
            {
                await App.Current.MainPage.DisplayAlert("You are not old enough", $"You must be 16 years old or over...", "OK");
                validAccount = false;
            }
            if(Password != passwordConfirm)
            {
                await App.Current.MainPage.DisplayAlert("Passwords Don't Match", $"Password and Confirm Password don't match", "OK");
                validAccount = false;
            }
            if (validAccount)
            {
                //Creates an Account object to send to the API
                Account acc = new Account() { DateOfBirth = this.DateOfBirth, Email = this.Email, FirstName = this.FirstName, LastName = this.LastName, Username = this.Username, Pass = this.Password };
                if (IsRestaurantOwner)
                    acc.AccountType = 2;
                (bool isSuccess, int) result2 = await proxy.SignUpAsync(acc); // Signs up server side
                if (result2.isSuccess) // Checks if the sign up was successful
                {
                    await App.Current.MainPage.DisplayAlert("Signup Completed Successfully!", "Your account has been created and saved on our servers.", "Login");
                    Pop?.Invoke();
                    return;
                }

                Error = App.ErrorHandler(result2.Item2, Error);
            }
            
            

        }

        public SignupViewModel() { proxy = MenUWebAPI.CreateProxy(); }
    }
}
