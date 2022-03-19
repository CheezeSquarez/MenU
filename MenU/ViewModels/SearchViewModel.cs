using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using MenU.Models;
using System.Linq;



namespace MenU.ViewModels
{
    public class SearchItem
    {
        public SearchItem(string name, string imgSource, int id, string city)
        { 
            this.ImgSource = imgSource;
            this.RestaurantName = name;
            this.RestaurantId = id;
            this.City = city;
        }
        public string ImgSource { get; private set; }
        public int RestaurantId { get; private set; }
        public string RestaurantName { get; private set; }
        public string City { get; private set; }
    }
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel()
        {
            searchTerm = "";
            SearchResult = new ObservableCollection<SearchItem>();
            proxy = MenUWebAPI.CreateProxy();
        }
        #region Attributes
        private string searchTerm;
        private SearchItem selectedItem;
        private MenUWebAPI proxy;
        #endregion

        #region Properties
        public string SearchTerm
        {
            get => searchTerm;
            set => SetValue(ref searchTerm, value);
        }

        public SearchItem SelectedItem
        {
            get => selectedItem;
            set => SetValue(ref selectedItem, value);
        }
        public ObservableCollection<SearchItem> SearchResult { get; set; }
        #endregion

        #region Commands and Methods
        public event Action<Page> Push;
        public Command SearchCommand => new Command(Search);
        private async void Search()
        {
            SearchResult.Clear();
            (List<Restaurant>, int) result = await proxy.GetRestaurantByString(SearchTerm);

            if(result.Item1 != null)
            {
                foreach (Restaurant r in result.Item1)
                    SearchResult.Add(new SearchItem(r.RestaurantName, /* ImgSource = MenUWebAPI.BASE_URI + r. */ "default_restaurant.jpg", r.RestaurantId, r.City));
            }

        }

        public Command<int> GoToRestaurant => new Command<int>(GoToRestaurantMethod);
        private async void GoToRestaurantMethod(int restaurantId)
        {
            (Restaurant, int) result = await proxy.FindRestaurantById(restaurantId);
            SelectedItem = null;
            Push?.Invoke(new RestaurantPage(result.Item1));

            
        }
        #endregion
    }
}
