using System;
using System.Collections.Generic;

namespace WebApiEF.Models.Data
{
    public partial class Komitent
    {
        public Komitent()
        {
            Racuns = new HashSet<Racun>();
        }

        public int KomitentId { get; set; }
        public string NazivKomitenta { get; set; } = null!;

        public virtual ICollection<Racun> Racuns { get; set; }
    }
}
