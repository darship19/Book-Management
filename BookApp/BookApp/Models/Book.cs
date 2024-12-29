using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateOnly PublicationDate { get; set; }
    }
}
