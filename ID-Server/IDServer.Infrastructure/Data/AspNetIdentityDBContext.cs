using IDServer.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace IDServer.Infrastructure.Data
{
    public class AspNetIdentityDBContext : IdentityDbContext
    {
        public AspNetIdentityDBContext(DbContextOptions<AspNetIdentityDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>()
            .Property(b => b.FirstName)
            .HasColumnType("nvarchar(250)");

            modelBuilder.Entity<Users>()
            .Property(b => b.LastName)
            .HasColumnType("nvarchar(250)");

            modelBuilder.Entity<Users>()
            .Property(b => b.Status)
            .HasColumnType("nvarchar(250)");

            //modelBuilder.Entity<Users>()
            //.Property(b => b.UserType)
            //.HasColumnType("nvarchar(250)");

            modelBuilder.Entity<Users>()
            .Property(b => b.timestamp)
            .HasColumnType("timestamp");

            modelBuilder.Entity<Users>()
            .HasMany(u => u.userSessions)
            .WithOne(s => s.user)
            .HasForeignKey(x => x.UserId);
            //.IsRequired();

            modelBuilder.Entity<Users>()
            .HasOne(u => u.roleType)
            .WithMany(ut => ut.Users)
            .HasForeignKey(u => u.UserTypeId);



            modelBuilder.Entity<Users>().Property(e => e.timestamp)
           .IsConcurrencyToken()           // If you want to handle concurrency
           .ValueGeneratedOnAddOrUpdate(); // This is important

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleType>()
            .HasMany(u => u.AssignedPages)
            .WithOne(x => x.RoleType)
            .HasForeignKey(x => x.RoleTypeId);

            modelBuilder.Entity<PageRoleModule>()
            .HasMany(u => u.assignPages);

            modelBuilder.Entity<AssignPageRole>()
                .HasOne(x => x.Page)
                .WithMany(x => x.assignPages)
                .HasForeignKey(x => x.PageRoleId);



        }
        public DbSet<Users> users { get; set; }
        public DbSet<UserSessions> UserDeviceSessions { get; set; }
        public DbSet<RoleType> UserTypes { get; set; }
        public DbSet<PageRoleModule> PageRoleModules { get; set; }
        public DbSet<AssignPageRole> AssignPageRoles { get; set; }

        

        

    }
}
