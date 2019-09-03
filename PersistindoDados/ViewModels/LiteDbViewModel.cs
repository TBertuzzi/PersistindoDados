using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using LiteDB;
using PersistindoDados.Models;
using PersistindoDados.Models.LiteDB;
using PersistindoDados.Services;
using Xamarin.Essentials;

namespace PersistindoDados.ViewModels
{
    public class LiteDbViewModel : BaseViewModel
    {
        public ObservableCollection<PokemonLTB> Pokemons { get; }
        private PokemonService _pokemonService;
        LiteDatabase _dataBase;

        public LiteDbViewModel()
        {
            Pokemons = new ObservableCollection<PokemonLTB>();
            _pokemonService = new PokemonService();

            _dataBase = new LiteDatabase(Path.Combine(FileSystem.AppDataDirectory, "MeuBanco.db"));
        }

        public override async Task LoadAsync()
        {
            Ocupado = true;
            try
            {
                LiteCollection<PokemonLTB> pokemonsDB = _dataBase.GetCollection<PokemonLTB>();

                if(pokemonsDB.Count() == 0)
                {
                    var pokemonsAPI = await _pokemonService.GetPokemonsAsync();

                    foreach (var pokemon in pokemonsAPI)
                    {
                        PokemonLTB pokeLTB = new PokemonLTB
                        {
                            Id = pokemon.Id,
                            Name = pokemon.Name.ToUpper()
                        };

                        pokemonsDB.Upsert(pokeLTB);
                    }

                    pokemonsDB = _dataBase.GetCollection<PokemonLTB>();
                }

               

                Pokemons.Clear();

                foreach (var pokemon in pokemonsDB.FindAll())
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
