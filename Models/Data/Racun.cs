using System;
using System.Collections.Generic;

namespace WebApiEF.Models.Data
{
    public partial class Racun
    {
        public Racun()
        {
            RacunStavkas = new HashSet<RacunStavka>();
        }

        public int RacunId { get; set; }
        public string BrojDokumenta { get; set; } = null!;
        public DateTime Datum { get; set; }
        public int KomitentId { get; set; }
        public decimal? Total { get; set; }

        public virtual Komitent Komitent { get; set; } = null!;
        public virtual ICollection<RacunStavka> RacunStavkas { get; set; }
    }
}
