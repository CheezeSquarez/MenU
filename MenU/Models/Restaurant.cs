﻿using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class Restaurant
    {
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
