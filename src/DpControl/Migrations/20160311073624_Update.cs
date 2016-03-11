using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace DpControl.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Alarm_AlarmMessage_AlarmMessageId", schema: "ControlSystem", table: "Alarms");
            migrationBuilder.DropForeignKey(name: "FK_ApplicationUser_Customer_CustomerId", schema: "ControlSystem", table: "AspNetUsers");
            migrationBuilder.DropForeignKey(name: "FK_ApplicationUser_Project_ProjectId", schema: "ControlSystem", table: "AspNetUsers");
            migrationBuilder.DropForeignKey(name: "FK_Group_Project_ProjectId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_GroupLocation_Group_GroupId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropForeignKey(name: "FK_GroupLocation_Location_LocationId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Project_ProjectId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Device_DeviceId", schema: "ControlSystem", table: "Locations");
            migrationBuilder.DropForeignKey(name: "FK_Location_Project_ProjectId", schema: "ControlSystem", table: "Locations");
            migrationBuilder.DropForeignKey(name: "FK_Log_Location_LocationId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Project_Customer_CustomerId", schema: "ControlSystem", table: "Projects");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Project_ProjectId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.DropForeignKey(name: "FK_UserGroup_Group_GroupId", schema: "ControlSystem", table: "UserGroups");
            migrationBuilder.DropForeignKey(name: "FK_UserGroup_ApplicationUser_UserId", schema: "ControlSystem", table: "UserGroups");
            migrationBuilder.DropForeignKey(name: "FK_UserLocation_Location_LocationId", schema: "ControlSystem", table: "UserLocations");
            migrationBuilder.DropForeignKey(name: "FK_UserLocation_ApplicationUser_UserId", schema: "ControlSystem", table: "UserLocations");
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId", schema: "ControlSystem", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId", schema: "ControlSystem", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId", schema: "ControlSystem", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_IdentityRole_RoleId", schema: "ControlSystem", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_ApplicationUser_UserId", schema: "ControlSystem", table: "AspNetUserRoles");
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                schema: "ControlSystem",
                table: "Logs",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_Alarm_AlarmMessage_AlarmMessageId",
                schema: "ControlSystem",
                table: "Alarms",
                column: "AlarmMessageId",
                principalSchema: "ControlSystem",
                principalTable: "AlarmMessages",
                principalColumn: "AlarmMessageId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Customer_CustomerId",
                schema: "ControlSystem",
                table: "AspNetUsers",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Project_ProjectId",
                schema: "ControlSystem",
                table: "AspNetUsers",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_Group_Project_ProjectId",
                schema: "ControlSystem",
                table: "Groups",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_GroupLocation_Group_GroupId",
                schema: "ControlSystem",
                table: "GroupLocations",
                column: "GroupId",
                principalSchema: "ControlSystem",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_GroupLocation_Location_LocationId",
                schema: "ControlSystem",
                table: "GroupLocations",
                column: "LocationId",
                principalSchema: "ControlSystem",
                principalTable: "Locations",
                principalColumn: "LocationId",
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
                name: "FK_Location_Device_DeviceId",
                schema: "ControlSystem",
                table: "Locations",
                column: "DeviceId",
                principalSchema: "ControlSystem",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_Location_Project_ProjectId",
                schema: "ControlSystem",
                table: "Locations",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_Log_Location_LocationId",
                schema: "ControlSystem",
                table: "Logs",
                column: "LocationId",
                principalSchema: "ControlSystem",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_Log_LogDescription_LogDescriptionId",
                schema: "ControlSystem",
                table: "Logs",
                column: "LogDescriptionId",
                principalSchema: "ControlSystem",
                principalTable: "LogDescription",
                principalColumn: "LogDescriptionId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_Project_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Projects",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Project_ProjectId",
                schema: "ControlSystem",
                table: "Scenes",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.SetNull);
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
                name: "FK_UserGroup_Group_GroupId",
                schema: "ControlSystem",
                table: "UserGroups",
                column: "GroupId",
                principalSchema: "ControlSystem",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_UserGroup_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "UserGroups",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_UserLocation_Location_LocationId",
                schema: "ControlSystem",
                table: "UserLocations",
                column: "LocationId",
                principalSchema: "ControlSystem",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_UserLocation_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "UserLocations",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                schema: "ControlSystem",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                schema: "ControlSystem",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Alarm_AlarmMessage_AlarmMessageId", schema: "ControlSystem", table: "Alarms");
            migrationBuilder.DropForeignKey(name: "FK_ApplicationUser_Customer_CustomerId", schema: "ControlSystem", table: "AspNetUsers");
            migrationBuilder.DropForeignKey(name: "FK_ApplicationUser_Project_ProjectId", schema: "ControlSystem", table: "AspNetUsers");
            migrationBuilder.DropForeignKey(name: "FK_Group_Project_ProjectId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_GroupLocation_Group_GroupId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropForeignKey(name: "FK_GroupLocation_Location_LocationId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Project_ProjectId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Device_DeviceId", schema: "ControlSystem", table: "Locations");
            migrationBuilder.DropForeignKey(name: "FK_Location_Project_ProjectId", schema: "ControlSystem", table: "Locations");
            migrationBuilder.DropForeignKey(name: "FK_Log_Location_LocationId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Project_Customer_CustomerId", schema: "ControlSystem", table: "Projects");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Project_ProjectId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.DropForeignKey(name: "FK_UserGroup_Group_GroupId", schema: "ControlSystem", table: "UserGroups");
            migrationBuilder.DropForeignKey(name: "FK_UserGroup_ApplicationUser_UserId", schema: "ControlSystem", table: "UserGroups");
            migrationBuilder.DropForeignKey(name: "FK_UserLocation_Location_LocationId", schema: "ControlSystem", table: "UserLocations");
            migrationBuilder.DropForeignKey(name: "FK_UserLocation_ApplicationUser_UserId", schema: "ControlSystem", table: "UserLocations");
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId", schema: "ControlSystem", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId", schema: "ControlSystem", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId", schema: "ControlSystem", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_IdentityRole_RoleId", schema: "ControlSystem", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_ApplicationUser_UserId", schema: "ControlSystem", table: "AspNetUserRoles");
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                schema: "ControlSystem",
                table: "Logs",
                nullable: false);
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
                name: "FK_ApplicationUser_Customer_CustomerId",
                schema: "ControlSystem",
                table: "AspNetUsers",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Project_ProjectId",
                schema: "ControlSystem",
                table: "AspNetUsers",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
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
                name: "FK_GroupLocation_Group_GroupId",
                schema: "ControlSystem",
                table: "GroupLocations",
                column: "GroupId",
                principalSchema: "ControlSystem",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_GroupLocation_Location_LocationId",
                schema: "ControlSystem",
                table: "GroupLocations",
                column: "LocationId",
                principalSchema: "ControlSystem",
                principalTable: "Locations",
                principalColumn: "LocationId",
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
                name: "FK_Location_Device_DeviceId",
                schema: "ControlSystem",
                table: "Locations",
                column: "DeviceId",
                principalSchema: "ControlSystem",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Location_Project_ProjectId",
                schema: "ControlSystem",
                table: "Locations",
                column: "ProjectId",
                principalSchema: "ControlSystem",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Log_Location_LocationId",
                schema: "ControlSystem",
                table: "Logs",
                column: "LocationId",
                principalSchema: "ControlSystem",
                principalTable: "Locations",
                principalColumn: "LocationId",
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
                name: "FK_UserGroup_Group_GroupId",
                schema: "ControlSystem",
                table: "UserGroups",
                column: "GroupId",
                principalSchema: "ControlSystem",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_UserGroup_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "UserGroups",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_UserLocation_Location_LocationId",
                schema: "ControlSystem",
                table: "UserLocations",
                column: "LocationId",
                principalSchema: "ControlSystem",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_UserLocation_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "UserLocations",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                schema: "ControlSystem",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                schema: "ControlSystem",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_ApplicationUser_UserId",
                schema: "ControlSystem",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "ControlSystem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
