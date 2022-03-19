using MenU.Models;
using MenU.Services;
using MenU.Views;
using Rg.Plugins.Popup.Services;
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
    public class SaveChangesViewModel : BaseViewModel
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
            ImgSource = "default_pfp.jpg";
            imgChanged = false;
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
        private ImageSource imgSource;
        private FileResult imageFileResult;
        private bool imgChanged;
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
        public ImageSource ImgSource
        {
            get => imgSource;
            set => SetValue(ref imgSource, value);
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
        public event Action<Page> Push;
        #endregion

        #region Commands and Methods
        public Command ChangePfp => new Command(async () => await PopupNavigation.PushAsync(new ChangePfpPopup(this)));
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
                        (bool, int) responseTuple = await proxy.ChangePass(new Credentials() { password = newPass, id = ((App)App.Current).User.AccountId }); //Requests to Update it via the server
                        if (!responseTuple.Item1) //Checks if the request went through and worked
                        {
                            Error = App.ErrorHandler(responseTuple.Item2, Error);
                        }
                        else Error = "";
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
                    Account currentAcc = ((App)App.Current).User;
                    currentAcc.Username = this.Username;
                    currentAcc.FirstName = this.FirstName;
                    currentAcc.LastName = this.LastName;
                    if (imgChanged)
                    {
                        (bool, int) result = await proxy.UploadImage(new FileInfo()
                        {
                            Name = this.imageFileResult.FullPath
                        }, $"pfp/A{currentAcc.AccountId}.jpg");
                    }
                    (bool, int) ret = await proxy.UpdateAccountInfo(currentAcc);
                    if (ret.Item1)
                    {
                        await App.Current.MainPage.DisplayAlert("Account Info Has Been Saved!", "", "OK");
                        ((App)App.Current).User = currentAcc;
                        //Push?.Invoke(new ProfilePage());
                    }
                    else
                        Error = App.ErrorHandler(ret.Item2, Error);
                }
                else
                    Error = App.ErrorHandler(28, Error);
            }
            
        }

        public Command OnCamera => new Command(OnCameraMethod);
        private async void OnCameraMethod()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "Take a picture"
            });

            if (result != null)
            {
                this.imageFileResult = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                this.ImgSource = imgSource;
            }
        }

        public Command OnGallery => new Command(OnGalleryMethod);
        private async void OnGalleryMethod()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Pick a photo"
            });

            if (result != null)
            {
                this.imageFileResult = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                this.ImgSource = imgSource;
            }
        }

        public Command SaveImage => new Command(async () => 
        {
            imgChanged = true;
            await PopupNavigation.PopAsync();
        });
        #endregion
    }
}
