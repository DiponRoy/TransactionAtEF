using System;
using System.Data.Entity;
using EfTransaction.Model;

namespace EfTransaction.Db
{
    public interface IUmsDbContext : IDisposable
    {
        Database Database { get; }

        IDbSet<Student> Students { get; set; }
        IDbSet<Address> Addresses { get; set; }

        int SaveChanges();
    }
}
