using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Text.Json.Serialization;
namespace MenU.Models
{
    public class DishDTO
    {
        public DishDTO() { }

        public Dish Dish { get; set; }
        public List<DishTag> Tags { get; set; }
        public List<AllergenInDish> AllergenInDishes { get; set; }
        public FileInfo Img { get; set; }

        [JsonIgnore]
        public ImageSource ImgSource { get; set; }
    }
}
