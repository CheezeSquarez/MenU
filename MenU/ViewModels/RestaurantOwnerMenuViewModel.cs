using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;

namespace MenU.ViewModels
{
    public class RestaurantOwnerMenuViewModel : BaseViewModel
    {
        #region Properties And Events
        public event Action<Page> Push;
        #endregion

        #region Commands and Method
        public Command GoToRegisterRestaurant => new Command(() => Push?.Invoke(new RestaurantRegister()));
        public Command<Restaurant> GoToRestaurantPage => new Command<Restaurant>((r) => Push?.Invoke(new EditRestaurantPage(r)));
        public Command<Restaurant> DeleteRestaurantCommand => new Command<Restaurant>((r) => DeleteRestaurant(r));
        public Command<Restaurant> GoToRestaurantSettings => new Command<Restaurant>((r) => Push?.Invoke(new RestaurantPage_ManagerSide(r.RestaurantId)));
        private async void DeleteRestaurant(Restaurant r)
        {
            if(r != null)
            {
                bool choice = await App.Current.MainPage.DisplayAlert("Delete Restaurant?", "Are you sure you want to delete this restaurant?", "Yes", "No");
                if (choice)
                {
                    (bool,int) result = await proxy.DeleteRestaurant(r.RestaurantId);
                    if (result.Item1)
                    {
                        ((App)App.Current).TriggerRestaurantChangedEvent();
                    }
                }
            }
        }


        private async void RefreshList()
        {
            RestaurantList.Clear();
            Account acc = ((App)App.Current).User;
            if (acc != null)
            {
                Random random = new Random();
                (List<Restaurant>, int) result = await proxy.GetOwnersRestaurants(acc.AccountId);
                List<Restaurant> restaurants = result.Item1;
                foreach (Restaurant r in restaurants)
                {
                    r.RestaurantPicture = $"{MenUWebAPI.DEFAULT_IMG_URI}banners/B{r.RestaurantId}.jpg?{random.Next()}";
                    RestaurantList.Add(r);
                }
            }
        }
        #endregion

        #region Attributes
        public ObservableCollection<Restaurant> RestaurantList { get; set; }
        private MenUWebAPI proxy;
        #endregion

        public RestaurantOwnerMenuViewModel()
        {
            proxy = MenUWebAPI.CreateProxy();
            RestaurantList = new ObservableCollection<Restaurant>();
            Account acc = ((App)App.Current).User;
            RefreshList();

            ((App)App.Current).RestaurantChanged += RefreshList;
            ((App)App.Current).RestaurantAdded += RefreshList;

        }
    }
}
