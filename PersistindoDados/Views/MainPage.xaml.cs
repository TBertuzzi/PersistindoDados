using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersistindoDados.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PersistindoDados
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new MainViewModel();

            if (Preferences.Get("PrimeiraExecucao", true))
            {
                lvlPrimeira.IsVisible = true;
                Preferences.Set("PrimeiraExecucao", false);
            }
        }
    }
}
