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
    public class EditRestaurantViewModel : BaseViewModel
    {
        private string restaurantName;
        private string houseNumber;
        private string streetName;
        private string cityName;
        private MenUWebAPI proxy;
        
        public string RestaurantName
        {
            get => restaurantName;
            set => SetValue(ref restaurantName, value);
        }
        public string HouseNumber
        {
            get => houseNumber;
            set => SetValue(ref houseNumber, value);
        }
        public string StreetName
        {
            get => streetName;
            set => SetValue(ref streetName, value);
        }
        public string CityName
        {
            get => cityName;
            set => SetValue(ref cityName, value);
        }
        public event Action<Page> PushModal;
        public event Action<Page> Push;
        public event Action Pop;
        public event Action PopModal;
        private Restaurant restaurant;

        private void TagsSelectionChangedMethod(object tagsList)
        {
            SelectedTags.Clear();
            if (tagsList is IList<object>)
            {
                List<object> tags = ((IList<object>)tagsList).ToList();
                foreach (object t in tags)
                {
                    SelectedTags.Add((Tag)t);
                }
            }
        }

        public Command<object> TagsSelectionChanged => new Command<object>(TagsSelectionChangedMethod);
        public ObservableCollection<Tag> SelectedTags { get; set; }
        public Command AddTagClicked => new Command(() => PushModal?.Invoke(new PickTagsModal(this)));
        public ObservableCollection<Tag> TagsList { get; set; }
        public Command ConfirmTags => new Command(() => PopModal?.Invoke());
        private void RemoveTagMethod(Tag toRemove) => this.SelectedTags.Remove(toRemove);
        public Command<Tag> RemoveTag => new Command<Tag>(RemoveTagMethod);
        public Command ConfirmChanges => new Command(ConfirmChangesMethod);
        private async Task ConfirmChangesMethod()
        {
            List<RestaurantTag> restaurantTags = new List<RestaurantTag>();
            foreach (Tag tag in this.SelectedTags)
            {
                restaurantTags.Add(new RestaurantTag { RestaurantId = restaurant.RestaurantId, TagId = tag.TagId });
            }
            RestaurantDTO r = new RestaurantDTO() 
            { 
                Restaurant = new Restaurant() 
                { 
                    RestaurantId = restaurant.RestaurantId, 
                    City = CityName, RestaurantName = RestaurantName, 
                    StreetName = StreetName, 
                    StreetNumber = houseNumber 
                }, 
                RestaurantTags = restaurantTags 
            };
            (Restaurant, int) result = await proxy.UpdateRestaurant(r);
        }
        public EditRestaurantViewModel(Restaurant r)
        {
            restaurant = r;
            proxy = MenUWebAPI.CreateProxy();
            List<Tag> allTags = ((App)App.Current).Tags;
            TagsList = new ObservableCollection<Tag>(allTags);

            SelectedTags = new ObservableCollection<Tag>();
            if (r!= null)
            {
                restaurantName = r.RestaurantName;
                houseNumber = r.StreetNumber;
                streetName = r.StreetName;
                cityName = r.City;
                foreach (RestaurantTag rT in r.RestaurantTags)
                    SelectedTags.Add(allTags.FirstOrDefault(x => x.TagId == rT.TagId));
            }
            

           
        }
    }
}
