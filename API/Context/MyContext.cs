using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        #region DbSet
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; } 
        public DbSet<Roles> Roles { get; set; }
        public DbSet<AccountRoles> AccountRoles { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured)
            {
                ob
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Employee>()
                .HasOne(b => b.Account)
                .WithOne(a => a.Employee)
                .HasForeignKey<Account>(b => b.NIK);
            mb.Entity<Account>()
                .HasOne(a => a.Profiling)
                .WithOne(b => b.Account)
                .HasForeignKey<Profiling>(a => a.NIK);

            mb.Entity<AccountRoles>()
                .HasKey("NIK","RolesId");
            mb.Entity<AccountRoles>()
                .HasOne(a => a.Account)
                .WithMany(b => b.AccountRoles)
                .HasForeignKey(a => a.NIK);
            mb.Entity<AccountRoles>()
                .HasOne(a => a.Roles)
                .WithMany(b => b.AccountRoles)
                .HasForeignKey(a => a.RolesId);

            mb.Entity<Profiling>()
                .HasOne(a => a.Education)
                .WithMany(b => b.Profilings)
                .HasForeignKey(a => a.Education_Id);
            mb.Entity<Education>()
                .HasOne(a => a.University)
                .WithMany(b => b.Educations)
                .HasForeignKey(a => a.University_Id);
        }
    }
}
