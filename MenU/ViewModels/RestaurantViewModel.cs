using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace MenU.ViewModels
{
    public class RestaurantViewModel : BaseViewModel
    {
        private string bannerImg;
        private string name;
        private string top3Tags;
        private string streetName;
        private string strNumber;
        private string city;
        private string dishCount;
        private string reviewCount;

        public string BannerImg
        {
            get => bannerImg;
            set => SetValue(ref bannerImg, value);
        }
        public string Name
        {
            get => name;
            set => SetValue(ref name, value);
        }
        public string Top3Tags
        {
            get => top3Tags;
            set => SetValue(ref top3Tags, value);
        }
        public string StreetName
        {
            get => streetName;
            set => SetValue(ref streetName, value);
        }
        public string StrNumber
        {
            get => strNumber;
            set => SetValue(ref strNumber, value);
        }
        public string City
        {
            get => city;
            set => SetValue(ref city, value);
        }
        public string DishCount
        {
            get => dishCount;
            set => SetValue(ref dishCount, value);
        }
        public string ReviewCount
        {
            get => reviewCount;
            set => SetValue(ref reviewCount, value);
        }
        public event Action<Page> Push;
        public Command<Dish> GoToDish => new Command<Dish>((d) => Push?.Invoke(new DishPage(d)));
        public ObservableCollection<Dish> Dishes { get; set; }
        public RestaurantViewModel(Restaurant r)
        {
            Random random = new Random();
            this.BannerImg = $"{MenUWebAPI.DEFAULT_IMG_URI}/banners/B{r.RestaurantId}.jpg?" + random.Next();
            this.Name = r.RestaurantName;
            this.City = r.City;
            this.ReviewCount = r.ReviewsCount.ToString();
            this.StreetName = r.StreetName;
            this.StrNumber = r.StreetNumber;
            this.DishCount = r.Dishes.Count.ToString();
            List<Tag> tags = new List<Tag>();
            foreach(RestaurantTag rt in r.RestaurantTags)
            {
                tags.Add(((App)App.Current).Tags.FirstOrDefault(x => x.TagId == rt.TagId));
            }
            this.Top3Tags = String.Join(", ", tags.Take(3).ToList());

            Dishes = new ObservableCollection<Dish>();
            foreach (Dish d in r.Dishes)
                Dishes.Add(d);

        }
    }
}
