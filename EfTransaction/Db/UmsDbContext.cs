using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using EfTransaction.Db.Configurations;
using EfTransaction.Model;

namespace EfTransaction.Db
{
    public class UmsDbContext : DbContext, IUmsDbContext
    {
        public IDbSet<Student> Students { get; set; }
        public IDbSet<Address> Addresses { get; set; }

        public UmsDbContext() : base()
        {
        }

        public UmsDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public UmsDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
        }

        static UmsDbContext()
        {
            Database.SetInitializer(new UmsDbContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new StudentConfig());
            modelBuilder.Configurations.Add(new AddressConfig());
        }

        public void Load()
        {
            Students.Count();
            Addresses.Count();
        }
    }
}
