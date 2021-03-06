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
    public partial class AddDish : ContentPage
    {
        public AddDish(RegisterRestaurantViewModel context)
        {
            this.BindingContext = context;
            
            InitializeComponent();

            BackCommand.Command = context.OnBackButtonTapped;

        }
        public AddDish(RestaurantPage_ManagerSide_ViewModel context)
        {
            this.BindingContext = context;
            
            InitializeComponent();

            BackCommand.Command = context.OnBackButtonTappedModal;
        }
    }
}