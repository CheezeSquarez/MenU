using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            RestaurantName = "";
            StreetName = "";
            OwnerId = 0;
            City = "";
            RestaurantPicture = "";
            StreetName = "";
            RestaurantStatus = 0;
            Dishes = new List<Dish>();
            RestaurantTags = new List<RestaurantTag>();
        }

        private double CalculateRating()
        {
            int counter = 0;
            int sum = 0;
            foreach (Dish d in this.Dishes)
            {
                foreach (Review review in d.Reviews)
                {
                    counter++;
                    sum += review.Rating;
                }
            }
            if (counter > 0)
                return sum / counter;
            else
                return 0;
        }
        public double AverageRating
        {
            get => CalculateRating();
        }

        private string CalculateOverallRating()
        {
            int counter = 0;
            foreach (Dish d in this.Dishes)
            {
                foreach (Review review in d.Reviews)
                {
                    if (review.IsLiked)
                        counter++;
                    else
                        counter--;
                }
            }
            string[] ratings = { "Extremly bad", "Very bad", "Bad", "Mediocre", "Nice", "Very Good", "Extremly God" };
            if (counter > 3) counter = 3;
            if(counter < -3) counter = -3;
            return ratings[3 + counter];
        }

        public string OverallRating
        {
            get => CalculateOverallRating();
        }

        public int ReviewsCount
        {
            get
            {
                int counter = 0;
                foreach (Dish d in this.Dishes)
                {
                    foreach (Review review in d.Reviews)
                    {
                        counter++;
                    }
                }
                return counter;
            }
        }
        
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string StreetName { get; set; }
        public int OwnerId { get; set; }
        public string City { get; set; }
        public string RestaurantPicture { get; set; }
        public string StreetNumber { get; set; }
        public int RestaurantStatus { get; set; }
        public virtual Account Owner { get; set; }
        public virtual ObjectStatus RestaurantStatusNavigation { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<RestaurantTag> RestaurantTags { get; set; }
        
    }
}
