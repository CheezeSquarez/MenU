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
using Rg.Plugins.Popup.Services;


namespace MenU.ViewModels
{
    public class RestaurantPage_ManagerSide_ViewModel : BaseViewModel
    {
        #region Attributes
        private Restaurant restaurant;
        private MenUWebAPI proxy;
        #endregion

        #region Properties
        public ObservableCollection<Dish> DishList { get; set; }
        public Restaurant Restaurant
        {
            get => restaurant;
            set => SetValue(ref restaurant, value);
        }
        #endregion

        #region Methods and Commands
        public Command<int> DeleteDishCommand => new Command<int>(DeleteDish);
        private async void DeleteDish(int dishId)
        {
            if(dishId > 0)
            {
                (bool,int) result = await proxy.DeleteDish(dishId);
                if (result.Item1)
                {
                    LoadRestaurant(this.Restaurant.RestaurantId);
                    ((App)App.Current).TriggerRestaurantChangedEvent();
                }
            }
        }

        #region Add Dish
        public Command AddDish => new Command(() => ((App)App.Current).MainPage.Navigation.PushModalAsync(new AddDish(this)));

        private string dishName;
        private string description;
        
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
        public Command AddAllergenClicked => new Command(() => ((App)App.Current).MainPage.Navigation.PushModalAsync(new PickAllergens(this)));
        public Command AddTagClicked => new Command(() => ((App)App.Current).MainPage.Navigation.PushModalAsync(new PickTagsModal(this)));
        public ObservableCollection<Allergen> SelectedAllergens { get; set; }
        public ObservableCollection<Tag> SelectedTags { get; set; }
        

        public Command<object> TagsSelectionChanged => new Command<object>(TagsSelectionChangedMethod);
        public Command ConfirmTags => new Command(() => ((App)App.Current).MainPage.Navigation.PopModalAsync());
        private void RemoveTagMethod(Tag toRemove) => this.SelectedTags.Remove(toRemove);
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
        public ObservableCollection<Allergen> AllergensList { get; set; }
        public ObservableCollection<Tag> TagsList { get; set; }

        public Command<object> AllergensSelectionChanged => new Command<object>(AllergensSelectionChangedMethod);
        public Command ConfirmAllergens => new Command(() => ((App)App.Current).MainPage.Navigation.PopModalAsync());
        private void RemoveAllergenMethod(Allergen toRemove) => this.SelectedAllergens.Remove(toRemove);

        public Command<Allergen> RemoveAllergen => new Command<Allergen>(RemoveAllergenMethod);

        public Command<Tag> RemoveTag => new Command<Tag>(RemoveTagMethod);

        public Command AddDishImage => new Command(async () => await PopupNavigation.PushAsync(new DishImage(this)));
        #region Add Dish Img
        private ImageSource imgSourceDish;
        private FileResult imageFileResultDish;


        public ImageSource ImgSourceDish
        {
            get => imgSourceDish;
            set => SetValue(ref imgSourceDish, value);
        }
        public Command OnCameraDish => new Command(OnCameraMethodDish);
        private async void OnCameraMethodDish()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "Take a picture"
            });

            if (result != null)
            {
                this.imageFileResultDish = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSourceDish = ImageSource.FromFile(result.FullPath);
                this.ImgSourceDish = imgSourceDish;
            }
        }

        public Command OnGalleryDish => new Command(OnGalleryMethodDish);
        private async void OnGalleryMethodDish()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Pick a photo"
            });

            if (result != null)
            {
                this.imageFileResultDish = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSourceDish = ImageSource.FromFile(result.FullPath);
                this.ImgSourceDish = imgSourceDish;
            }
        }

        public Command SaveImageDish => new Command(async () =>
        {
            await PopupNavigation.PopAsync();
        });
        #endregion
        public Command AddDishClicked => new Command(AddDishMethod);
        private async void AddDishMethod()
        {
            List<AllergenInDish> allergensInDish = new List<AllergenInDish>();
            List<DishTag> tagsInDish = new List<DishTag>();
            
            foreach (Allergen a in SelectedAllergens)
            {
                allergensInDish.Add(new AllergenInDish() { AllergenId = a.AllergenId });
            }
            foreach (Tag tag in SelectedTags)
            {
                tagsInDish.Add(new DishTag() { TagId = tag.TagId });
            }

            //Builds DTO
            DishDTO d = new DishDTO()
            {
                Dish = new Dish()
                {
                    DishDescription = Description,
                    //DishId = dishId.Item1, 
                    DishName = DishName,
                    DishPicture = "abc",
                    Restaurant = this.Restaurant.RestaurantId 
                },
                AllergenInDishes = allergensInDish,
                Tags = tagsInDish,
                Img = new FileInfo()
                {
                    Name = this.imageFileResultDish.FullPath
                }
            };
            (int, int) result = await proxy.AddDish(d);
            if (result.Item1 > 0)
            {
                string fileName = $"dishes/D{result.Item1}.jpg";
                await proxy.UploadImage(d.Img, fileName);
                ((App)App.Current).TriggerRestaurantChangedEvent();
                LoadRestaurant(this.restaurant.RestaurantId);
                await ((App)App.Current).MainPage.Navigation.PopModalAsync();
            }
        }
        public Command ShowAnalyticsCommand => new Command(ShowAnalytics);
        private async void ShowAnalytics()
        {

        }

        private async void LoadRestaurant(int restaurantId)
        {
            DishList.Clear();
            (Restaurant, int) result = await proxy.FindRestaurantById(restaurantId);
            if (result.Item1 != null)
            {
                this.Restaurant = result.Item1;
                foreach (Dish d in result.Item1.Dishes)
                    this.DishList.Add(d);
            }  
            else
            {

            }
        }
        #endregion
        #endregion

        public RestaurantPage_ManagerSide_ViewModel(int restaurantId)
        {
            this.proxy = MenUWebAPI.CreateProxy();
            this.DishList = new ObservableCollection<Dish>();
            AllergensList = new ObservableCollection<Allergen>(((App)App.Current).Allergens);
            TagsList = new ObservableCollection<Tag>(((App)App.Current).Tags);
            SelectedAllergens = new ObservableCollection<Allergen>();
            SelectedTags = new ObservableCollection<Tag>();
            LoadRestaurant(restaurantId);
        }
    }
}
