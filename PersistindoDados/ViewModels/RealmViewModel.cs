using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PersistindoDados.Models.Realm;
using PersistindoDados.Services;
using Realms;
using Xamarin.Forms;

namespace PersistindoDados.ViewModels
{
    public class RealmViewModel : BaseViewModel
    {
        public ObservableCollection<PokemonRealm> Pokemons { get; }
        private PokemonService _pokemonService;

        Realm _realm;
        public RealmViewModel()
        {
            Pokemons = new ObservableCollection<PokemonRealm>();
            _pokemonService = new PokemonService();
            _realm = Realm.GetInstance();
        }

        public override async Task LoadAsync()
        {
            Ocupado = true;
            try
            {
                List<PokemonRealm> pokemonsDB = _realm.All<PokemonRealm>().ToList();

                if (pokemonsDB.Count() == 0)
                {
                    var pokemonsAPI = await _pokemonService.GetPokemonsAsync();

                    foreach (var pokemon in pokemonsAPI)
                    {
                        _realm.Write(() =>
                        {
                            PokemonRealm pokeRealm = new PokemonRealm
                            {
                                Id = pokemon.Id,
                                Name = pokemon.Name.ToUpper(),
                                Height = pokemon.Height,
                                Image = GetImageStreamFromUrl(pokemon.Sprites.FrontDefault.AbsoluteUri)
                            };

                            _realm.Add(pokeRealm);
                        });

                    }

                    pokemonsDB = _realm.All<PokemonRealm>().ToList();
                }



                Pokemons.Clear();

                foreach (var pokemon in pokemonsDB)
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

        public static byte[] GetImageStreamFromUrl(string url)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    var imageBytes = webClient.GetByteArrayAsync(url).Result;

                    return imageBytes;

                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return null;

            }
        }
    }

}
