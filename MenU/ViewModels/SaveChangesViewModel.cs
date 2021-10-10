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
        }

        #region Attributes
        MenUWebAPI proxy;
        private string username;
        private string email;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private Account acc;
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
        #endregion

        #region Commands and Methods
        public Command ChangePass => new Command(ChangePassMethod);
        private void ChangePassMethod()
        {

        }
        public Command AcceptChanges => new Command(AcceptChangesMethod);
        private void AcceptChangesMethod()
        {

        }
        #endregion
    }
}
