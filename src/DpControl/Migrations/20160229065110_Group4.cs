using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace DpControl.Migrations
{
    public partial class Group4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Alarm_AlarmMessage_AlarmMessageId", schema: "ControlSystem", table: "Alarms");
            migrationBuilder.DropForeignKey(name: "FK_Group_Project_ProjectId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Project_ProjectId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Project_ProjectId", schema: "ControlSystem", table: "DeviceLocations");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Operator_Project_ProjectId", schema: "ControlSystem", table: "Operators");
            migrationBuilder.DropForeignKey(name: "FK_Project_Customer_CustomerId", schema: "ControlSystem", table: "Projects");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Project_ProjectId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_IdentityRole_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_ApplicationUser_UserId", table: "AspNetUserRoles");
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
                name: "FK_Group_Project_ProjectId",
                schema: "ControlSystem",
                table: "Groups",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_Project_ProjectId",
                schema: "ControlSystem",
                table: "Holidays",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Location_Project_ProjectId",
                schema: "ControlSystem",
                table: "DeviceLocations",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
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
                name: "FK_Operator_Project_ProjectId",
                schema: "ControlSystem",
                table: "Operators",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Project_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Projects",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Project_ProjectId",
                schema: "ControlSystem",
                table: "Scenes",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
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
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_ApplicationUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Alarm_AlarmMessage_AlarmMessageId", schema: "ControlSystem", table: "Alarms");
            migrationBuilder.DropForeignKey(name: "FK_Group_Project_ProjectId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Project_ProjectId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Project_ProjectId", schema: "ControlSystem", table: "DeviceLocations");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Operator_Project_ProjectId", schema: "ControlSystem", table: "Operators");
            migrationBuilder.DropForeignKey(name: "FK_Project_Customer_CustomerId", schema: "ControlSystem", table: "Projects");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Project_ProjectId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_IdentityRole_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_ApplicationUser_UserId", table: "AspNetUserRoles");
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
                name: "FK_Group_Project_ProjectId",
                schema: "ControlSystem",
                table: "Groups",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_Project_ProjectId",
                schema: "ControlSystem",
                table: "Holidays",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Location_Project_ProjectId",
                schema: "ControlSystem",
                table: "DeviceLocations",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
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
                name: "FK_Operator_Project_ProjectId",
                schema: "ControlSystem",
                table: "Operators",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Project_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Projects",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Project_ProjectId",
                schema: "ControlSystem",
                table: "Scenes",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
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
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_ApplicationUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
