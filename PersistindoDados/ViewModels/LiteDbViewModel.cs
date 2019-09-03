using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using PersistindoDados.Models;
using PersistindoDados.Services;

namespace PersistindoDados.ViewModels
{
    public class LiteDbViewModel : BaseViewModel
    {
        public ObservableCollection<Pokemon> Pokemons { get; }
        private PokemonService _pokemonService;

        public LiteDbViewModel()
        {
            Pokemons = new ObservableCollection<Pokemon>();
            _pokemonService = new PokemonService();
        }

        public override async Task LoadAsync()
        {
            Ocupado = true;
            try
            {
                var pokemonsAPI = await _pokemonService.GetPokemonsAsync();

                Pokemons.Clear();

                foreach (var pokemon in pokemonsAPI)
                {
                    Pokemons.Add(pokemon);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro", ex.Message);
            }
            finally
            {
                Ocupado = false;
            }

        }
    }

   

}
