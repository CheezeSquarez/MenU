using System;
using System.Collections.Generic;
namespace MenU.Models
{
    public partial class RestaurantTag
    {
        public RestaurantTag()
        {
            TagId = 0;
            RestaurantId = 0;
        }
        public int TagId { get; set; }
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
