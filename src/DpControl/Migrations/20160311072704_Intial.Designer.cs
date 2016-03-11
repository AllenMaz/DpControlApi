using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using DpControl.Domain.EFContext;

namespace DpControl.Migrations
{
    [DbContext(typeof(ShadingContext))]
    [Migration("20160311072704_Intial")]
    partial class Intial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("Relational:DefaultSchema", "ControlSystem")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DpControl.Domain.Entities.Alarm", b =>
                {
                    b.Property<int>("AlarmId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AlarmMessageId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int?>("LocationId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("AlarmId");

                    b.HasAnnotation("Relational:TableName", "Alarms");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.AlarmMessage", b =>
                {
                    b.Property<int>("AlarmMessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ErrorCode");

                    b.Property<string>("Message")
                        .HasAnnotation("MaxLength", 500);

                    b.HasKey("AlarmMessageId");

                    b.HasAnnotation("Relational:TableName", "AlarmMessages");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int?>("CustomerId");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int?>("ProjectId");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("CustomerNo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Modifier")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("CustomerId");

                    b.HasIndex("CustomerNo")
                        .IsUnique();

                    b.HasAnnotation("Relational:TableName", "Customers");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Diameter");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<float>("Torque");

                    b.Property<float>("Voltage");

                    b.HasKey("DeviceId");

                    b.HasAnnotation("Relational:TableName", "Devices");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Modifier");

                    b.Property<int?>("ProjectId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int?>("SceneId");

                    b.HasKey("GroupId");

                    b.HasIndex("GroupName")
                        .IsUnique();

                    b.HasAnnotation("Relational:TableName", "Groups");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupLocation", b =>
                {
                    b.Property<int>("GroupLocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GroupId");

                    b.Property<int>("LocationId");

                    b.HasKey("GroupLocationId");

                    b.HasAnnotation("Relational:TableName", "GroupLocations");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Holiday", b =>
                {
                    b.Property<int>("HolidayId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<int>("Day");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Modifier");

                    b.Property<int>("ProjectId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("HolidayId");

                    b.HasAnnotation("Relational:TableName", "Holidays");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Building")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("CommAddress")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<int>("CommMode");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int>("CurrentPosition");

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 2000);

                    b.Property<int?>("DeviceId");

                    b.Property<string>("DeviceSerialNo")
                        .HasAnnotation("MaxLength", 16);

                    b.Property<int>("DeviceType");

                    b.Property<int>("FavorPositionFirst");

                    b.Property<int>("FavorPositionThird");

                    b.Property<int>("FavorPositionrSecond");

                    b.Property<string>("Floor")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<int>("InstallationNumber");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Modifier");

                    b.Property<int>("Orientation");

                    b.Property<int?>("ProjectId");

                    b.Property<string>("RoomNo")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("LocationId");

                    b.HasAnnotation("Relational:TableName", "Locations");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int?>("LocationId");

                    b.Property<int?>("LogDescriptionId");

                    b.Property<DateTime?>("ModifiedDate")
                        .IsRequired();

                    b.Property<string>("Modifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("LogId");

                    b.HasAnnotation("Relational:TableName", "Logs");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.LogDescription", b =>
                {
                    b.Property<int>("LogDescriptionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<int>("DescriptionCode");

                    b.HasKey("LogDescriptionId");

                    b.HasAnnotation("Relational:TableName", "LogDescription");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Completed")
                        .HasAnnotation("Relational:DefaultValue", "False")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<int?>("CustomerId");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Modifier");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("ProjectNo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("ProjectId");

                    b.HasIndex("ProjectNo")
                        .IsUnique();

                    b.HasAnnotation("Relational:TableName", "Projects");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Scene", b =>
                {
                    b.Property<int>("SceneId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<bool>("Enable")
                        .HasAnnotation("Relational:DefaultValue", "False")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Modifier");

                    b.Property<int?>("ProjectId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("SceneName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("SceneId");

                    b.HasAnnotation("Relational:TableName", "Scenes");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.SceneSegment", b =>
                {
                    b.Property<int>("SceneSegmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Modifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("SceneId");

                    b.Property<int>("SequenceNo");

                    b.Property<string>("StartTime")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<int>("Volumn");

                    b.HasKey("SceneSegmentId");

                    b.HasAnnotation("Relational:TableName", "SceneSegments");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.UserGroup", b =>
                {
                    b.Property<int>("UserGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GroupId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("UserGroupId");

                    b.HasAnnotation("Relational:TableName", "UserGroups");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.UserLocation", b =>
                {
                    b.Property<int>("UserLocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LocationId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("UserLocationId");

                    b.HasAnnotation("Relational:TableName", "UserLocations");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Alarm", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.AlarmMessage")
                        .WithMany()
                        .HasForeignKey("AlarmMessageId");

                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.ApplicationUser", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("DpControl.Domain.Entities.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Group", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("DpControl.Domain.Entities.Scene")
                        .WithMany()
                        .HasForeignKey("SceneId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupLocation", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Holiday", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Location", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Device")
                        .WithMany()
                        .HasForeignKey("DeviceId");

                    b.HasOne("DpControl.Domain.Entities.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Log", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("DpControl.Domain.Entities.LogDescription")
                        .WithMany()
                        .HasForeignKey("LogDescriptionId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Project", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Scene", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.SceneSegment", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Scene")
                        .WithMany()
                        .HasForeignKey("SceneId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.UserGroup", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("DpControl.Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.UserLocation", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("DpControl.Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("DpControl.Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
