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
        #endregion

        #region Attributes
        public ObservableCollection<Restaurant> RestaurantList { get; set; }
        #endregion

        public RestaurantOwnerMenuViewModel()
        {
            RestaurantList = new ObservableCollection<Restaurant>();
            Account acc = ((App)App.Current).User;
            if(acc != null)
            {
                foreach (Restaurant r in acc.Restaurants)
                {
                    RestaurantList.Add(r);
                }
            }
        }
    }
}
