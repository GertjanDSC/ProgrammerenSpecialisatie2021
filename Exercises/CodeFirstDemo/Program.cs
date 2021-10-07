using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace CodeFirstDemo
{

    // Abstration to load/save data
    public class BlogDbContext : DbContext
    {
        //static LoggerFactory object
        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(
            builder => {
                builder
                    //.AddFilter("SampleApp.Program", LogLevel.Debug)
                    .AddDebug();
            }
        );
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var cs = System.Configuration.ConfigurationManager.ConnectionStrings["BlogDbContext"].ConnectionString;
                // Tie up DbContext with LoggerFactory object
                optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging().UseSqlServer(cs);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("PostID");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.DatePublished).HasColumnType("smalldatetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);
            });
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new BlogDbContext();
            Post post = new() { Body = "body", DatePublished = DateTime.Now, Title = "title" /*, PostId = 1*/ };
            context.Posts.Add(post);
            context.SaveChanges();
        }
    }

}