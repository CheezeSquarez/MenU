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
        private string randomText;
        #endregion

        #region Properties
        public event Action<Page> Push;
        public string RandomText
        {
            get => randomText;
            set => SetValue(ref randomText, value);
        }
        public string Message
        {
            get => message;
            set => SetValue(ref message, value);
        }
        #endregion

        #region Methods

        public async void StartUp()
        {

            MenUWebAPI proxy = MenUWebAPI.CreateProxy();

            try
            {
                (List<Tag>, int) tags = await proxy.GetAllTags();
                (List<Allergen>, int) allergens = await proxy.GetAllAllergens();
                if (tags.Item1 != null && allergens.Item1 != null)
                {
                    ((App)App.Current).Tags = tags.Item1;
                    ((App)App.Current).Allergens = allergens.Item1;
                }
            }
            catch (Exception ex)
            {

            }
            SetText();
            //Retrieves auth token from secure storage
            string token = await SecureStorage.GetAsync("auth_token");
            if (token != null)
            {
                (Account, int) result = await proxy.LoginAsync(token); //Logs in using auth token
                Account acc = result.Item1;
                if (acc != null) //Checks if login was successful
                    ((App)App.Current).User = acc;
                else //If the login was not successful, it will push a new login page with an error
                {
                    Push?.Invoke(new Login(App.ErrorHandler(result.Item2, "")));
                    return;
                }



            }
            //App.Current.MainPage = new NavigationPage(new TabControlView());
            List<RestaurantTag> rts = new List<RestaurantTag>();
            rts.Add(new RestaurantTag() { RestaurantId = 1, TagId = 1 });
            rts.Add(new RestaurantTag() { RestaurantId = 1, TagId = 2 });
            rts.Add(new RestaurantTag() { RestaurantId = 1, TagId = 3 });
            Restaurant r = new Restaurant() { City = "abc", RestaurantName = "abc", StreetName="abc", StreetNumber="abc", RestaurantTags = rts };
            App.Current.MainPage = new NavigationPage(new EditRestaurantPage(r));
            
            //try
            //{
            //    (List<Tag>, int) tags = await proxy.GetAllTags();
            //    (List<Allergen>, int) allergens = await proxy.GetAllAllergens();
            //    if (tags.Item1 != null && allergens.Item1 != null)
            //    {
            //        ((App)App.Current).Tags = tags.Item1;
            //        ((App)App.Current).Allergens = allergens.Item1;

            //        List<DishTag> dishTags = new List<DishTag>();
            //        dishTags.Add(new DishTag() { DishId = 32, TagId = 1 });
            //        dishTags.Add(new DishTag() { DishId = 32, TagId = 2 });
            //        dishTags.Add(new DishTag() { DishId = 32, TagId = 3 });

            //        List<AllergenInDish> allergenInDishes = new List<AllergenInDish>();
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 1 });
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 2 });
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 3 });
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 4 });
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 5 });
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 6 });
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 7 });
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 8 });
            //        allergenInDishes.Add(new AllergenInDish { DishId = 32, AllergenId = 9 });
            //        DishPage d = new DishPage(new Dish() { DishPicture = "pasta.png", DishTags = dishTags, AllergenInDishes = allergenInDishes, DishName = "Pasta Aglio e Olio", DishDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum." });
            //        ((App)App.Current).MainPage = new NavigationPage(new TabControlView());
            //    }
            //    else
            //    {
            //        //Do Something
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Do Something
            //}

        }

        public void SetText()
        {
            string[] strings =
            {
                "Steaming your buns",
                "Flipping your burgers",
                "Salting your chips",
                "Sneezing in your salad",
                "Mixing your salad",
                "Sauteing your fish",
                "Burning your rice",
                "Baking your cookies",
                "Boiling the onions",
                "Steaming your dumplings",
                "Strangling the waiter",
                "Calling the manager",
                "Freeing the cows",
                "Milking the cows",
                "Curdling the cheese",
                "Grilling your burger",
                "Chopping your vegetables",
                "Broiling the shepards pie",
                "Wetting the noodles",
                "Roasting your personality",
                "Roasting the brisket",
                "Smoking the brisket",
                "Baking the pizza",
                "Topping your pizza",
                "Rolling the dough",
                "Making the donut hole",
                "Toasting your sandwich",
                "Boiling your eggs",
                "Steaming your rice",
                "Simmering the Legumes",
                "Braising the steak",
                "Stewing the meats",
                "Blanching the tomatos",
                "Emulsifying the mayonaise",
                "Filleting your fish",
                "Flambeing your pasta",
                "Spilling your water",
                "Staining your shirt",
                "Making you think your food is ready",
                "Punching the dough",
                "Breaking some eggs",
                "The cake is a lie",
                "Pressuring you to order",
                "Rating your dish"
            };

            Random ran = new Random();
            RandomText = strings[ran.Next(0, strings.Length)];
        }
        public StartupPageViewModel() 
        {
            SetText();
            proxy = MenUWebAPI.CreateProxy();
        }
        #endregion
    }

}
