namespace Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Order Order { get; set; }
    }
}
