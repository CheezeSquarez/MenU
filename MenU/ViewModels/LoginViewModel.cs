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

namespace MenU.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private string username = "";
        private string pass = "";
        private bool keepLoggedIn = false;

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

        public LoginViewModel() { }
    }
}
