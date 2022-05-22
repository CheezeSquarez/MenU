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
    public partial class AddReview : ContentPage
    {
        AddRatingViewModel context;
        public AddReview(int dishId)
        {
            context = new AddRatingViewModel(dishId);
            this.BindingContext = context;
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            uint transitionTime = 60;
            double displacement = Like.Width;

            await Task.WhenAll(
                Like.FadeTo(0, transitionTime, Easing.Linear));

            // Changes image source.
            context.ToggleLiked.Execute(null);
            
            await Task.WhenAll(
                Like.FadeTo(1, transitionTime, Easing.Linear));
        }
    }
}