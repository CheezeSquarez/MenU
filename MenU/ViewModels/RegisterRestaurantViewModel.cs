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
        #region Attributes
        private string restaurantName;
        private string streetName;
        private string city;
        private string houseNo;
        private MenUWebAPI proxy;
        private string dishName;
        private string description;
        private int restaurantId;
        private List<Tag> restaurantTags;
        #endregion

        #region Register Page 1
        private async void RegisterRestaurantMethod()
        {
            List<Dish> dishList = new List<Dish>();
            foreach (Dish dish in this.Dishes)
            {
                dishList.Add(dish);
            }
            List<RestaurantTag> tagList = new List<RestaurantTag>();
            foreach (Tag tag in restaurantTags)
            {
                RestaurantTag restaurantTag = new RestaurantTag() { RestaurantId = restaurantId, TagId = tag.TagId };
                tagList.Add(restaurantTag);
            }
            //Restaurant restaurant = new Restaurant() { City = this.City, OwnerId = ((App)App.Current).User.AccountId, RestaurantId = this.restaurantId, RestaurantName = RestaurantName,
            //StreetName = streetName, StreetNumber = HouseNumber, Dishes = dishList, RestaurantTags = tagList };
            Restaurant restaurant = new Restaurant()
            {
                //City = this.City,
                //OwnerId = ((App)App.Current).User.AccountId,
                //RestaurantId = this.restaurantId,
                //RestaurantName = RestaurantName,
                //StreetName = streetName, StreetNumber = HouseNumber, /*Dishes = dishList, RestaurantTags = tagList*/
            };
                (bool, int) registerRestult = await proxy.AddRestaurant(restaurant);
            if (registerRestult.Item1)
            {
                await App.Current.MainPage.DisplayAlert("Restaurant Registered Successfully", "Your Restaurant has been successfully been registered and added to our database", "OK");
                
            }
            App.ErrorHandler(registerRestult.Item2, "");
        }

        public Command GoToAddMenu => new Command(() => {
            foreach (Tag tag in SelectedTags)
            {
                restaurantTags.Add(tag);
            }
            SelectedTags.Clear();
            Push?.Invoke(new AddMenu(this)); } );
        public Command RegisterRestaurant => new Command(RegisterRestaurantMethod);
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

        public string StreetName
        {
            get => streetName;
            set => SetValue(ref streetName, value);
        }

        public string RestaurantName
        {
            get => restaurantName;
            set => SetValue(ref restaurantName, value);
        }

        #endregion

        #region Add Tags
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
        public Command ConfirmTags => new Command(() => PopModal?.Invoke());
        private void RemoveTagMethod(Tag toRemove) => this.SelectedTags.Remove(toRemove);

        public Command AddTagClicked => new Command(() => PushModal?.Invoke(new PickTagsModal(this)));
        public Command<Tag> RemoveTag => new Command<Tag>(RemoveTagMethod);
        public ObservableCollection<Tag> SelectedTags { get; set; }
        public ObservableCollection<Tag> TagsList { get; set; }
        #endregion

        #region Add Allergens
        private void AllergensSelectionChangedMethod(object AllergensList)
        {
            SelectedAllergens.Clear();
            if (AllergensList is IList<object>)
            {
                List<object> Allergens = ((IList<object>)AllergensList).ToList();
                foreach (object t in Allergens)
                {
                    SelectedAllergens.Add((Allergen)t);
                }
            }
        }

        public Command<object> AllergensSelectionChanged => new Command<object>(AllergensSelectionChangedMethod);
        public Command ConfirmAllergens => new Command(() => PopModal?.Invoke());
        private void RemoveAllergenMethod(Allergen toRemove) => this.SelectedAllergens.Remove(toRemove);

        public Command AddAllergenClicked => new Command(() => PushModal?.Invoke(new PickAllergens(this)));
        public Command<Allergen> RemoveAllergen => new Command<Allergen>(RemoveAllergenMethod);
        public ObservableCollection<Allergen> SelectedAllergens { get; set; }
        public ObservableCollection<Allergen> AllergensList { get; set; }
        #endregion

        #region Add Menu
        public Command GoToAddNewDish => new Command(() => Push(new AddDish(this)));
        public Command<Dish> GoToDish => new Command<Dish>((d) => PushModal(new DishPage(d)));
        #endregion

        #region Add Dish
        public ObservableCollection<Dish> Dishes { get; set; }
        public string DishName
        {
            get => dishName;
            set => SetValue(ref dishName, value);
        }
        public string Description
        {
            get => description;
            set => SetValue(ref description, value);
        }
        public Command AddDishClicked => new Command(AddDishMethod);
        public async void AddDishMethod()
        {
            List<AllergenInDish> allergensInDish = new List<AllergenInDish>();
            List<DishTag> tagsInDish = new List<DishTag>();
            (int, int) dishId = await proxy.StampDish();
            if(dishId.Item2 == 200)
            {
                foreach (Allergen a in SelectedAllergens)
                {
                    allergensInDish.Add(new AllergenInDish() { AllergenId = a.AllergenId, DishId = dishId.Item1});
                }
                foreach(Tag tag in SelectedTags)
                {
                    tagsInDish.Add(new DishTag() { TagId = tag.TagId, DishId = dishId.Item1 });
                }
                Dish d = new Dish() { AllergenInDishes = allergensInDish, DishDescription = Description, DishId = dishId.Item1, DishName = DishName, DishTags = tagsInDish, Restaurant = this.restaurantId };
                this.Dishes.Add(d);
                SelectedTags.Clear();
                Pop?.Invoke();
            }
        }
        #endregion

        #region Navigation Events
        public event Action<Page> PushModal;
        public event Action PopModal;
        public event Action<Page> Push;
        public event Action Pop; 
        #endregion
        public RegisterRestaurantViewModel()
        {
            restaurantName = "";
            streetName = "";
            houseNo = "";
            city = "";
            SelectedTags = new ObservableCollection<Tag>();
            SelectedAllergens = new ObservableCollection<Allergen>();  
            AllergensList = new ObservableCollection<Allergen>();
            TagsList = new ObservableCollection<Tag>();
            restaurantTags = new List<Tag>();
            Dishes = new ObservableCollection<Dish>();
            InitTagsList();
            InitAllergensList();
            StampRestaurant();

        }

        private async void StampRestaurant()
        {
            (int, int) stampResult = await proxy.StampRestaurant();
            if(stampResult.Item2 == 200)
            {
                restaurantId = stampResult.Item1;
            }
        }
        private async void InitTagsList()
        {
            proxy = MenUWebAPI.CreateProxy();
            (List<Tag>, int) proxyResult = await proxy.GetAllTags();
            List<Tag> tags = proxyResult.Item1;
            foreach (Tag t in tags)
                this.TagsList.Add(t);
        }
        private async void InitAllergensList()
        {
            proxy = MenUWebAPI.CreateProxy();
            (List<Allergen>, int) proxyResult = await proxy.GetAllAllergens();
            List<Allergen> tags = proxyResult.Item1;
            foreach (Allergen t in tags)
                this.AllergensList.Add(t);
        }
    }
}
