using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenU.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenU.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabControlView : ContentPage
    {
        public TabControlView()
        {
            TabControlViewModel viewModel = new TabControlViewModel();
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}