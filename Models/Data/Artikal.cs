using System;
using System.Collections.Generic;

namespace WebApiEF.Models.Data
{
    public partial class Artikal
    {
        public Artikal()
        {
            RacunStavkas = new HashSet<RacunStavka>();
        }

        public int ArtikalId { get; set; }
        public string NazivArtikla { get; set; } = null!;

        public virtual ICollection<RacunStavka> RacunStavkas { get; set; }
    }
}
