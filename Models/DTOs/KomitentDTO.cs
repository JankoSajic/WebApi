using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiEF.Models.DTOs
{
    public class CreateUpdateKomitentDTO
    {
        [Required]
        public string NazivKomitenta { get; set; } = null!;
    }

    public class KomitentDTO : CreateUpdateKomitentDTO
    {
        public KomitentDTO()
        {
            Racuns = new HashSet<RacunDTO>();
        }

        public int KomitentId { get; set; }

        public virtual ICollection<RacunDTO> Racuns { get; set; }
    }
}
