using System;
using System.Collections.Generic;
using System.Text;

namespace MenU.Models
{
    public class RestaurantResult
    {
        public Restaurant Restaurant { get; set; }
        public List<DishDTO> Dishes { get; set;}
    }
}
