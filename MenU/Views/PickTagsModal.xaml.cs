using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenU.ViewModels;
using MenU.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
namespace MenU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickTagsModal : ContentPage
    {
        public PickTagsModal(RegisterRestaurantViewModel context)
        {
            this.BindingContext = context;
            InitializeComponent();
        }
        public PickTagsModal(EditRestaurantViewModel context)
        {
            this.BindingContext = context;
           
            InitializeComponent();
            
        }

       

    }
}