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
    public partial class ProfilePage : ContentView
    {
        public ProfilePage()
        {
            ProfilePageViewModel context = new ProfilePageViewModel();
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}