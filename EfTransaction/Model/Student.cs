using System.Collections.Generic;

namespace EfTransaction.Model
{
    public class Student
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
