using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class Allergen
    {
        public Allergen()
        {
            AllergenInDishes = new List<AllergenInDish>();
        }
        public int AllergenId { get; set; }
        public string Allergen1 { get; set; }
        public virtual List<AllergenInDish> AllergenInDishes { get; set; }
    }
}
