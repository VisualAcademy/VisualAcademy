namespace Domain.Entities.Samples
{
    //[EntityTypeConfiguration(typeof(BookConfiguration))]
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
    }
}
