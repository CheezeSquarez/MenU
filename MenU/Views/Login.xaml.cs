using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenU.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login(string error)
        {
            LoginViewModel context = new LoginViewModel(error);
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;
            InitializeComponent();
        }
        public Login()
        {
            LoginViewModel context = new LoginViewModel();
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}