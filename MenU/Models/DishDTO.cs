using System;
using System.Collections.Generic;
using System.Text;

namespace MenU.Models
{
    public class DishDTO
    {
        public DishDTO() { }

        public Dish Dish { get; set; }
        public List<DishTag> Tags { get; set; }
        public List<AllergenInDish> AllergenInDishes { get; set; }
    }
}
