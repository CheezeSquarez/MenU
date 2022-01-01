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
    class RestaurantOwnerMenuViewModel : BaseViewModel
    {
        #region Properties And Events
        public event Action<Page> Push;
        #endregion

        #region Commands and Method
        public Command GoToRegisterRestaurant => new Command(() => Push?.Invoke(new RestaurantRegister()));
        #endregion

        #region Attributes
        ObservableCollection<Restaurant> RestaurantList;
        #endregion

        public RestaurantOwnerMenuViewModel()
        {
            RestaurantList = new ObservableCollection<Restaurant>();

        }
    }
}
