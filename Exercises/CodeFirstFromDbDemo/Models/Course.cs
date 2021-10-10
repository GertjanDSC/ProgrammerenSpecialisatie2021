using System.Collections.Generic;

#nullable disable

namespace CodeFirstFromDbDemo.Models
{

    public partial class Course
    {
        public Course()
        {
            CourseSections = new HashSet<CourseSection>();
            CourseTags = new HashSet<CourseTag>();
        }

        public int CourseId { get; set; }
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short Price { get; set; }
        public string LevelString { get; set; }
        public byte Level { get; set; }

        public virtual Author Author { get; set; }
        public virtual ICollection<CourseSection> CourseSections { get; set; }
        public virtual ICollection<CourseTag> CourseTags { get; set; }
    }
}
