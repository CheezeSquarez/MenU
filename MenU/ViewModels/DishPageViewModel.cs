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
    public class DishPageViewModel : BaseViewModel
    {
        #region Attributes
        private Dish dish;
        private string dishName;
        private string dishDescription;
        private string imgSource;
        private string heartSource;
        private const string FILLED_HEART = "icon_heart_filled.png";
        private const string OUTLINE_HEART = "icon_heart_outline.png";
        private MenUWebAPI proxy;
        #endregion

        #region Properties

        public string DishName
        {
            get => dishName;
            set => SetValue(ref dishName, value);
        }
        public string DishDescription
        {
            get => dishDescription;
            set => SetValue(ref dishDescription, value);
        }
        public string ImgSource 
        {
            get => imgSource;
            set => SetValue(ref imgSource, value);
        }
        public string HeartSource
        {
            get => heartSource;
            set => SetValue(ref heartSource, value);
        }
        public ObservableCollection<Tag> FilteredTags { get; set; }
        public ObservableCollection<Allergen> FilteredAllergens { get; set; }

        public ObservableCollection<Review> Reviews { get; set; }
        #endregion
        public DishPageViewModel(Dish dish)
        {
            proxy = MenUWebAPI.CreateProxy();
            this.dish = dish;
            this.dishName = dish.DishName;
            this.DishDescription = dish.DishDescription;
            this.FilteredTags = new ObservableCollection<Tag>();
            this.FilteredAllergens = new ObservableCollection<Allergen>();
            Random random = new Random();
            ImgSource = MenUWebAPI.DEFAULT_IMG_URI + "dishes/D" + dish.DishId + ".jpg?" + random.Next();
            //imgSource = "pasta.jpg";
            heartSource = OUTLINE_HEART;
            //Filters out the tags and allergens that appear in the dish
            foreach (DishTag dt in dish.DishTags)
            {
                Tag t = ((App)App.Current).Tags.FirstOrDefault(x => x.TagId == dt.TagId);
                FilteredTags.Add(t);
            }
            foreach(AllergenInDish ag in dish.AllergenInDishes)
            {
                Allergen allergen = ((App) App.Current).Allergens.FirstOrDefault(x => x.AllergenId == ag.AllergenId);
                FilteredAllergens.Add(allergen);
            }
            Reviews = new ObservableCollection<Review>();
            LoadReviews();
        }

        public Command AddReviewTapped => new Command(() => { App.Current.MainPage.Navigation.PushAsync(new AddReview(this.dish.DishId)); });
        public Command HeartTapped => new Command(HeartTappedMethod);
        private void HeartTappedMethod()
        {
            if (HeartSource == FILLED_HEART)
            {
                HeartSource = OUTLINE_HEART;
            }
            else
            {
                HeartSource = FILLED_HEART;
            }
                
        }

        public async void LoadReviews()
        {
            Reviews.Clear();
            (List<Review>, int) result = await proxy.GetDishReviews(this.dish.DishId);
            List<Review> reviews = result.Item1;
            foreach (Review review in reviews)
            {
                Reviews.Add(review);
            }
        }
    }
}
