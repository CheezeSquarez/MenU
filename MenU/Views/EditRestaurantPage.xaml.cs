using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenU.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MenU.Models;

namespace MenU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditRestaurantPage : ContentPage
    {
        public EditRestaurantPage(Restaurant r)
        {
            EditRestaurantViewModel context = new EditRestaurantViewModel(r);
            context.PushModal += (p) => Navigation.PushModalAsync(p);
            context.Push += (p) => Navigation.PushAsync(p);
            context.PopModal += () => Navigation.PopModalAsync();
            context.Pop += () => Navigation.PopAsync();
            this.BindingContext = context;
            InitializeComponent();
        }
    }
}