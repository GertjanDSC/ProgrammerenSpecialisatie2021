using CodeFirstFromDbDemo.Models;
using Microsoft.EntityFrameworkCore; // nodig voor Include()
using System.Linq;

namespace CodeFirstFromDbDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new EfPsContext();

            // Lazy loading
            /*
            var course = context.Courses.Single(c => c.CourseId == 2);

            foreach(var courseTag in course.CourseTags)
            {
                System.Diagnostics.Debug.WriteLine(courseTag.Tag.Name);
            }
            */

            /*
            var courses = context.Courses.ToList();
            foreach (var course in courses)
            {
                System.Diagnostics.Debug.WriteLine("{0} door {1}", course.Name, course.Author.Name);
            }
            */

            // Eager loading
            //var courses = context.Courses.Include(c => c.CourseTags).ToList();
            // Explicit loading
            // MSDN
            {
                var author = context.Authors.Single(a => a.AuthorId == 1);
                // Entry() werkt uiteraard niet voor meer objecten:
                context.Entry(author).Collection(a => a.Courses).Load();
            }
            // Alternatief: LINQ
            {
                var author = context.Authors.Single(a => a.AuthorId == 1);
                // Alle cursussen voor deze auteur:
                context.Courses.Where(c => c.AuthorId == author.AuthorId).Load();
            }
            // MSDN
            {
                var author = context.Authors.Single(a => a.AuthorId == 1);
                context.Entry(author).Collection(a => a.Courses).Query().Where(c => c.Price == 0).Load();
            }
            // Alternatief: LINQ
            {
                var author = context.Authors.Single(a => a.AuthorId == 1); 
                context.Courses.Where(c => c.AuthorId == author.AuthorId && c.Price == 0).Load();
            }
            // Add
            {
                var course = new Course
                {
                    Name = "New Course",
                    Description = "New Description",
                    Price = 19,
                    Level = 1,
                    LevelString = "L",
                    Author = new Author() {  /*AuthorId = 1,*/ Name = "Luc Vervoort" } // Change tracker always sees this as a new object! Vroeger kon AuthorId = 1 erbij, nu niet meer
                };
                context.Courses.Add(course);
                context.SaveChanges();
                // Drie oplossingen voor nieuwe auteur:
                // 1. Bestaand object gebruiken: beter voor WPF
                var authors = context.Authors.ToList();
                var author = context.Authors.Single(a => a.AuthorId == 1);
                var course2 = new Course
                {
                    Name = "New Course 2",
                    Description = "New Description",
                    Price = 19,
                    Level = 1,
                    LevelString = "L",
                    Author = author // Change tracker always sees this as a new object!
                };
                context.Courses.Add(course2);
                context.SaveChanges();
                // 2. Foreign key property gebruiken: beter voor web?
                var course3 = new Course
                {
                    Name = "New Course 3",
                    Description = "New Description",
                    Price = 19,
                    Level = 1,
                    LevelString = "L",
                    AuthorId = 1 // Change tracker always sees this as a new object!
                };
                context.Courses.Add(course3);
                context.SaveChanges();
                // 3. Attach object: normaal niet nodig; je gebruikt EF intern en dit is een nadeel
                try
                {
                    var attachedAuthor = new Author() { AuthorId = 1, Name = "Luc Vervoort" }; // genereert tegenwoordig een fout als het object met Id al tracked wordt, bijvoorbeeld 1
                    context.Authors.Attach(attachedAuthor);
                }
                catch(System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    // The instance of entity type 'Author' cannot be tracked because another instance with the same key value for {'AuthorId'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached.
                    // Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.
                }
            }
        }
    }
}
