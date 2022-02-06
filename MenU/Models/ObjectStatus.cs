using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class ObjectStatus
    {
        public ObjectStatus()
        {
            Accounts = new List<Account>();
            Dishes = new List<Dish>();
            Restaurants = new List<Restaurant>();
            Reviews = new List<Review>();
            StatusId = 0;
            StatusName = "";
        }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public virtual List<Account> Accounts { get; set; }
        public virtual List<Dish> Dishes { get; set; }
        public virtual List<Restaurant> Restaurants { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
