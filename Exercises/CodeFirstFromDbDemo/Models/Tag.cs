using System;
using System.Collections.Generic;

#nullable disable

namespace CodeFirstFromDbDemo.Models
{
    public partial class Tag
    {
        public Tag()
        {
            CourseTags = new HashSet<CourseTag>();
        }

        public int TagId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CourseTag> CourseTags { get; set; }
    }
}
