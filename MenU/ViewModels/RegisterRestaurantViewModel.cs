using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;



namespace MenU.ViewModels
{
    public class RegisterRestaurantViewModel : BaseViewModel
    {

        private string restaurantName;
        private string streetName;
        private string city;
        private string houseNo;
        private MenUWebAPI proxy;

        public ObservableCollection<Tag> TagsList { get; set; }
        public ObservableCollection<Tag> SelectedTags { get; set; }

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
        public event Action<Page> PushModal;
        public event Action PopModal;
        public event Action<Page> Push;

        public Command<Tag> RemoveTag => new Command<Tag>(RemoveTagMethod);
        public Command AddTagClicked => new Command(() => PushModal?.Invoke(new PickTagsModal(this)));
        private void RemoveTagMethod(Tag toRemove) => this.SelectedTags.Remove(toRemove);
        public Command RegisterRestaurant => new Command(RegisterRestaurantMethod);
        public Command ConfirmTags => new Command(() => PopModal?.Invoke());
        public Command<object> TagsSelectionChanged => new Command<object>(TagsSelectionChangedMethod);
        private void TagsSelectionChangedMethod(object tagsList)
        {
            SelectedTags.Clear();
            if(tagsList is IList<object>)
            {
                List<object> tags = ((IList<object>)tagsList).ToList();
                foreach(object t in tags)
                {
                    SelectedTags.Add((Tag)t);
                }
            }
        }
        private void RegisterRestaurantMethod()
        {

        }
        public RegisterRestaurantViewModel()
        {
            restaurantName = "";
            streetName = "";
            houseNo = "";
            city = "";
            SelectedTags = new ObservableCollection<Tag>();

            TagsList = new ObservableCollection<Tag>();
            InitTagsList();


        }

        private async void InitTagsList()
        {
            proxy = MenUWebAPI.CreateProxy();
            (List<Tag>, int) proxyResult = await proxy.GetAllTags();
            List<Tag> tags = proxyResult.Item1;
            foreach (Tag t in tags)
                this.TagsList.Add(t);
        }
    }
}
