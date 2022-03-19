using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MenU.Views;
using MenU.Models;
using System.Collections.Generic;
using MenU.Services;
using System.Threading;

[assembly: ExportFont("BIGSHOULDERSDISPLAY-REGULAR.TTF", Alias = "Big Shoulders")]
[assembly: ExportFont("Staatliches-Regular.ttf", Alias = "Staatliches")]
[assembly: ExportFont("VarelaRound-Regular.ttf", Alias = "Varela")]
namespace MenU
{
    
    public partial class App : Application
    {
        public static string ErrorHandler(int statusCode, string currentMessage)
        {
            if (currentMessage == "" || currentMessage == null)
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
        private Account user;
        public event Action UserChanged;
        public Account User { get => user; 
            set
            {
                user = value;
                UserChanged?.Invoke();
            }
        }

        
        public Dictionary<int, string> StatusCodes { get; private set; }
        public List<Tag> Tags { get; set; }
        public List<Allergen> Allergens { get; set; }
        public App()
        {
            //User = new Account { FirstName = "Ido", LastName = "Rosenfeld-Sadeh", Username = "ProjectFluffy", DateOfBirth = DateTime.Now, Reviews = new List<Review>(), Pass = "aafeeba6959ebeeb96519d5dcf0bcc069f81e4bb56c246d04872db92666e6d4b", Salt = "12345678", Iterations = 1 };
            InitializeComponent();

            #region Status Codes
            StatusCodes = new Dictionary<int, string>();
            StatusCodes.Add(404, "This page does not exist.");
            StatusCodes.Add(403, "You don't have access to this. Log in or try again later");
            StatusCodes.Add(500, "We are having trouble connecting to our servers. Please try again later");
            StatusCodes.Add(409, "A user with this username or email already exists.");
            StatusCodes.Add(204, "This content is missing from our database...");
            StatusCodes.Add(401, "Incorrect credentials. Please try again later");
            StatusCodes.Add(503, "We are having trouble with our web service. Please try again later");
            StatusCodes.Add(200, "");
            #endregion

            #region Dynamic Resources
            Resources.Add("ThemeBlue", Color.FromHex("030D30"));
            Resources.Add("ErrorRed", "Red");
            Resources.Add("SecondaryBlue", Color.FromHex("00b4d8"));
            Resources.Add("SvgTint", Color.FromHex("808080"));
            
            #endregion
            user = null;

            Tags = new List<Tag>();
            Allergens = new List<Allergen>();

            ContentPage page = new ContentPage();
            page.Content = new SearchView();
            MainPage = new StartupPage();
        }
        private async void ResolveLists()
        {
            MenUWebAPI proxy = MenUWebAPI.CreateProxy();
            try
            {
                (List<Tag>, int) tags = await proxy.GetAllTags();
                (List<Allergen>, int) allergens = await proxy.GetAllAllergens();
                if(tags.Item1 != null && allergens.Item1 != null)
                {
                    this.Tags = tags.Item1;
                    this.Allergens = allergens.Item1;
                }
                else
                {
                    //Do Something
                }
            }
            catch (Exception ex)
            {
                // Do Something
            }
            
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
