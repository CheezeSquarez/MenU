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
    public class TabControlViewModel : BaseViewModel
    {
        public TabControlViewModel()
        {
            ((App)App.Current).Searched += this.GoToSearchTab;
            SelectedIndex = 0;
            Account acc = ((App)App.Current).User;
            if(acc != null)
            {
                IsAdmin = acc.AccountType == 3;
                IsRestaurantOwner = acc.AccountType == 2;
            }
            ((App)App.Current).UserChanged += SetTabs;
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => SetValue(ref selectedIndex, value);
        }
        private bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set => SetValue(ref isAdmin, value);
        }
        private bool isRestaurantOwner;
        public bool IsRestaurantOwner
        {
            get => isRestaurantOwner;
            set => SetValue(ref isRestaurantOwner, value);
        }

        public void GoToSearchTab(string s)
        {
            SelectedIndex = 1;
        }

        public void SetTabs()
        {
            Account acc = ((App)App.Current).User;
            if (acc != null)
            {
                IsAdmin = acc.AccountType == 3;
                IsRestaurantOwner = acc.AccountType == 2;
            }
        }
    }
}
