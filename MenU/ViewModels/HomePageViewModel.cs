using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using MenU.Models;
using MenU.Services;
using MenU.Views;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace MenU.ViewModels
{
    class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel() 
        { 
            this.proxy = MenUWebAPI.CreateProxy(); 
            DishList1 = new ObservableCollection<Dish>();
            DishList2 = new ObservableCollection<Dish>();
            RestaurantList1 = new ObservableCollection<Restaurant>();
            RestaurantList2 = new ObservableCollection<Restaurant>();
            ((App)App.Current).RestaurantChanged += PopulateLists;
            PopulateLists();
        }

        #region Attributes
        private MenUWebAPI proxy;
        private Tag dishesTag1;
        private Tag restaurantsTag1;
        private Tag dishesTag2;
        private Tag restaurantsTag2;
        private string searchTerm;
        #endregion

        #region Properties and Events
        public ObservableCollection<Dish> DishList1 { get; set; }
        public ObservableCollection<Dish> DishList2 { get; set; }
        public ObservableCollection<Restaurant> RestaurantList1 { get; set; }
        public ObservableCollection<Restaurant> RestaurantList2 { get; set; }
        public Tag DishesTag1
        {
            get => this.dishesTag1;
            set => SetValue(ref this.dishesTag1, value);
        }
        public Tag RestaurantsTag1
        {
            get => this.restaurantsTag1;
            set => SetValue(ref this.restaurantsTag1, value);
        }
        public Tag DishesTag2
        {
            get => this.dishesTag2;
            set => SetValue(ref dishesTag2, value);
        }
        public Tag RestaurantsTag2
        {
            get => this.restaurantsTag2;
            set => SetValue(ref this.restaurantsTag2, value);
        }
        public string SearchTerm
        {
            get => searchTerm;
            set => SetValue(ref searchTerm, value);
        }
        #endregion

        #region Commands and Methods

        public Command SearchCommand => new Command(SearchMethod);
        private void SearchMethod()
        {
            string s = this.SearchTerm;
            SearchTerm = "";
            ((App)App.Current).SearchEvent(s);
        }

        public async void PopulateLists()
        {
            DishList1.Clear();
            DishList2.Clear();
            RestaurantList1.Clear();
            RestaurantList2.Clear();

            List<Tag> tags = new List<Tag>(((App)App.Current).Tags);
            Random random = new Random();
            int randomInt = /*random.Next(1, 20)*/ 17;
            (List<Dish>,int) dishes = await proxy.GetDishesByTag(randomInt);
            int length = Math.Min(20, dishes.Item1.Count);
            for (int i = 0; i < length; i++)
            {
                this.DishList1.Add(dishes.Item1[i]);
            }
            this.DishesTag1 = tags.First(x => x.TagId == randomInt);

            randomInt = random.Next(1, 20);
            dishes = await proxy.GetDishesByTag(randomInt);
            length = Math.Min(20, dishes.Item1.Count);
            for (int i = 0; i < length; i++)
            {
                this.DishList2.Add(dishes.Item1[i]);
            }
            this.DishesTag2 = tags.First(x => x.TagId == randomInt);

            randomInt = random.Next(1, 20);
            (List<Restaurant>, int) restaurants = await proxy.GetRestaurantsByTag(randomInt);
            length = Math.Min(20, restaurants.Item1.Count);
            for (int i = 0; i < length; i++)
            {
                this.RestaurantList1.Add(restaurants.Item1[i]);
            }
            this.RestaurantsTag1 = tags.First(x => x.TagId == randomInt);

            randomInt = random.Next(1, 20);
            restaurants = await proxy.GetRestaurantsByTag(randomInt);
            length = Math.Min(20, restaurants.Item1.Count);
            for (int i = 0; i < length; i++)
            {
                this.RestaurantList2.Add(restaurants.Item1[i]);
            }
            this.RestaurantsTag2 = tags.First(x => x.TagId == randomInt);
        }
        #endregion
    }
}
