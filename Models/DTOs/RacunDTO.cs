using System;
using System.ComponentModel.DataAnnotations;
using WebApiEF.Models.Data;

namespace WebApiEF.Models.DTOs
{
    public class CreateUpdateRacunDTO
    {
        [Required]
        public string BrojDokumenta { get; set; } = null!;

        [Required]
        public int KomitentId { get; set; }

        public virtual ICollection<RacunStavkaDTO>? RacunStavkas { get; set; }
    }

    public class RacunDTO : CreateUpdateRacunDTO
    {
        public RacunDTO()
        {
            RacunStavkas = new HashSet<RacunStavkaDTO>();
        }

        public int RacunId { get; set; }
        public DateTime Datum { get; set; }
        public decimal? Total { get; set; }

        public virtual KomitentDTO Komitent { get; set; } = null!;
    }
}
