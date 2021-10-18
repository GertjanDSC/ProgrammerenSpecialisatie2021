using Microsoft.EntityFrameworkCore;
using Queries.Core.Domain;
using Queries.Persistence;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Queries
{
    class Program
    {
        /// <summary>
        /// Deletes table contents every run and resets identity to 1 for all tables
        /// </summary>
        /// <param name="context"></param>
        // LVET TODO: adaptable to unit of work ...
        static private void Seed(CourseContext context)
        {
            #region Clear tables, reset identity keys
            context.Database.ExecuteSqlRaw("DELETE FROM CourseTag");
            context.Database.ExecuteSqlRaw("DELETE FROM Authors");
            context.Database.ExecuteSqlRaw("DELETE FROM Courses");
            context.Database.ExecuteSqlRaw("DELETE FROM Cover");
            context.Database.ExecuteSqlRaw("DELETE FROM Tags");
            // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('CourseTag', RESEED, 0)"); // no identity column
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Tags', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Courses', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Authors', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Cover', RESEED, 0)");
            #endregion

            #region Add Tags
            var tags = new Dictionary<string, Tag>
            {
                {"c#", new Tag {/*Id = 1,*/ Name = "c#"}},
                {"angularjs", new Tag {/*Id = 2,*/ Name = "angularjs"}},
                {"javascript", new Tag {/*Id = 3,*/ Name = "javascript"}},
                {"nodejs", new Tag {/*Id = 4,*/ Name = "nodejs"}},
                {"oop", new Tag {/*Id = 5,*/ Name = "oop"}},
                {"linq", new Tag {/*Id = 6,*/ Name = "linq"}},
            };

            foreach (var tag in tags.Values)
                context.Tags.Add(tag);
            #endregion

            #region Add Authors
            var authors = new List<Author>
            {
                new Author
                {
                    //Id = 1,
                    Name = "Luc Vervoort"
                },
                new Author
                {
                    //Id = 2,
                    Name = "Tom Vande Wiele"
                },
                new Author
                {
                    //Id = 3,
                    Name = "Yanic Inghelbrecht"
                },
                new Author
                {
                    //Id = 4,
                    Name = "Wim Goedertier"
                },
                new Author
                {
                    //Id = 5,
                    Name = "Annick Burms"
                }
            };

            foreach (var author in authors)
                context.Authors.Add(author);
            #endregion

            #region Add Courses
            var courses = new List<Course>
            {
                new Course
                {
                    //Id = 1,
                    Name = "C# Basics",
                    Author = authors[0],
                    FullPrice = 49,
                    Description = "Description for C# Basics",
                    Level = 1,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"]
                    }
                },
                new Course
                {
                    //Id = 2,
                    Name = "C# Intermediate",
                    Author = authors[0],
                    FullPrice = 49,
                    Description = "Description for C# Intermediate",
                    Level = 2,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"],
                        tags["oop"]
                    }
                },
                new Course
                {
                    //Id = 3,
                    Name = "C# Advanced",
                    Author = authors[0],
                    FullPrice = 69,
                    Description = "Description for C# Advanced",
                    Level = 3,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"]
                    }
                },
                new Course
                {
                    //Id = 4,
                    Name = "Javascript: Understanding the Weird Parts",
                    Author = authors[1],
                    FullPrice = 149,
                    Description = "Description for Javascript",
                    Level = 2,
                    Tags = new Collection<Tag>()
                    {
                        tags["javascript"]
                    }
                },
                new Course
                {
                    //Id = 5,
                    Name = "Learn and Understand AngularJS",
                    Author = authors[1],
                    FullPrice = 99,
                    Description = "Description for AngularJS",
                    Level = 2,
                    Tags = new Collection<Tag>()
                    {
                        tags["angularjs"]
                    }
                },
                new Course
                {
                    //Id = 6,
                    Name = "Learn and Understand NodeJS",
                    Author = authors[1],
                    FullPrice = 149,
                    Description = "Description for NodeJS",
                    Level = 2,
                    Tags = new Collection<Tag>()
                    {
                        tags["nodejs"]
                    }
                },
                new Course
                {
                    //Id = 7,
                    Name = "Programming for Complete Beginners",
                    Author = authors[2],
                    FullPrice = 45,
                    Description = "Description for Programming for Beginners",
                    Level = 1,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"]
                    }
                },
                new Course
                {
                    //Id = 8,
                    Name = "A 16 Hour C# Course with Visual Studio 2013",
                    Author = authors[3],
                    FullPrice = 150,
                    Description = "Description 16 Hour Course",
                    Level = 1,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"]
                    }
                },
                new Course
                {
                    //Id = 9,
                    Name = "Learn JavaScript Through Visual Studio 2013",
                    Author = authors[3],
                    FullPrice = 20,
                    Description = "Description Learn Javascript",
                    Level = 1,
                    Tags = new Collection<Tag>()
                    {
                        tags["javascript"]
                    }
                }
            };

            foreach (var course in courses)
                context.Courses.Add(course);
            #endregion

            context.SaveChanges();
        }

        static void Main(string[] args)
        {
            var dbContext = new CourseContext();
            Seed(dbContext);

            using (var unitOfWork = new UnitOfWork(dbContext))
            {
                // Example1
                var course = unitOfWork.Courses.Get(1);

                // Example2
                var courses = unitOfWork.Courses.GetCoursesWithAuthors(1, 4);

                // Example3
                var author = unitOfWork.Authors.GetAuthorWithCourses(1);
                unitOfWork.Courses.RemoveRange(author.Courses);
                unitOfWork.Authors.Remove(author);

                var trackedAuthors = dbContext.ChangeTracker.Entries<Author>();
                foreach(var a in trackedAuthors)
                {
                    System.Diagnostics.Debug.Write(a.CurrentValues["Id"]);
                    System.Diagnostics.Debug.WriteLine(") " + a.Entity.Name + ": " + a.State);
                    // a.Reload(); // reloads from the database - overwriting data!
                    // System.Diagnostics.Debug.WriteLine(a.Entity.Name + ": " + a.State);
                }
                var trackedObjects = dbContext.ChangeTracker.Entries();
                foreach (var o in trackedObjects)
                {
                    System.Diagnostics.Debug.WriteLine(o.Entity.GetType().ToString() + ": " + o.State);
                }

                unitOfWork.Complete();
            }
        }
    }
}
  