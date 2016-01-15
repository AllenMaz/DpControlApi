using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace DpControl.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Alarm_AlarmMessage_AlarmMessageId", schema: "ControlSystem", table: "Alarms");
            migrationBuilder.DropForeignKey(name: "FK_Group_Customer_CustomerId", schema: "ControlSystem", table: "Groups");
            migrationBuilder.DropForeignKey(name: "FK_GroupLocation_Group_GroupId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropForeignKey(name: "FK_GroupLocation_Location_LocationId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropForeignKey(name: "FK_GroupOperator_Group_GroupId", schema: "ControlSystem", table: "GroupOperators");
            migrationBuilder.DropForeignKey(name: "FK_GroupOperator_Operator_OperatorId", schema: "ControlSystem", table: "GroupOperators");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Customer_CustomerId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Customer_CustomerId", schema: "ControlSystem", table: "DeviceLocations");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Operator_Customer_CustomerId", schema: "ControlSystem", table: "Operators");
            migrationBuilder.DropForeignKey(name: "FK_OperatorLocation_Location_LocationId", schema: "ControlSystem", table: "OperatorLocations");
            migrationBuilder.DropForeignKey(name: "FK_OperatorLocation_Operator_OperatorId", schema: "ControlSystem", table: "OperatorLocations");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Customer_CustomerId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.AlterColumn<int>(
                name: "OperatorId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                nullable: false);
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                nullable: false);
            migrationBuilder.AddColumn<int>(
                name: "LocationLocationId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "OperatorOperatorId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "OperatorId",
                schema: "ControlSystem",
                table: "GroupOperators",
                nullable: false);
            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                schema: "ControlSystem",
                table: "GroupOperators",
                nullable: false);
            migrationBuilder.AddColumn<int>(
                name: "GroupGroupId",
                schema: "ControlSystem",
                table: "GroupOperators",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "OperatorOperatorId",
                schema: "ControlSystem",
                table: "GroupOperators",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "ControlSystem",
                table: "GroupLocations",
                nullable: false);
            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                schema: "ControlSystem",
                table: "GroupLocations",
                nullable: false);
            migrationBuilder.AddColumn<int>(
                name: "GroupGroupId",
                schema: "ControlSystem",
                table: "GroupLocations",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "LocationLocationId",
                schema: "ControlSystem",
                table: "GroupLocations",
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
                name: "FK_GroupLocation_Group_GroupGroupId",
                schema: "ControlSystem",
                table: "GroupLocations",
                column: "GroupGroupId",
                principalSchema: "ControlSystem",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_GroupLocation_Location_LocationLocationId",
                schema: "ControlSystem",
                table: "GroupLocations",
                column: "LocationLocationId",
                principalSchema: "ControlSystem",
                principalTable: "DeviceLocations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_GroupOperator_Group_GroupGroupId",
                schema: "ControlSystem",
                table: "GroupOperators",
                column: "GroupGroupId",
                principalSchema: "ControlSystem",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_GroupOperator_Operator_OperatorOperatorId",
                schema: "ControlSystem",
                table: "GroupOperators",
                column: "OperatorOperatorId",
                principalSchema: "ControlSystem",
                principalTable: "Operators",
                principalColumn: "OperatorId",
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
                name: "FK_OperatorLocation_Location_LocationLocationId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                column: "LocationLocationId",
                principalSchema: "ControlSystem",
                principalTable: "DeviceLocations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocation_Operator_OperatorOperatorId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                column: "OperatorOperatorId",
                principalSchema: "ControlSystem",
                principalTable: "Operators",
                principalColumn: "OperatorId",
                onDelete: ReferentialAction.Restrict);
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
            migrationBuilder.DropForeignKey(name: "FK_GroupLocation_Group_GroupGroupId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropForeignKey(name: "FK_GroupLocation_Location_LocationLocationId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropForeignKey(name: "FK_GroupOperator_Group_GroupGroupId", schema: "ControlSystem", table: "GroupOperators");
            migrationBuilder.DropForeignKey(name: "FK_GroupOperator_Operator_OperatorOperatorId", schema: "ControlSystem", table: "GroupOperators");
            migrationBuilder.DropForeignKey(name: "FK_Holiday_Customer_CustomerId", schema: "ControlSystem", table: "Holidays");
            migrationBuilder.DropForeignKey(name: "FK_Location_Customer_CustomerId", schema: "ControlSystem", table: "DeviceLocations");
            migrationBuilder.DropForeignKey(name: "FK_Log_LogDescription_LogDescriptionId", schema: "ControlSystem", table: "Logs");
            migrationBuilder.DropForeignKey(name: "FK_Operator_Customer_CustomerId", schema: "ControlSystem", table: "Operators");
            migrationBuilder.DropForeignKey(name: "FK_OperatorLocation_Location_LocationLocationId", schema: "ControlSystem", table: "OperatorLocations");
            migrationBuilder.DropForeignKey(name: "FK_OperatorLocation_Operator_OperatorOperatorId", schema: "ControlSystem", table: "OperatorLocations");
            migrationBuilder.DropForeignKey(name: "FK_Scene_Customer_CustomerId", schema: "ControlSystem", table: "Scenes");
            migrationBuilder.DropForeignKey(name: "FK_SceneSegment_Scene_SceneId", schema: "ControlSystem", table: "SceneSegments");
            migrationBuilder.DropColumn(name: "LocationLocationId", schema: "ControlSystem", table: "OperatorLocations");
            migrationBuilder.DropColumn(name: "OperatorOperatorId", schema: "ControlSystem", table: "OperatorLocations");
            migrationBuilder.DropColumn(name: "GroupGroupId", schema: "ControlSystem", table: "GroupOperators");
            migrationBuilder.DropColumn(name: "OperatorOperatorId", schema: "ControlSystem", table: "GroupOperators");
            migrationBuilder.DropColumn(name: "GroupGroupId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.DropColumn(name: "LocationLocationId", schema: "ControlSystem", table: "GroupLocations");
            migrationBuilder.AlterColumn<int>(
                name: "OperatorId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "OperatorId",
                schema: "ControlSystem",
                table: "GroupOperators",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                schema: "ControlSystem",
                table: "GroupOperators",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "ControlSystem",
                table: "GroupLocations",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                schema: "ControlSystem",
                table: "GroupLocations",
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
                principalTable: "DeviceLocations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_GroupOperator_Group_GroupId",
                schema: "ControlSystem",
                table: "GroupOperators",
                column: "GroupId",
                principalSchema: "ControlSystem",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_GroupOperator_Operator_OperatorId",
                schema: "ControlSystem",
                table: "GroupOperators",
                column: "OperatorId",
                principalSchema: "ControlSystem",
                principalTable: "Operators",
                principalColumn: "OperatorId",
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
                name: "FK_OperatorLocation_Location_LocationId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                column: "LocationId",
                principalSchema: "ControlSystem",
                principalTable: "DeviceLocations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocation_Operator_OperatorId",
                schema: "ControlSystem",
                table: "OperatorLocations",
                column: "OperatorId",
                principalSchema: "ControlSystem",
                principalTable: "Operators",
                principalColumn: "OperatorId",
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
