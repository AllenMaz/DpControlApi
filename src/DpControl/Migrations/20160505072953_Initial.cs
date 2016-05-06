using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace DpControl.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema("ControlSystem");
            migrationBuilder.CreateTable(
                name: "AlarmMessages",
                schema: "ControlSystem",
                columns: table => new
                {
                    AlarmMessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ErrorCode = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmMessage", x => x.AlarmMessageId);
                });
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "ControlSystem",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CustomerNo = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    ProjectNo = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserLevel = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "ControlSystem",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    CustomerName = table.Column<string>(nullable: false),
                    CustomerNo = table.Column<string>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });
            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "ControlSystem",
                columns: table => new
                {
                    DeviceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Diameter = table.Column<float>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true),
                    Torque = table.Column<float>(nullable: false),
                    Voltage = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.DeviceId);
                });
            migrationBuilder.CreateTable(
                name: "LogDescription",
                schema: "ControlSystem",
                columns: table => new
                {
                    LogDescriptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    DescriptionCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDescription", x => x.LogDescriptionId);
                });
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "ControlSystem",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "ControlSystem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim<string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "ControlSystem",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "ControlSystem",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserLogin<string>", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "ControlSystem",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "ControlSystem",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Completed = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: false),
                    ProjectNo = table.Column<string>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Project_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "ControlSystem",
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.SetNull);
                });
            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "ControlSystem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRoleClaim<string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "ControlSystem",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "ControlSystem",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserRole<string>", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "ControlSystem",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityUserRole<string>_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "ControlSystem",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Holidays",
                schema: "ControlSystem",
                columns: table => new
                {
                    HolidayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.HolidayId);
                    table.ForeignKey(
                        name: "FK_Holiday_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "ControlSystem",
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "ControlSystem",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Building = table.Column<string>(nullable: false),
                    CommAddress = table.Column<string>(nullable: true),
                    CommMode = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    CurrentPosition = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DeviceId = table.Column<int>(nullable: true),
                    DeviceSerialNo = table.Column<string>(nullable: true),
                    DeviceType = table.Column<int>(nullable: false),
                    FavorPositionFirst = table.Column<int>(nullable: false),
                    FavorPositionThird = table.Column<int>(nullable: false),
                    FavorPositionrSecond = table.Column<int>(nullable: false),
                    Floor = table.Column<string>(nullable: true),
                    InstallationNumber = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    Orientation = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: true),
                    RoomNo = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Location_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "ControlSystem",
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Location_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "ControlSystem",
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.SetNull);
                });
            migrationBuilder.CreateTable(
                name: "Scenes",
                schema: "ControlSystem",
                columns: table => new
                {
                    SceneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    Enable = table.Column<bool>(nullable: false, defaultValue: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true),
                    SceneName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scene", x => x.SceneId);
                    table.ForeignKey(
                        name: "FK_Scene_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "ControlSystem",
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.SetNull);
                });
            migrationBuilder.CreateTable(
                name: "Alarms",
                schema: "ControlSystem",
                columns: table => new
                {
                    AlarmId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlarmMessageId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarm", x => x.AlarmId);
                    table.ForeignKey(
                        name: "FK_Alarm_AlarmMessage_AlarmMessageId",
                        column: x => x.AlarmMessageId,
                        principalSchema: "ControlSystem",
                        principalTable: "AlarmMessages",
                        principalColumn: "AlarmMessageId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Alarm_Location_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "ControlSystem",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "ControlSystem",
                columns: table => new
                {
                    LogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    LogDescriptionId = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Log_Location_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "ControlSystem",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Log_LogDescription_LogDescriptionId",
                        column: x => x.LogDescriptionId,
                        principalSchema: "ControlSystem",
                        principalTable: "LogDescription",
                        principalColumn: "LogDescriptionId",
                        onDelete: ReferentialAction.SetNull);
                });
            migrationBuilder.CreateTable(
                name: "UserLocations",
                schema: "ControlSystem",
                columns: table => new
                {
                    UserLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LocationId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocation", x => x.UserLocationId);
                    table.ForeignKey(
                        name: "FK_UserLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "ControlSystem",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLocation_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "ControlSystem",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "ControlSystem",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    GroupName = table.Column<string>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true),
                    SceneId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Group_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "ControlSystem",
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Group_Scene_SceneId",
                        column: x => x.SceneId,
                        principalSchema: "ControlSystem",
                        principalTable: "Scenes",
                        principalColumn: "SceneId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "SceneSegments",
                schema: "ControlSystem",
                columns: table => new
                {
                    SceneSegmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true),
                    SceneId = table.Column<int>(nullable: false),
                    SequenceNo = table.Column<int>(nullable: false),
                    StartTime = table.Column<string>(nullable: false),
                    Volumn = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SceneSegment", x => x.SceneSegmentId);
                    table.ForeignKey(
                        name: "FK_SceneSegment_Scene_SceneId",
                        column: x => x.SceneId,
                        principalSchema: "ControlSystem",
                        principalTable: "Scenes",
                        principalColumn: "SceneId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "GroupLocations",
                schema: "ControlSystem",
                columns: table => new
                {
                    GroupLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupLocation", x => x.GroupLocationId);
                    table.ForeignKey(
                        name: "FK_GroupLocation_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "ControlSystem",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "ControlSystem",
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "UserGroups",
                schema: "ControlSystem",
                columns: table => new
                {
                    UserGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.UserGroupId);
                    table.ForeignKey(
                        name: "FK_UserGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "ControlSystem",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "ControlSystem",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "ControlSystem",
                table: "AspNetUsers",
                column: "NormalizedEmail");
            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "ControlSystem",
                table: "AspNetUsers",
                column: "NormalizedUserName");
            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerNo",
                schema: "ControlSystem",
                table: "Customers",
                column: "CustomerNo",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_Group_GroupName",
                schema: "ControlSystem",
                table: "Groups",
                column: "GroupName",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectNo",
                schema: "ControlSystem",
                table: "Projects",
                column: "ProjectNo",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "ControlSystem",
                table: "AspNetRoles",
                column: "NormalizedName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Alarms", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "GroupLocations", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Holidays", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Logs", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "SceneSegments", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "UserGroups", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "UserLocations", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "AspNetRoleClaims", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "AspNetUserClaims", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "AspNetUserLogins", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "AspNetUserRoles", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "AlarmMessages", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "LogDescription", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Groups", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Locations", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "AspNetRoles", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "AspNetUsers", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Scenes", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Devices", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Projects", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Customers", schema: "ControlSystem");
        }
    }
}
