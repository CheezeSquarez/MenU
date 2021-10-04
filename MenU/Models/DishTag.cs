using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class DishTag
    {
        public int DishId { get; set; }
        public int TagId { get; set; }
        public virtual Dish Dish { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
