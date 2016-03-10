using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext.Configurations;
using Microsoft.AspNet.Identity.EntityFramework;
using DpControl.Domain.Models;

namespace DpControl.Domain.EFContext
{
    public class ShadingContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<AlarmMessage> AlarmMessages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupLocation> GroupLocations { get; set; }
        public DbSet<UserGroup> GroupOperators { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogDescription> LogDescription { get; set; }
        public DbSet<UserLocation> UserDeviceLocations { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<SceneSegment> SceneSegments { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("ControlSystem");

            new AlarmMessageConfiguration(modelBuilder.Entity<AlarmMessage>());
            new AlarmConfiguration(modelBuilder.Entity<Alarm>());
            new CustomerConfiguration(modelBuilder.Entity<Customer>());
            new ProjectConfiguration(modelBuilder.Entity<Project>());
            new GroupConfiguration(modelBuilder.Entity<Group>());
            new UserGroupConfiguration(modelBuilder.Entity<UserGroup>());
            new GroupLocationConfiguration(modelBuilder.Entity<GroupLocation>());
            new HolidayConfiguration(modelBuilder.Entity<Holiday>());
            new LocationConfiguration(modelBuilder.Entity<Location>());
            new DeviceConfiguration(modelBuilder.Entity<Device>());
            new LogConfiguration(modelBuilder.Entity<Log>());
            new LogDescriptionConfiguration(modelBuilder.Entity<LogDescription>());
            new UserLocationConfiguration(modelBuilder.Entity<UserLocation>());
            new SceneConfiguration(modelBuilder.Entity<Scene>());
            new SceneSegmentConfiguration(modelBuilder.Entity<SceneSegment>());
            new AspNetUserConfiguration(modelBuilder.Entity<ApplicationUser>());

            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectsV12;Initial Catalog=FlugDbE2E;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //optionsBuilder.UseInMemoryDatabase();
            var connectionString = Startup.Configuration["Data:DefaultConnection:ConnectionString"];
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
