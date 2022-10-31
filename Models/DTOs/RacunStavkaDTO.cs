using System;
using System.ComponentModel.DataAnnotations;
using WebApiEF.Models.Data;

namespace WebApiEF.Models.DTOs
{
    public class RacunStavkaDTO
    {
        [Required]
        public decimal Kolicina { get; set; }

        [Required]
        public decimal Cena { get; set; }

        [Required]
        public int ArtikalId { get; set; }
    }
}
