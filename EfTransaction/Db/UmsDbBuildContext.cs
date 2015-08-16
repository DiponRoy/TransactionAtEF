using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using EfTransaction.Db.Configurations;
using EfTransaction.Model;

namespace EfTransaction.Db
{
    public class UmsDbBuildContext : DbContext, IUmsDbContext
    {
        public IDbSet<Student> Students { get; set; }
        public IDbSet<Address> Addresses { get; set; }

        public UmsDbBuildContext() : base()
        {
        }

        public UmsDbBuildContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public UmsDbBuildContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
        }

        static UmsDbBuildContext()
        {
            Database.SetInitializer(new UmsDbBuildContextInitializer());
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
