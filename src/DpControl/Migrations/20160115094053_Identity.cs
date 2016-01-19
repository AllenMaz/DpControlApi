using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace DpControl.Migrations
{
    public partial class Identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Alarm_AlarmMessage_AlarmMessageId", schema: "ControlSystem", table: "Alarms");
            migrationBuilder.DropForeignKey(name: "FK_Group_Customer_CustomerId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Customer_CustomerId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Customer_CustomerId", schema: "ControlSystem", table: "DeviceLocations");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Operator_Customer_CustomerId", schema: "ControlSystem", table: "Operators");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Customer_CustomerId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
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
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
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
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityUserRole<string>_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                schema: "ControlSystem",
                table: "Alarms",
                nullable: false);
            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");
            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName");
            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
            migrationBuilder.AddForeignKey(
                name: "FK_Alarm_AlarmMessage_AlarmMessageId",
                schema: "ControlSystem",
                table: "Alarms",
                column: "AlarmMessageId",
                principalSchema: "ControlSystem",
                principalTable: "AlarmMessages",
                principalColumn: "AlarmMessageId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Group_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Groups",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Holidays",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Location_Customer_CustomerId",
                schema: "ControlSystem",
                table: "DeviceLocations",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Log_LogDescription_LogDescriptionId",
                schema: "ControlSystem",
                table: "Logs",
                column: "LogDescriptionId",
                principalSchema: "ControlSystem",
                principalTable: "LogDescription",
                principalColumn: "LogDescriptionId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Operator_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Operators",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Scenes",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_SceneSegment_Scene_SceneId",
                schema: "ControlSystem",
                table: "SceneSegments",
                column: "SceneId",
                principalSchema: "ControlSystem",
                principalTable: "Scenes",
                principalColumn: "SceneId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Alarm_AlarmMessage_AlarmMessageId", schema: "ControlSystem", table: "Alarms");
            migrationBuilder.DropForeignKey(name: "FK_Group_Customer_CustomerId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Customer_CustomerId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Customer_CustomerId", schema: "ControlSystem", table: "DeviceLocations");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Operator_Customer_CustomerId", schema: "ControlSystem", table: "Operators");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Customer_CustomerId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.DropTable("AspNetRoleClaims");
            migrationBuilder.DropTable("AspNetUserClaims");
            migrationBuilder.DropTable("AspNetUserLogins");
            migrationBuilder.DropTable("AspNetUserRoles");
            migrationBuilder.DropTable("AspNetRoles");
            migrationBuilder.DropTable("AspNetUsers");
            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                schema: "ControlSystem",
                table: "Alarms",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_Alarm_AlarmMessage_AlarmMessageId",
                schema: "ControlSystem",
                table: "Alarms",
                column: "AlarmMessageId",
                principalSchema: "ControlSystem",
                principalTable: "AlarmMessages",
                principalColumn: "AlarmMessageId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Group_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Groups",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Holidays",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Location_Customer_CustomerId",
                schema: "ControlSystem",
                table: "DeviceLocations",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Log_LogDescription_LogDescriptionId",
                schema: "ControlSystem",
                table: "Logs",
                column: "LogDescriptionId",
                principalSchema: "ControlSystem",
                principalTable: "LogDescription",
                principalColumn: "LogDescriptionId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Operator_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Operators",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Scenes",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_SceneSegment_Scene_SceneId",
                schema: "ControlSystem",
                table: "SceneSegments",
                column: "SceneId",
                principalSchema: "ControlSystem",
                principalTable: "Scenes",
                principalColumn: "SceneId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
