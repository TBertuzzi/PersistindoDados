using System;
using Realms;
using Xamarin.Forms;

namespace PersistindoDados.Models.Realm
{
    public class PokemonRealm : RealmObject
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string Name { get; set; }

        public long Height { get; set; }

        public SpritesRealm Sprites { get; set; }

        public byte[] Image { get; set; }

        [Ignored]
        public ImageSource ImageSrc { get; set; }
    }

    public class SpritesRealm : RealmObject
    {
        [PrimaryKey]
        public string IdRealm { get; set; } = Guid.NewGuid().ToString();

        public string FrontDefault { get; set; }
    }
}
