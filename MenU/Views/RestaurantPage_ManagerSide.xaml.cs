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
    public partial class RestaurantPage_ManagerSide : ContentPage
    {
        public RestaurantPage_ManagerSide(int restaurantId)
        {
            RestaurantPage_ManagerSide_ViewModel context = new RestaurantPage_ManagerSide_ViewModel(restaurantId);
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}