using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace DpControl.Migrations
{
    public partial class improveDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Alarm_AlarmMessage_AlarmMessageId", schema: "ControlSystem", table: "Alarms");
            migrationBuilder.DropForeignKey(name: "FK_Group_Customer_CustomerId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_Group_Scene_SceneSceneId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Customer_CustomerId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Customer_CustomerId", schema: "ControlSystem", table: "DeviceLocations");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Log_Location_LogOfLocationId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Operator_Customer_CustomerId", schema: "ControlSystem", table: "Operators");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Customer_CustomerId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.DropColumn(name: "LogOfLocationId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropColumn(name: "SceneSceneId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "ControlSystem",
                table: "Logs",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "SceneId",
                schema: "ControlSystem",
                table: "Groups",
                nullable: true);
            migrationBuilder.CreateIndex(
                name: "IX_Group_GroupName",
                schema: "ControlSystem",
                table: "Groups",
                column: "GroupName",
                unique: true);
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
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
                name: "FK_Group_Scene_SceneId",
                schema: "ControlSystem",
                table: "Groups",
                column: "SceneId",
                principalSchema: "ControlSystem",
                principalTable: "Scenes",
                principalColumn: "SceneId",
                onDelete: ReferentialAction.Restrict);
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
                name: "FK_Log_Location_LocationId",
                schema: "ControlSystem",
                table: "Logs",
                column: "LocationId",
                principalSchema: "ControlSystem",
                principalTable: "DeviceLocations",
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
            migrationBuilder.DropForeignKey(name: "FK_Group_Scene_SceneId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Customer_CustomerId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Customer_CustomerId", schema: "ControlSystem", table: "DeviceLocations");
            migrationBuilder.DropForeignKey(name: "FK_Log_Location_LocationId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Operator_Customer_CustomerId", schema: "ControlSystem", table: "Operators");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Customer_CustomerId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.DropIndex(name: "IX_Group_GroupName", schema: "ControlSystem", table: "Groups");
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "ControlSystem",
                table: "Logs",
                nullable: false);
            migrationBuilder.AddColumn<int>(
                name: "LogOfLocationId",
                schema: "ControlSystem",
                table: "Logs",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "SceneId",
                schema: "ControlSystem",
                table: "Groups",
                nullable: false);
            migrationBuilder.AddColumn<int>(
                name: "SceneSceneId",
                schema: "ControlSystem",
                table: "Groups",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "ControlSystem",
                table: "Alarms",
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
                name: "FK_Group_Customer_CustomerId",
                schema: "ControlSystem",
                table: "Groups",
                column: "CustomerId",
                principalSchema: "ControlSystem",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Group_Scene_SceneSceneId",
                schema: "ControlSystem",
                table: "Groups",
                column: "SceneSceneId",
                principalSchema: "ControlSystem",
                principalTable: "Scenes",
                principalColumn: "SceneId",
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
                name: "FK_Log_Location_LogOfLocationId",
                schema: "ControlSystem",
                table: "Logs",
                column: "LogOfLocationId",
                principalSchema: "ControlSystem",
                principalTable: "DeviceLocations",
                principalColumn: "LocationId",
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
