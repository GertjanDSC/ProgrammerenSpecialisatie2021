using System;
using System.Collections.Generic;

#nullable disable

namespace CodeFirstFromDbDemo.Models
{
    public partial class CourseSection
    {
        public int SectionId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }

        public virtual Course Course { get; set; }
    }
}
