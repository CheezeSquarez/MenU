using System;
using System.Collections.Generic;

namespace MenU.Models
{
    public partial class Tag
    {
        public Tag()
        {
            AccountTags = new List<AccountTag>();
            DishTags = new List<DishTag>();
            RestaurantTags = new List<RestaurantTag>();
        }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public virtual List<AccountTag> AccountTags { get; set; }
        public virtual List<DishTag> DishTags { get; set; }
        public virtual List<RestaurantTag> RestaurantTags { get; set; }
        public override string ToString()
        {
            return this.TagName;
        }
    }
}
