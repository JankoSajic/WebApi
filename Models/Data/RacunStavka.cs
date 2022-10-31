using System;
using System.Collections.Generic;

namespace WebApiEF.Models.Data
{
    public partial class RacunStavka
    {
        public int RacunStavkaId { get; set; }
        public decimal Kolicina { get; set; }
        public decimal Cena { get; set; }
        public int ArtikalId { get; set; }
        public int RacunId { get; set; }

        public virtual Artikal Artikal { get; set; } = null!;
        public virtual Racun Racun { get; set; } = null!;
    }
}
