using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaZuF.Models;

namespace WaZuF.Data
{
    public class AppDbContext : IdentityDbContext<Company>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAnswer> CandidateAnswers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // العلاقة بين JobRequest و Company
            modelBuilder.Entity<JobRequest>()
                .HasOne(j => j.Company)
                .WithMany(c => c.JobRequests)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // العلاقة بين Candidate و JobRequest
            modelBuilder.Entity<Employee>()
                .HasOne(c => c.JobRequest)
                .WithMany(j => j.Employees)
                .HasForeignKey(c => c.JobRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // تعطيل Cascade Delete لعلاقة CandidateAnswer مع Candidate
            modelBuilder.Entity<EmployeeAnswer>()
                .HasOne(ca => ca.Employee)
                .WithMany(c => c.CandidateAnswers)
                .HasForeignKey(ca => ca.EmployeesId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ يمنع الحذف التلقائي

            // العلاقة بين CandidateAnswer و Question
            modelBuilder.Entity<EmployeeAnswer>()
                .HasOne(ca => ca.Question)
                .WithMany()
                .HasForeignKey(ca => ca.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // العلاقة بين Question و JobRequest
            modelBuilder.Entity<Question>()
                .HasOne(q => q.JobRequest)
                .WithMany(j => j.Questions)
                .HasForeignKey(q => q.JobRequestId)
                .OnDelete(DeleteBehavior.Cascade);




        }

        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<JobRequest> JobRequests { get; set; }
        }
    }
}
