using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PersistindoDados.Services;

namespace PersistindoDados.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private string _titulo;
        public string Titulo
        {
            get { return _titulo; }
            set
            {
                SetProperty(ref _titulo, value);
            }
        }

        private bool _ocupado;
        public bool Ocupado
        {
            get { return _ocupado; }
            set
            {
                SetProperty(ref _ocupado, value);
            }
        }

        public virtual Task LoadAsync(object[] args) => Task.FromResult(true);

        public virtual Task LoadAsync() => Task.FromResult(true);

        protected NavigationService Navigation => NavigationService.Current;
    }
}
