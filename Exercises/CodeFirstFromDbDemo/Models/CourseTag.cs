using System;
using System.Collections.Generic;

#nullable disable

namespace CodeFirstFromDbDemo.Models
{
    public partial class CourseTag
    {
        public int CourseId { get; set; }
        public int TagId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
