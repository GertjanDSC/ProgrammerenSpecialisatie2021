using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CodeFirstFromDbDemo.Models
{
    public partial class EfPsContext : DbContext
    {
        public EfPsContext()
        {
        }

        public EfPsContext(DbContextOptions<EfPsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseSection> CourseSections { get; set; }
        public virtual DbSet<CourseTag> CourseTags { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TblUser> TblUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           if (!optionsBuilder.IsConfigured)
           {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost,1436;database=EfPs;Trusted_Connection=False;user ID=sa;Password=1Secure*Password1");
           }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.LevelString)
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

            modelBuilder.Entity<CourseSection>(entity =>
            {
                entity.HasKey(e => e.SectionId)
                    .HasName("PK_Sections");

                entity.Property(e => e.SectionId).HasColumnName("SectionID");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseSections)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_CourseSections_Courses");
            });

            modelBuilder.Entity<CourseTag>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.TagId });

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseTags)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_CourseTags_Courses");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.CourseTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_CourseTags_Tags");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.PostId)
                    .ValueGeneratedNever()
                    .HasColumnName("PostID");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.DatePublished).HasColumnType("smalldatetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tblUser");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
