using System;
using WebApiEF.Models.Data;

namespace WebApiEF.Repository
{
    public interface IUnitOfWork
    {
        // TestDb
        IRepository<Artikal> Artikli { get; }
        IRepository<Komitent> Komitenti { get; }
        IRepository<Racun> Racuni { get; }
        IRepository<RacunStavka> RacunStavke { get; }

        Task<int> Save();
    }
}
