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
    public class AddRatingViewModel : BaseViewModel
    {
        private int selectedStarsNum;
        private string reviewBody;
        private string reviewTitle;
        private string likedSource;
        private string dishName;
        private bool isLiked;
        private const string ICON_LIKED_FILLED = "icon_like_filled.png";
        private const string ICON_LIKED_EMPTY = "icon_like_empty.png";
        private ImageSource imgSource;
        private FileResult imageFileResult;
        private bool hasImage;
        private int dishId;
        private MenUWebAPI proxy;

        public int SelectedStarsNum
        {
            get => selectedStarsNum;
            set => SetValue(ref selectedStarsNum, value);
        }

        public string ReviewBody
        {
            get => reviewBody;
            set => SetValue(ref reviewBody, value);
        }

        public string ReviewTitle
        {
            get => reviewTitle;
            set => SetValue(ref reviewTitle, value);
        }

        public string LikedSource
        {
            get => likedSource;
            set => SetValue(ref likedSource, value);
        }

        public string DishName
        {
            get => dishName;
            private set => SetValue(ref dishName, value);
        }

        public ImageSource ImgSource
        {
            get => imgSource;
            set => SetValue(ref imgSource, value);
        }

        public Command ToggleLiked => new Command(ToggleLikedMethod);
        private void ToggleLikedMethod()
        {
            isLiked = !isLiked;
            if (isLiked)
                LikedSource = ICON_LIKED_FILLED;
            else
                LikedSource = ICON_LIKED_EMPTY;

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
                hasImage = true;
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
                hasImage = true;
            }
        }

        public Command PostReview => new Command(PostReviewMethod);
        private async void PostReviewMethod()
        {
            Review r = new Review()
            {
                Dish = this.dishId,
                HasPicture = this.hasImage,
                PostDate = DateTime.Now,
                Rating = this.SelectedStarsNum,
                ReviewBody = this.ReviewBody,
                IsLiked = this.isLiked,
                Reviewer = ((App)App.Current).User.AccountId,
                ReviewTitle = this.ReviewTitle
            };

            (int, int) result = await proxy.PostReview(r);

            if (result.Item1 > 0 && hasImage)
                await proxy.UploadImage(new FileInfo()
                {
                    Name = this.imageFileResult.FullPath
                }, "reviews/R" + result.Item1 + ".jpg");

            await App.Current.MainPage.Navigation.PopAsync();
        }

        public AddRatingViewModel(int DishId)
        {
            proxy = MenUWebAPI.CreateProxy();
            GetDish(DishId);
            hasImage = false;
            ImgSource = ImageSource.FromFile("icon_take_picture.png");
            LikedSource = ICON_LIKED_EMPTY;
            selectedStarsNum = 0;
        }

        public async void GetDish(int id) 
        {
            (Dish, int) dish = await proxy.GetDishById(id);
            if(dish.Item1 != null)
            {
                this.DishName = dish.Item1.DishName;
                this.dishId = dish.Item1.DishId;
            }
        }
    }
}
