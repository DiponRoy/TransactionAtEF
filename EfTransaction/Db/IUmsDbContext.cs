using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EfTransaction.Model;

namespace EfTransaction.Db
{
    public interface IUmsDbContext : IDisposable
    {
        IDbSet<Student> Students { get; set; }
        IDbSet<Address> Addresses { get; set; }

        Database Database { get; }
        DbEntityEntry Entry(object entity);
        int SaveChanges();
    }
}
