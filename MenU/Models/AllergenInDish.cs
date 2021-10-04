using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class AllergenInDish
    {
        public int AllergenId { get; set; }
        public int DishId { get; set; }
        public virtual Allergen Allergen { get; set; }
        public virtual Dish Dish { get; set; }
    }
}
