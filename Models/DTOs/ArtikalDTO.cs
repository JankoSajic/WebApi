using System;
using System.ComponentModel.DataAnnotations;
using WebApiEF.Config;

namespace WebApiEF.Models.DTOs
{
    public class CreateUpdateArtikalDTO
    {
        [Required, UniqueArtikal]
        public string NazivArtikla { get; set; } = null!;

    }

    public class ArtikalDTO : CreateUpdateArtikalDTO
    {
        public ArtikalDTO()
        {
            RacunStavkas = new HashSet<RacunStavkaDTO>();
        }

        public int ArtikalId { get; set; }

        public virtual ICollection<RacunStavkaDTO>? RacunStavkas { get; set; }
    }
}
