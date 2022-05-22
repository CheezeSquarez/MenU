using System;
using System.Collections.Generic;

namespace MenU.Models
{
    
    public partial class Review
    {
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

        public string ImgSource { get
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
                    return "icon_like_empty.png";
            }
        }
    }
}
