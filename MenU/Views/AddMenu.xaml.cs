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
    public partial class AddMenu : ContentPage
    {
        public AddMenu(RegisterRestaurantViewModel context)
        {
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}