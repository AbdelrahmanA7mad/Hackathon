using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaZuF.Models;

namespace WaZuF.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<EmployeeAnswer> EmployeeAnswers { get; set; } // ✅ Renamed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // JobRequest and AppUser Relationship
            modelBuilder.Entity<JobRequest>()
                .HasOne(j => j.AppUser)
                .WithMany(c => c.JobRequests)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employee and JobRequest Relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.JobRequest)
                .WithMany(j => j.Employees)
                .HasForeignKey(e => e.JobRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // EmployeeAnswer and Employee Relationship (Fixed Name)
            modelBuilder.Entity<EmployeeAnswer>()
                .HasOne(ea => ea.Employee)
                .WithMany(e => e.EmployeeAnswers)
                .HasForeignKey(ea => ea.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevents cascade delete

            // EmployeeAnswer and Question Relationship
            modelBuilder.Entity<EmployeeAnswer>()
                .HasOne(ea => ea.Question)
                .WithMany()
                .HasForeignKey(ea => ea.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question and JobRequest Relationship
            modelBuilder.Entity<Question>()
                .HasOne(q => q.JobRequest)
                .WithMany(j => j.Questions)
                .HasForeignKey(q => q.JobRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Exam and AppUser Relationship
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.AppUser)
                .WithMany() // No navigation property in AppUser
                .HasForeignKey(e => e.AppUserId)
                .OnDelete(DeleteBehavior.SetNull); // ✅ Prevents FK constraint errors

        }
    }
}
