using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LiteDB;
using PersistindoDados.Models;
using PersistindoDados.Models.LiteDB;
using PersistindoDados.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

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
                            Name = pokemon.Name.ToUpper(),
                            Height = pokemon.Height
                        };

                        pokemonsDB.Upsert(pokeLTB);

                        using (Stream stream = GetImageStreamFromUrl(pokemon.Sprites.FrontDefault.AbsoluteUri))
                        {
                            if (stream != null)
                            {
                                //Verfica se ja existe a imagem,se existir apaga
                                if (_dataBase.FileStorage.Exists(pokemon.Id.ToString()))
                                {
                                    _dataBase.FileStorage.Delete(pokemon.Id.ToString());
                                }
                                _dataBase.FileStorage.Upload(pokemon.Id.ToString(), pokemon.Name, stream);

                            }
                        }
                    }

                    pokemonsDB = _dataBase.GetCollection<PokemonLTB>();
                }

               

                Pokemons.Clear();

                foreach (var pokemon in pokemonsDB.FindAll())
                {
                    pokemon.Image = ImageSource.FromStream(() => _dataBase.FileStorage.FindById(pokemon.Id.ToString()).OpenRead());
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

        private Stream GetImageStreamFromUrl(string url)
        {
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    Stream stream = new MemoryStream(imageBytes);
                    return stream;
                }
            }
            return null;
        }
    }

   

}
