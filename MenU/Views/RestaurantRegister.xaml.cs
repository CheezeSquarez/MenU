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
    public partial class RestaurantRegister : ContentPage
    {
        public RestaurantRegister()
        {
            RegisterRestaurantViewModel context = new RegisterRestaurantViewModel();
            context.PushModal += (p) => Navigation.PushModalAsync(p);
            context.Push += (p) => Navigation.PushAsync(p);
            context.PopModal += () => Navigation.PopModalAsync();
            context.Pop += () => Navigation.PopAsync();
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}