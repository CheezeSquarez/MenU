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
using Rg.Plugins.Popup.Services;



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
            //Adds all dishes to a new list
            List<DishDTO> dishList = new List<DishDTO>();
            foreach (DishDTO dish in this.Dishes)
            {
                dishList.Add(dish);
            }
            //Adds all tags into a list
            List<RestaurantTag> tagList = new List<RestaurantTag>();
            foreach (Tag tag in restaurantTags)
            {
                RestaurantTag restaurantTag = new RestaurantTag() { TagId = tag.TagId };
                tagList.Add(restaurantTag);
            }

            //Builds DTO to send to the server
            RestaurantDTO restaurant = new RestaurantDTO()
            {
                Restaurant = new Restaurant()
                {
                    City = this.City,
                    OwnerId = ((App)App.Current).User.AccountId,
                    //RestaurantId = this.restaurantId,
                    RestaurantName = RestaurantName,
                    StreetName = streetName,
                    StreetNumber = HouseNumber,
                    RestaurantStatus = 1,
                    RestaurantPicture = "abc"
                },
                Dishes = dishList,
                RestaurantTags = tagList
            };

            //Dish images need to be added from client/send file. cant read local files from server
            //Sends a server Request and displays a fitting message
            (RestaurantResult, int) registerRestult = await proxy.AddRestaurant(restaurant);
            if (registerRestult.Item1 != null)
            {
                string imgPath = $"banners/B{registerRestult.Item1.Restaurant.RestaurantId}.jpg";
                if (imgChanged)
                {
                    (bool, int) result = await proxy.UploadImage(new FileInfo()
                    {
                        Name = this.imageFileResult.FullPath
                    }, imgPath);
                }
                List<DishDTO> dishes = registerRestult.Item1.Dishes;
                foreach (DishDTO dish in dishes)
                {
                    string fileName = $"dishes/D{dish.Dish.DishId}.jpg";
                    await proxy.UploadImage(dish.Img, fileName);
                }
                await App.Current.MainPage.DisplayAlert("Restaurant Registered Successfully", "Your Restaurant has been successfully been registered and added to our database", "OK");
                ((App)App.Current).TriggerRestaurantAddedEvent();
                App.Current.MainPage = new NavigationPage(new TabControlView());

            }
            App.ErrorHandler(registerRestult.Item2, "");


        }

        public Command GoToAddMenu => new Command(async () =>
        {
        if (SelectedTags.Count > 0 && RestaurantName.Length > 0 && StreetName.Length > 0 && City.Length > 0 && HouseNumber.Length > 0 && imageFileResult != null && imageFileResult.FullPath.Length > 0)
            {
                foreach (Tag tag in SelectedTags)
                {
                    restaurantTags.Add(tag);
                }
                SelectedTags.Clear();
                Push?.Invoke(new AddMenu(this));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Fill In All Fields", "Please make sure to fill in all requested fields. (Make sure to select at least 1 tag)", "OK");


            }
        });
            
    
        
    

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
        public ObservableCollection<DishDTO> Dishes { get; set; }
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
        public async void AddDishMethod()
        {
            if(this.imageFileResultDish == null || this.imageFileResultDish.FullPath.Length == 0)
            {
                await App.Current.MainPage.DisplayAlert("No Image Added", "Please make sure to add an image of the dish.", "OK");
            }
            else
            {
                if (SelectedAllergens.Count > 0 && SelectedTags.Count > 0 && DishName.Length > 0 && Description.Length > 0) 
                {
                    //Defines lists for allergens and tags (for DTO)
                    List<AllergenInDish> allergensInDish = new List<AllergenInDish>();
                    List<DishTag> tagsInDish = new List<DishTag>();
                    //(int, int) dishId = await proxy.StampDish();
                    //if (dishId.Item2 == 200)
                    //{
                    //Adds tags and allergens to lists
                    foreach (Allergen a in SelectedAllergens)
                    {
                        allergensInDish.Add(new AllergenInDish() { AllergenId = a.AllergenId });
                    }
                    foreach (Tag tag in SelectedTags)
                    {
                        tagsInDish.Add(new DishTag() { TagId = tag.TagId });
                    }
                    ImageSource source = "";
                    if (this.imageFileResult != null)
                        source = ImageSource.FromFile(this.imageFileResult.FullPath);
                    //Builds DTO
                    DishDTO d = new DishDTO()
                    {
                        Dish = new Dish()
                        {
                            DishDescription = Description,
                            //DishId = dishId.Item1, 
                            DishName = DishName,
                            DishPicture = "abc",
                            //Restaurant = this.restaurantId 
                        },
                        AllergenInDishes = allergensInDish,
                        Tags = tagsInDish,
                        Img = new FileInfo()
                        {
                            Name = this.imageFileResultDish.FullPath
                        },
                        ImgSource = source
                    };
                    //Adds DTO to list of dished (used to send to server and display)
                    this.Dishes.Add(d);
                    //Clears all info (so that the next dish can be added)
                    Description = "";
                    imageFileResultDish = null;
                    DishName = "";
                    SelectedAllergens.Clear();
                    SelectedTags.Clear();

                    Pop?.Invoke();
                    //}

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Fill In All Fields", "Please make sure to fill in all requested fields. (Make sure to select at least 1 allergen and at least 1 tag)", "OK");
                }
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
            Dishes = new ObservableCollection<DishDTO>();
            proxy = MenUWebAPI.CreateProxy();
            InitTagsList();
            InitAllergensList();
            

        }

        private async void InitTagsList()
        {
            (List<Tag>, int) proxyResult = await proxy.GetAllTags();
            List<Tag> tags = proxyResult.Item1;
            foreach (Tag t in tags)
                this.TagsList.Add(t);
        }
        private async void InitAllergensList()
        {
            (List<Allergen>, int) proxyResult = await proxy.GetAllAllergens();
            List<Allergen> tags = proxyResult.Item1;
            foreach (Allergen t in tags)
                this.AllergensList.Add(t);
        }

        #region Add Banner Img
        private ImageSource imgSource;
        private FileResult imageFileResult;
        private bool imgChanged;
        public Command AddBannerPopUp => new Command(async () => await PopupNavigation.PushAsync(new ChangePfpPopup(this)));
        public ImageSource ImgSource
        {
            get => imgSource;
            set => SetValue(ref imgSource, value);
        }
        public Command OnCamera => new Command(OnCameraMethod);
        private async void OnCameraMethod()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "Take a picture"
            });

            if (result != null)
            {
                this.imageFileResult = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                this.ImgSource = imgSource;
            }
        }

        public Command OnGallery => new Command(OnGalleryMethod);
        private async void OnGalleryMethod()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Pick a photo"
            });

            if (result != null)
            {
                this.imageFileResult = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                this.ImgSource = imgSource;
            }
        }

        public Command SaveImage => new Command(async () =>
        {
            imgChanged = true;
            await PopupNavigation.PopAsync();
        });
        #endregion
    }
}
