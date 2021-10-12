using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CodeFirstFromDbDemo.Models
{
    public partial class Author
    {
        public Author()
        {
            Courses = new HashSet<Course>();
        }

        public int AuthorId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
