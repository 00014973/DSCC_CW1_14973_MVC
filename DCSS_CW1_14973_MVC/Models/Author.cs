using System.ComponentModel.DataAnnotations;

namespace DSCC_CW1_14973_MVC.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        // One-to-many relationship with books.
        public List<Book>? Books { get; set; } = new List<Book>();
    }
}
