using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using DpControl.Domain.EFContext;

namespace DpControl.Migrations
{
    [DbContext(typeof(ShadingContext))]
    [Migration("20160220081311_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DpControl.Domain.Entities.Alarm", b =>
                {
                    b.Property<int>("AlarmId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlarmMessageId");

                    b.Property<int?>("LocationId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("AlarmId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Alarms");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.AlarmMessage", b =>
                {
                    b.Property<int>("AlarmMessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ErrorNo");

                    b.Property<string>("Message")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("AlarmMessageId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "AlarmMessages");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 60);

                    b.Property<string>("CustomerNo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 60);

                    b.Property<string>("ProjectNo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("CustomerId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Customers");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int?>("SceneId");

                    b.HasKey("GroupId");

                    b.HasIndex("GroupName")
                        .IsUnique();

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Groups");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupLocation", b =>
                {
                    b.Property<int>("GroupLocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GroupGroupId");

                    b.Property<int>("GroupId");

                    b.Property<int>("LocationId");

                    b.Property<int?>("LocationLocationId");

                    b.HasKey("GroupLocationId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "GroupLocations");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupOperator", b =>
                {
                    b.Property<int>("GroupOperatorId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GroupGroupId");

                    b.Property<int>("GroupId");

                    b.Property<int>("OperatorId");

                    b.Property<int?>("OperatorOperatorId");

                    b.HasKey("GroupOperatorId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "GroupOperators");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Holiday", b =>
                {
                    b.Property<int>("HolidayId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<int>("Day");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("HolidayId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Holidays");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Building")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 80);

                    b.Property<string>("CommAddress")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<int>("CommMode");

                    b.Property<int>("CurrentPosition");

                    b.Property<int>("CustomerId");

                    b.Property<string>("DeviceSerialNo")
                        .HasAnnotation("MaxLength", 16);

                    b.Property<int>("DeviceType");

                    b.Property<int>("FavorPositionFirst");

                    b.Property<int>("FavorPositionThird");

                    b.Property<int>("FavorPositionrSecond");

                    b.Property<string>("Floor")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<int>("InstallationNumber");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("Orientation");

                    b.Property<string>("RoomNo")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("LocationId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "DeviceLocations");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int?>("LocationId");

                    b.Property<int>("LogDescriptionId");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int?>("OperatorId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("LogId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Logs");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.LogDescription", b =>
                {
                    b.Property<int>("LogDescriptionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("DescriptionNo");

                    b.HasKey("LogDescriptionId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "LogDescription");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Operator", b =>
                {
                    b.Property<int>("OperatorId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("NickName")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("Password")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("OperatorId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Operators");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.OperatorLocation", b =>
                {
                    b.Property<int>("OperatorLocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LocationId");

                    b.Property<int?>("LocationLocationId");

                    b.Property<int>("OperatorId");

                    b.Property<int?>("OperatorOperatorId");

                    b.HasKey("OperatorLocationId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "OperatorLocations");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Scene", b =>
                {
                    b.Property<int>("SceneId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<bool>("Enable");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("SceneId");

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "Scenes");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.SceneSegment", b =>
                {
                    b.Property<int>("SceneSegmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ModifiedDate");

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

                    b.HasAnnotation("Relational:Schema", "ControlSystem");

                    b.HasAnnotation("Relational:TableName", "SceneSegments");
                });

            modelBuilder.Entity("DpControl.Domain.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

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

            modelBuilder.Entity("DpControl.Domain.Entities.Group", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("DpControl.Domain.Entities.Scene")
                        .WithMany()
                        .HasForeignKey("SceneId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupLocation", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Group")
                        .WithMany()
                        .HasForeignKey("GroupGroupId");

                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationLocationId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.GroupOperator", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Group")
                        .WithMany()
                        .HasForeignKey("GroupGroupId");

                    b.HasOne("DpControl.Domain.Entities.Operator")
                        .WithMany()
                        .HasForeignKey("OperatorOperatorId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Holiday", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Location", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Log", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("DpControl.Domain.Entities.LogDescription")
                        .WithMany()
                        .HasForeignKey("LogDescriptionId");

                    b.HasOne("DpControl.Domain.Entities.Operator")
                        .WithMany()
                        .HasForeignKey("OperatorId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Operator", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.OperatorLocation", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Location")
                        .WithMany()
                        .HasForeignKey("LocationLocationId");

                    b.HasOne("DpControl.Domain.Entities.Operator")
                        .WithMany()
                        .HasForeignKey("OperatorOperatorId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.Scene", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("DpControl.Domain.Entities.SceneSegment", b =>
                {
                    b.HasOne("DpControl.Domain.Entities.Scene")
                        .WithMany()
                        .HasForeignKey("SceneId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DpControl.Domain.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DpControl.Domain.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("DpControl.Domain.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}