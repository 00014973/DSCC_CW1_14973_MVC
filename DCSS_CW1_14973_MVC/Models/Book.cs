using System.ComponentModel.DataAnnotations;

namespace DSCC_CW1_14973_MVC.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        // One-to-many relation with author
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
