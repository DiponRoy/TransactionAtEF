using System.Data.Entity;

namespace EfTransaction.Db
{
    //public class UmsDbContextInitializer : CreateDatabaseIfNotExists<UmsDbContext>
    public class UmsDbContextInitializer : DropCreateDatabaseAlways<UmsDbContext>
    {
    }
}
