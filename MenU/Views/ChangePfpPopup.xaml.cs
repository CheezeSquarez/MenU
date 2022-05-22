using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MenU.ViewModels;

namespace MenU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePfpPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ChangePfpPopup(SaveChangesViewModel context)
        {
            this.BindingContext = context;
            InitializeComponent();
           
        }

        public ChangePfpPopup(RegisterRestaurantViewModel context)
        {
            this.BindingContext = context;
            InitializeComponent();

        }
    }
}