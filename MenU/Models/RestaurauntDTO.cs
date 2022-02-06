using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class RestaurantDTO
    {
        public RestaurantDTO() { }

        public RestaurantDTO(Restaurant r, List<RestaurantTag> lst, List<DishDTO> dishes )
        {
            Restaurant = r;
            RestaurantTags = lst;
            Dishes = dishes;
        }

        public Restaurant Restaurant { get; set; }
        public List<RestaurantTag> RestaurantTags { get; set; }
        public List<DishDTO> Dishes { get; set; }

    }
}
