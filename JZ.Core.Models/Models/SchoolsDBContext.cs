using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JZ.Core.Models
{
    public partial class SchoolsDBContext : DbContext
    {
        public SchoolsDBContext()
        {
        }

        public SchoolsDBContext(DbContextOptions<SchoolsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TTeacher> TTeacher { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=.;Database=SchoolsDB;uid=sa;pwd=p@ssw0rd");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TTeacher>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("T_Teacher");

                entity.Property(e => e.FId).HasColumnName("F_ID");

                entity.Property(e => e.FTeacherName)
                    .HasColumnName("F_TeacherName")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
