using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PersistindoDados.Models;
using Xamarin.Helpers;

namespace PersistindoDados.Services
{

    public class PokemonService     {         public async Task<List<Pokemon>> GetPokemonsAsync()         {
            List<Pokemon> pokemons = new List<Pokemon>();

            try
            {
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string ts = DateTime.Now.Ticks.ToString();

                for (int i = 0; i < 20; i++)
                {
                    var response = await httpClient.
                        GetAsync<Pokemon>($"https://pokeapi.co/api/v2/pokemon/{i.ToString()}");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        pokemons.Add(response.Value);
                    }
                    else
                    {
                        Debug.WriteLine(response.Error.Message);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return pokemons;         }     } 
}
