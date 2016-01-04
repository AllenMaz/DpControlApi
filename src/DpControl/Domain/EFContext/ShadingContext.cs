using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using DpControl.Domain.Entities;
using DpControl.Domain.EFContext.Configurations;

namespace DpControl.Domain.EFContext
{
    public class ShadingContext :DbContext
    {
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<AlarmMessage> AlarmMessages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<GroupLocation> GroupLocations { get; set; }
        public DbSet<GroupOperator> GroupOperators { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogDescription> LogDescription { get; set; }
        public DbSet<OperatorLocation> OperatorLocation { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<SceneSegment> SceneSegments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new AlarmMessageConfiguration(modelBuilder.Entity<AlarmMessage>());
            new AlarmConfiguration(modelBuilder.Entity<Alarm>());
            new CustomerConfiguration(modelBuilder.Entity<Customer>());
            new GroupConfiguration(modelBuilder.Entity<Group>());
            new GroupOperatorConfiguration(modelBuilder.Entity<GroupOperator>());
            new GroupLocationConfiguration(modelBuilder.Entity<GroupLocation>());
            new HolidayConfiguration(modelBuilder.Entity<Holiday>());
            new LocationConfiguration(modelBuilder.Entity<Location>());
            new LogConfiguration(modelBuilder.Entity<Log>());
            new LogDescriptionConfiguration(modelBuilder.Entity<LogDescription>());
            new OperatorLocationConfiguration(modelBuilder.Entity<OperatorLocation>());
            new SceneConfiguration(modelBuilder.Entity<Scene>());
            new SceneSegmentConfiguration(modelBuilder.Entity<SceneSegment>());

        }
    }
}
