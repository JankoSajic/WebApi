using Microsoft.EntityFrameworkCore;
using System;
using WebApiEF.Models.Data;

namespace WebApiEF.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        // TestDb
        private readonly TestDbContext _testDbContext;

        public UnitOfWork(TestDbContext testDbContext)
            => _testDbContext = testDbContext;

        public IRepository<Artikal> Artikli
            => new Repository<Artikal>(_testDbContext);

        public IRepository<Komitent> Komitenti
            => new Repository<Komitent>(_testDbContext);

        public IRepository<Racun> Racuni
            => new Repository<Racun>(_testDbContext);

        public IRepository<RacunStavka> RacunStavke
            => new Repository<RacunStavka>(_testDbContext);

        public async Task<int> Save()
            => await _testDbContext.SaveChangesAsync();

        public void Dispose()
        {
            _testDbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
