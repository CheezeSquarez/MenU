using MenU.Models;
using MenU.Services;
using MenU.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MenU.ViewModels
{
    class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel() { this.proxy = MenUWebAPI.CreateProxy(); }

        #region Attributes
        MenUWebAPI proxy;
        #endregion

        #region Properties and Events

        #endregion

        #region Commands and Methods

        #endregion
    }
}
