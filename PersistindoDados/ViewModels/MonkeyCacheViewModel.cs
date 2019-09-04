using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MonkeyCache.SQLite;
using PersistindoDados.Models.LiteDB;
using PersistindoDados.Services;

namespace PersistindoDados.ViewModels
{
    public class MonkeyCacheViewModel : BaseViewModel
    {
        public ObservableCollection<PokemonLTB> Pokemons { get; }
        private PokemonService _pokemonService;

        private string _key = "PokemonCache"; //Chave com o nome do objeto que armazena os dados.

        public MonkeyCacheViewModel()
        {
            Pokemons = new ObservableCollection<PokemonLTB>();
            _pokemonService = new PokemonService();

            CarregaPokemons();
        }

        private async Task CarregaPokemons()
        {
            Ocupado = true;
            Pokemons.Clear();

            var existingList = Barrel.Current.Get<List<PokemonLTB>>(_key) ?? new List<PokemonLTB>();

            if (existingList.Count == 0)
                await GravarPokemons();
            else
            {
                var pokemonsCache = Barrel.Current.Get<List<PokemonLTB>>(_key);

                foreach (var pokemon in pokemonsCache)
                {
                    Pokemons.Add(pokemon);
                }

            }
            Ocupado = false;
        }

        private async Task GravarPokemons()
        {
            Ocupado = true;

            var pokemonsAPI = await _pokemonService.GetPokemonsAsync();

            Pokemons.Clear();

            var existingList = Barrel.Current.Get<List<PokemonLTB>>(_key) ?? new List<PokemonLTB>();

            foreach (var pokemon in pokemonsAPI)
            {
                var isExist = existingList.Any(e => e.Id == pokemon.Id);

                PokemonLTB pokeLTB = new PokemonLTB
                {
                    Id = pokemon.Id,
                    Name = pokemon.Name.ToUpper(),
                    Height = pokemon.Height,
                    ImageByte = GetImageStreamFromUrl(pokemon.Sprites.FrontDefault.AbsoluteUri)
                };

                if (!isExist)
                {
                    existingList.Add(pokeLTB);
                }

                Pokemons.Add(pokeLTB);
            }

            existingList = existingList.ToList();

            Barrel.Current.Add(_key, existingList, TimeSpan.FromDays(30));

            Ocupado = false;
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
