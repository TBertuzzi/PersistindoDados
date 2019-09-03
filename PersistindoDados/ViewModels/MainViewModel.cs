using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PersistindoDados.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Command LiteDBCommand { get; }
        public Command RealmCommand { get; }
        public Command MonkeyCacheCommand { get; }

        public MainViewModel()
        {
            LiteDBCommand = new Command(ExecuteLiteDBCommand);
            RealmCommand = new Command(ExecuteLiteDBCommand);
            MonkeyCacheCommand = new Command(ExecuteLiteDBCommand);
        }

        private async void ExecuteLiteDBCommand()
        {
            await Navigation.PushAsync<LiteDbViewModel>(false);
        }
        private async void ExecuteRealmCommand()
        {
            await Navigation.PushAsync<RealmViewModel>(false);
        }
        private async void ExecuteMonkeyCacheCommand()
        {
            await Navigation.PushAsync<MonkeyCacheViewModel>(false);
        }
    }
}
