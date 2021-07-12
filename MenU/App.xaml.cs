using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MenU.Views;

[assembly: ExportFont("BIGSHOULDERSDISPLAY-REGULAR.TTF", Alias = "Big Shoulders")]
[assembly: ExportFont("Staatliches-Regular.ttf", Alias = "Staatliches")]
namespace MenU
{
    
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            

            MainPage = new Login();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
