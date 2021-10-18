using Microsoft.EntityFrameworkCore;
using Queries.Core.Domain;
using Microsoft.Extensions.Configuration;
using System;

#nullable disable


namespace Queries.Persistence
{
    public partial class CourseContext : DbContext
    {
        private string _connectionString;

        //public CourseContext()
        //{
        //}

        // This constructor obtains the connection string from your appsettings.json file.
        // Tell LINQPad to use it if you don't want to specify a connection string in LINQPad's dialog.
        // Install Microsoft.Extensions.Configuration and Microsoft.Extensions.Configuration.Json Nuget packages
        public CourseContext()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // The constructor that ASP.NET Core expects. LINQPad can use it too.
        public CourseContext(DbContextOptions<CourseContext> options)
            : base(options)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // This constructor is simpler and more robust. Use it if LINQPad errors on the constructor above.
        // Note that _connectionString is picked up in the OnConfiguring method below.
        public CourseContext(string connectionString) => _connectionString = connectionString;

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies(true); // Enables lazy loading; off by default!!

                // Assign _connectionString to the optionsBuilder:
                if (_connectionString != null)
                    optionsBuilder.UseSqlServer(_connectionString);    // Change to UseSqlite if you're using SQLite

                // Recommended: uncomment the following line to enable lazy-loading navigation hyperlinks in LINQPad:
                if (InsideLINQPad) optionsBuilder.UseLazyLoadingProxies();
                // (You'll need to add a reference to the Microsoft.EntityFrameworkCore.Proxies NuGet package, and mark your navigation properties as virtual)

                // Recommended: uncomment the following line to enable the SQL trace window:
                if (InsideLINQPad) optionsBuilder.EnableSensitiveDataLogging(true);
            }
        }

        // This property indicates whether or not you're running inside LINQPad:
        internal bool InsideLINQPad => AppDomain.CurrentDomain.FriendlyName.StartsWith("LINQPad");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("AuthorID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("CourseID");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Courses_Authors");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("TagID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    // This is just For Visual Studio design-time support and Migrations (LINQPad doesn't use it).
    // Include this class if you want to specify a different connection string when using Visual Studio design-time tools.
    public class SampleDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<CourseContext>
    {
        public CourseContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("DesignTimeConnection");
            return new CourseContext(connectionString); // design-time connection string
        }
    }
}
