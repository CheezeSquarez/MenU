using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MenU.Views;
using MenU.Models;
using System.Collections.Generic;

[assembly: ExportFont("BIGSHOULDERSDISPLAY-REGULAR.TTF", Alias = "Big Shoulders")]
[assembly: ExportFont("Staatliches-Regular.ttf", Alias = "Staatliches")]
namespace MenU
{
    
    public partial class App : Application
    {
        public static string ErrorHandler(int statusCode, string currentMessage)
        {
            if (currentMessage != "")
            {
                string message = "";
                ((App)App.Current).StatusCodes.TryGetValue(statusCode, out message);
                if (message == default(String))
                    return "An unknown error occured. Please try again later";
                else
                    return message;
            }
            else
                return currentMessage;
        }
        public Account User { get; set; }
        public Dictionary<int, string> StatusCodes { get; set; }
        public App()
        {
            InitializeComponent();
            StatusCodes = new Dictionary<int, string>();
            StatusCodes.Add(404, "This page does not exist.");
            StatusCodes.Add(403, "You don't have access to this. Log in or try again later");
            StatusCodes.Add(500, "We are having trouble connecting to our servers. Please try again later");
            StatusCodes.Add(409, "A user with this username or email already exists.");
            StatusCodes.Add(200, "");

            MainPage = new NavigationPage(new StartupPage())
            {
                BarBackgroundColor = Color.FromHex("#000000"),
                BarTextColor = Color.White
            };
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
