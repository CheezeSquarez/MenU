using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenU.Models;
using MenU.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestaurantOwnerMenu : ContentView
    {
        public RestaurantOwnerMenu()
        {
            RestaurantOwnerMenuViewModel context = new RestaurantOwnerMenuViewModel();
            context.Push += (p) => Navigation.PushAsync(p);
            this.BindingContext = context;
            InitializeComponent();
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {

        }
    }
}