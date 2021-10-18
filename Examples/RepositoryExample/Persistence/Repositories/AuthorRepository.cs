﻿using Microsoft.EntityFrameworkCore;
using Queries.Core.Domain;
using Queries.Core.Repositories;
using System.Linq;

namespace Queries.Persistence.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(CourseContext context) : base(context)
        {
        }

        public Author GetAuthorWithCourses(int id)
        {
            return CourseContext.Authors.Include(a => a.Courses).SingleOrDefault(a => a.Id == id);
        }

        public CourseContext CourseContext
        {
            get { return Context as CourseContext; }
        }
    }
}