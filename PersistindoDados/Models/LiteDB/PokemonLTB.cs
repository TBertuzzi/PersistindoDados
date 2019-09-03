using System;
using LiteDB;

namespace PersistindoDados.Models.LiteDB
{
    public class PokemonLTB
    {
        [BsonId]
        public long Id { get; set; }

        public string Name { get; set; }

        public SpritesLDB Sprites { get; set; }
    }

    public class SpritesLDB
    {
        public Uri FrontDefault { get; set; }
    }

}
