using System;
using LiteDB;
using Xamarin.Forms;

namespace PersistindoDados.Models.LiteDB
{
    public class PokemonLTB
    {
        [BsonId]
        public long Id { get; set; }

        public string Name { get; set; }

        public long Height { get; set; }

        public SpritesLDB Sprites { get; set; }

       [BsonIgnore]
        public ImageSource Image { get; set; }
    }

    public class SpritesLDB
    {
        public Uri FrontDefault { get; set; }
    }

}
