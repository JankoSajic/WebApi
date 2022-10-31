using System;
using System.ComponentModel.DataAnnotations;
using WebApiEF.Models.Data;
using WebApiEF.Repository;
namespace WebApiEF.Config
{
    public class UniqueArtikalAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var uow = new UnitOfWork(new TestDbContext());
            var check = uow.Artikli.Exists(entity => entity.NazivArtikla == value as string);
            if (check)
                return false;
            else
                return true;
        }
    }
}