using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EfTransaction.Model;

namespace EfTransaction.Db.Configurations
{
    internal class StudentConfig : EntityTypeConfiguration<Student>
    {
        public StudentConfig()
        {
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name)
                .HasMaxLength(50);
        }
    }
}
