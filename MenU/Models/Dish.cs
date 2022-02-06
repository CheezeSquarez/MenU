using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class Dish
    {
        public Dish()
        {
            AllergenInDishes = new List<AllergenInDish>();
            DishTags = new List<DishTag>();
            Reviews = new List<Review>();
            DishId = 0;
            DishName = "";
            DishDescription = "";
            Restaurant = 0;
            DishStatus = 0;
            DishPicture = "";
        }

        public int DishId { get; set; }
        public string DishName { get; set; }
        public string DishDescription { get; set; }
        public int Restaurant { get; set; }
        public int DishStatus { get; set; }
        public string DishPicture { get; set; }
        public virtual ObjectStatus DishStatusNavigation { get; set; }
        public virtual Restaurant RestaurantNavigation { get; set; }
        public virtual ICollection<AllergenInDish> AllergenInDishes { get; set; }
        public virtual ICollection<DishTag> DishTags { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
