using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MenU.ViewModels
{
    class StartupPageViewModel : BaseViewModel
    {
        #region Attributes
        private string message = "";
        private MenUWebAPI proxy;
        #endregion

        #region Properties
        public event Action<Page> Push;
        public string Message
        {
            get => message;
            set => SetValue(ref message, value);
        }
        #endregion

        #region Methods

        private async void StartUp()
        {
            ////Retrieves auth token from secure storage
            //string token = await SecureStorage.GetAsync("auth_token");
            //if (token != null)
            //{
            //    (Account, int) result = await proxy.LoginAsync(token); //Logs in using auth token
            //    Account acc = result.Item1;
            //    if (acc != null) //Checks if login was successful
            //        ((App)App.Current).User = acc;
            //    else //If the login was not successful, it will push a new login page with an error
            //    {
            //        Push?.Invoke(new Login(App.ErrorHandler(result.Item2, "")));
            //        return;
            //    }



            //}
            //Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            //{
            //    // do something every 1 seconds
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //    });
            //    return false; // runs again, or false to stop
            //});
            //App.Current.MainPage = new NavigationPage(new ProfilePage());

            MenUWebAPI proxy = MenUWebAPI.CreateProxy();
            try
            {
                (List<Tag>, int) tags = await proxy.GetAllTags();
                (List<Allergen>, int) allergens = await proxy.GetAllAllergens();
                if (tags.Item1 != null && allergens.Item1 != null)
                {
                    ((App)App.Current).Tags = tags.Item1;
                    ((App)App.Current).Allergens = allergens.Item1;

                    List<DishTag> dishTags = new List<DishTag>();
                    dishTags.Add(new DishTag() { DishId = 32, TagId = 1 });
                    dishTags.Add(new DishTag() { DishId = 32, TagId = 2 });
                    dishTags.Add(new DishTag() { DishId = 32, TagId = 3 });

                    List<AllergenInDish> allergenInDishes = new List<AllergenInDish>();
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 1 });
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 2 });
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 3 });
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 4 });
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 5 });
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 6 });
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 7 });
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 8 });
                    allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 9 });

                    ((App)App.Current).MainPage = new NavigationPage(new DishPage(new Dish() { DishPicture = "pasta.png", DishTags = dishTags, AllergenInDishes = allergenInDishes, DishName = "Pasta Aglio e Olio", DishDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum." }));
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
        public StartupPageViewModel() 
        { 
            proxy = MenUWebAPI.CreateProxy();
            this.StartUp();
        }
        #endregion
    }

}
