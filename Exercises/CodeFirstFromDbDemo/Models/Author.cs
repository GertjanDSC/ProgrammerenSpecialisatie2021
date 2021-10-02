using System;
using System.Collections.Generic;

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
