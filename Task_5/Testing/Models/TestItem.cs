namespace Task_5.Models
{
    public class TestItem
    {
        public User User { get; set; }
        public Product Product { get; set; }

        public TestItem()
        {
            User = new User();
            Product = new Product();
        }
    }
}
