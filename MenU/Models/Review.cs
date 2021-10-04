using System;
using System.Collections.Generic;

namespace MenU.Models
{
    
    public partial class Review
    {
        
        public int ReviewId { get; set; }
        public DateTime PostDate { get; set; }
        public int Dish { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewBody { get; set; }
        public int Rating { get; set; }
        public int Reviewer { get; set; }
        public int ReviewStatus { get; set; }
        public string ReviewPicture { get; set; }
        public bool IsLiked { get; set; }
        public virtual Dish DishNavigation { get; set; }
        public virtual ObjectStatus ReviewStatusNavigation { get; set; }
        public virtual Account ReviewerNavigation { get; set; }
    }
}
