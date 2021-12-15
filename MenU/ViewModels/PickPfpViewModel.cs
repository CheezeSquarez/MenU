using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;

namespace MenU.ViewModels
{
    class ImageInfo
    {
        public string ImageUrl { get; set; }
        public ImageInfo() { }
    }
    class PickPfpViewModel : BaseViewModel
    {
        private string[] URLS = { "imgs\\pfp\\default\\pfp_1.png", "imgs\\pfp\\default\\pfp_10.png", "imgs\\pfp\\default\\pfp_2.png", "imgs\\pfp\\default\\pfp_3.png", "imgs\\pfp\\default\\pfp_4.png", "imgs\\pfp\\default\\pfp_5.png", "imgs\\pfp\\default\\pfp_6.png", "imgs\\pfp\\default\\pfp_7.png", "imgs\\pfp\\default\\pfp_8.png", "imgs\\pfp\\default\\pfp_9.png" };
        public PickPfpViewModel() 
        {
            List<string> temp = URLS.Select(x => "http://10.0.2.2:39135/" + x).ToList();
            //this.proxy = MenUWebAPI.CreateProxy();
            //Task<(List<string>, int)> t = proxy.GetDefaultPfps();
            //t.Wait();
            //if (t.Result.Item1 != null)
            //     temp = t.Result.Item1;
            //else
            //{
            //    //Popup error + push to different page
            //}
            
            temp = temp.Select(x => x.Replace('\\', '/')).ToList();
            //temp.Add("custom");
            pfpSources = new ObservableCollection<ImageInfo>();
            foreach (string s in temp)
                pfpSources.Add(new ImageInfo() { ImageUrl = s });
            
        }

        #region Attributes
        public ObservableCollection<ImageInfo> pfpSources { get; set; }
        MenUWebAPI proxy;
        #endregion

        #region Properties and Events
        #endregion

        #region Commands and Methods
        Command PfpClicked => new Command<string>(SetPfp);
        private async void SetPfp(string source)
        {
            if (source == "custom")
            {

            }
            else
            {

            }
        }

        #endregion
    }
}
