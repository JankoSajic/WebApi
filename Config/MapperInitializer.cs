using AutoMapper;
using System;
using WebApiEF.Models.Data;
using WebApiEF.Models.DTOs;

namespace WebApiEF.Models
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Artikal, ArtikalDTO>().ReverseMap();
            CreateMap<Artikal, CreateUpdateArtikalDTO>().ReverseMap();
            CreateMap<Komitent, KomitentDTO>().ReverseMap();
            CreateMap<Komitent, CreateUpdateKomitentDTO>().ReverseMap();
            CreateMap<Racun, RacunDTO>().ReverseMap();
            CreateMap<RacunStavka, RacunStavkaDTO>().ReverseMap();
            CreateMap<Racun, PredRacun>().ReverseMap();
            CreateMap<RacunStavka, PredRacunStavka>().ReverseMap();
            CreateMap<PredRacun, Racun>().ReverseMap();
            CreateMap<PredRacunStavka, RacunStavka>().ReverseMap();
        }
    }
}
