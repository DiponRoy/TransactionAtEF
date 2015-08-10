namespace EfTransaction.Model
{
    public class Address
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public string Description { get; set; }
        public virtual Student Student { get; set; }
    }
}
