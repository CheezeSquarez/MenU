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
    class RegisterRestaurantViewModel : BaseViewModel
    {
        private string restaurantName;
        private string streetName;
        private string city;
        private string houseNo;
        private MenUWebAPI proxy;
        public ObservableCollection<Tag> TagList;
        public ObservableCollection<Tag> SelectedTags;
        private Tag currentTag;

        public string RestaurantName
        {
            get => restaurantName;
            set => SetValue(ref restaurantName, value);
        }
        public string StreetName
        {
            get => streetName;
            set => SetValue(ref streetName, value);
        }
        public string HouseNumber
        {
            get => houseNo;
            set => SetValue(ref houseNo, value);
        }
        public string City
        {
            get => city;
            set => SetValue(ref city, value);
        }
        public Tag CurrentTag
        {
            get => currentTag;
            set
            {
                SetValue(ref currentTag, value);
                TagSelected();
            }
        }

        private void TagSelected()
        {
            SelectedTags.Add(CurrentTag);
            CurrentTag = null;
        }
        public Command<Tag> RemoveTag => new Command<Tag>(RemoveTagMethod);
        private void RemoveTagMethod(Tag toRemove) => this.SelectedTags.Remove(toRemove);
        public Command RegisterRestaurant => new Command(RegisterRestaurantMethod);
        private void RegisterRestaurantMethod()
        {

        }
        public RegisterRestaurantViewModel()
        {
            restaurantName = "";
            streetName = "";
            houseNo = "";
            city = "";
            currentTag = null;
            SelectedTags = new ObservableCollection<Tag>();
            (List<Tag>, int) proxyResult = await proxy.GetAllTags();

        }
    }
}
