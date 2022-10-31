using WebApiEF.Models.Data;

namespace WebApiEF.Models.DTOs
{
    public class PredRacun
    {
        //public string BrojDokumenta { get; set; } = null!;
        public DateTime Datum { get; set; }
        public int KomitentId { get; set; }

        public  ICollection<PredRacunStavka>? RacunStavkas { get; set; }
    }
}
