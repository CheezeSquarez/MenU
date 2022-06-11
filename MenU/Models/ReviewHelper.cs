using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using MenU.Models;
using MenU.Services;
using MenU.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace MenU.Models
{
    public class ReviewHelper : BaseViewModel
    {
        public ReviewHelper(Review r)
        {
            this.ReviewId = r.ReviewId;
            this.PostDate = r.PostDate;
            this.Dish = r.Dish;
            this.ReviewTitle = r.ReviewTitle;
            this.ReviewBody = r.ReviewBody;
            this.Rating = r.Rating;
            this.Reviewer = r.Reviewer;
            this.ReviewStatus = r.ReviewStatus;
            this.HasPicture = r.HasPicture;
            this.IsLiked = r.IsLiked;
            this.ExpandImgSource = EXPAND_MORE;
            this.StackVisibility = false;
            this.DishNavigation = r.DishNavigation;
        }
        public int ReviewId { get; set; }
        public DateTime PostDate { get; set; }
        public int Dish { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewBody { get; set; }
        public int Rating { get; set; }
        public int Reviewer { get; set; }
        public int ReviewStatus { get; set; }
        public bool HasPicture { get; set; }
        public bool IsLiked { get; set; }
        public virtual Dish DishNavigation { get; set; }
        public virtual ObjectStatus ReviewStatusNavigation { get; set; }
        public virtual Account ReviewerNavigation { get; set; }

        public string[] Stars
        {
            get
            {
                string[] stars = new string[5];
                for (int i = 0; i < Rating; i++)
                {
                    stars[i] = "icon_star_filled.png";
                }
                int rest = 5 - this.Rating;
                for (int i = 5 - rest; i < 5; i++)
                {
                    stars[i] = "icon_star_empty.png";
                }
                return stars;
            }
        }

        public string ImgSource
        {
            get
            {
                Random r = new Random();
                string source = $"{MenU.Services.MenUWebAPI.DEFAULT_IMG_URI}reviews/R{this.ReviewId}.jpg?{r.Next()}";
                return source;
            }
        }

        public string LikedImgSource
        {
            get
            {
                if (this.IsLiked)
                    return "icon_like_filled.png";
                else
                    return "icon_dislike.png";
            }
        }

        private bool stackVisibility;
        public bool StackVisibility
        {
            get => stackVisibility;
            set => SetValue(ref stackVisibility, value);
        }
        private string expandImgSource;
        public string ExpandImgSource
        {
            get => expandImgSource;
            set => SetValue(ref expandImgSource, value);
        }
        private const string EXPAND_MORE = "icon_expand_more.svg";
        private const string EXPAND_LESS = "icon_expand_less.svg";
        public Command ExpandPressed => new Command(() =>
        {
            if (StackVisibility)
                ExpandImgSource = EXPAND_MORE;
            else
                ExpandImgSource = EXPAND_LESS;
            StackVisibility = !StackVisibility;
        });
    }
}
