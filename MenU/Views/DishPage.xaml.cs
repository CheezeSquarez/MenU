using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenU.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MenU.ViewModels;

namespace MenU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DishPage : ContentPage
    {
        public DishPage(Dish d)
        {
            DishPageViewModel context = new DishPageViewModel(d);


            this.BindingContext = context;
            InitializeComponent();
        }

    }
}