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
    public partial class DishImage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public DishImage(RegisterRestaurantViewModel context)
        {
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}