using System;
using System.Threading.Tasks;
using PersistindoDados.ViewModels;
using Xamarin.Forms;

namespace PersistindoDados.Services
{
    public class NavigationService     {          static Lazy<NavigationService> LazyNavi = new Lazy<NavigationService>(() => new NavigationService());         public static NavigationService Current => LazyNavi.Value;           private Page GetViewModelLocator<TViewModel>(params object[] args) where TViewModel : BaseViewModel         {             var viewModelType = typeof(TViewModel);             var viewModelTypeName = viewModelType.Name;             var viewModelWordLength = "ViewModel".Length;              var namespaceName = typeof(BaseViewModel).AssemblyQualifiedName.Split('.')[0];              var viewTypeName = $"{ namespaceName}.{ viewModelTypeName.Substring(0, viewModelTypeName.Length - viewModelWordLength)}Page";             var viewType = Type.GetType(viewTypeName);              var page = Activator.CreateInstance(viewType) as Page;              var viewModel = args != null ? Activator.CreateInstance(viewModelType, args): Activator.CreateInstance(viewModelType);             if (page != null)             {                 page.BindingContext = viewModel;             }              return page;         }          public async Task PushAsync<TViewModel>(bool modal = false, params object[] args) where TViewModel : BaseViewModel         {             var page = GetViewModelLocator<TViewModel>(args);              if (modal)                 await Application.Current.MainPage.Navigation.PushModalAsync(page);             else                 await Application.Current.MainPage.Navigation.PushAsync(page);               await (args.Length > 0 ? (page.BindingContext as BaseViewModel).LoadAsync(args) : (page.BindingContext as BaseViewModel).LoadAsync());         }          public async Task PopAsync() =>            await Application.Current.MainPage.Navigation.PopAsync();          public async Task PopToRootAsync() =>           await Application.Current.MainPage.Navigation.PopToRootAsync();



    } 
}
