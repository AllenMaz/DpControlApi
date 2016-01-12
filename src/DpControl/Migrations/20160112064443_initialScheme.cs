using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace DpControl.Migrations
{
    public partial class initialScheme : Migration
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
                    ErrorNo = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmMessage", x => x.AlarmMessageId);
                });
            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "ControlSystem",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerName = table.Column<string>(nullable: false),
                    CustomerNo = table.Column<string>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ProjectName = table.Column<string>(nullable: false),
                    ProjectNo = table.Column<string>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });
            migrationBuilder.CreateTable(
                name: "LogDescription",
                schema: "ControlSystem",
                columns: table => new
                {
                    LogDescriptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    DescriptionNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDescription", x => x.LogDescriptionId);
                });
            migrationBuilder.CreateTable(
                name: "Holidays",
                schema: "ControlSystem",
                columns: table => new
                {
                    HolidayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.HolidayId);
                    table.ForeignKey(
                        name: "FK_Holiday_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "ControlSystem",
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "DeviceLocations",
                schema: "ControlSystem",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Building = table.Column<string>(nullable: false),
                    CommAddress = table.Column<string>(nullable: true),
                    CommMode = table.Column<int>(nullable: false),
                    CurrentPosition = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    DeviceSerialNo = table.Column<string>(nullable: true),
                    DeviceType = table.Column<int>(nullable: false),
                    FavorPositionFirst = table.Column<int>(nullable: false),
                    FavorPositionThird = table.Column<int>(nullable: false),
                    FavorPositionrSecond = table.Column<int>(nullable: false),
                    Floor = table.Column<string>(nullable: true),
                    InstallationNumber = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Orientation = table.Column<int>(nullable: false),
                    RoomNo = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Location_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "ControlSystem",
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Operators",
                schema: "ControlSystem",
                columns: table => new
                {
                    OperatorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    NickName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operator", x => x.OperatorId);
                    table.ForeignKey(
                        name: "FK_Operator_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "ControlSystem",
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Scenes",
                schema: "ControlSystem",
                columns: table => new
                {
                    SceneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    Enable = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scene", x => x.SceneId);
                    table.ForeignKey(
                        name: "FK_Scene_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "ControlSystem",
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Alarms",
                schema: "ControlSystem",
                columns: table => new
                {
                    AlarmId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlarmMessageId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alarm_Location_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "ControlSystem",
                        principalTable: "DeviceLocations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "ControlSystem",
                columns: table => new
                {
                    LogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    LocationId = table.Column<int>(nullable: false),
                    LogDescriptionId = table.Column<int>(nullable: false),
                    LogOfLocationId = table.Column<int>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    OperatorOperatorId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Log_LogDescription_LogDescriptionId",
                        column: x => x.LogDescriptionId,
                        principalSchema: "ControlSystem",
                        principalTable: "LogDescription",
                        principalColumn: "LogDescriptionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Log_Location_LogOfLocationId",
                        column: x => x.LogOfLocationId,
                        principalSchema: "ControlSystem",
                        principalTable: "DeviceLocations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Log_Operator_OperatorOperatorId",
                        column: x => x.OperatorOperatorId,
                        principalSchema: "ControlSystem",
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "OperatorLocations",
                schema: "ControlSystem",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    LocationLocationId = table.Column<int>(nullable: true),
                    OperatorOperatorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorLocation", x => new { x.LocationId, x.OperatorId });
                    table.ForeignKey(
                        name: "FK_OperatorLocation_Location_LocationLocationId",
                        column: x => x.LocationLocationId,
                        principalSchema: "ControlSystem",
                        principalTable: "DeviceLocations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperatorLocation_Operator_OperatorOperatorId",
                        column: x => x.OperatorOperatorId,
                        principalSchema: "ControlSystem",
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "ControlSystem",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    GroupName = table.Column<string>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true),
                    SceneId = table.Column<int>(nullable: false),
                    SceneSceneId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Group_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "ControlSystem",
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Group_Scene_SceneSceneId",
                        column: x => x.SceneSceneId,
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
                    ModifiedDate = table.Column<DateTime>(nullable: false),
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
                    GroupId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    GroupGroupId = table.Column<int>(nullable: true),
                    LocationLocationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupLocation", x => new { x.GroupId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_GroupLocation_Group_GroupGroupId",
                        column: x => x.GroupGroupId,
                        principalSchema: "ControlSystem",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupLocation_Location_LocationLocationId",
                        column: x => x.LocationLocationId,
                        principalSchema: "ControlSystem",
                        principalTable: "DeviceLocations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "GroupOperators",
                schema: "ControlSystem",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    GroupGroupId = table.Column<int>(nullable: true),
                    OperatorOperatorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOperator", x => new { x.GroupId, x.OperatorId });
                    table.ForeignKey(
                        name: "FK_GroupOperator_Group_GroupGroupId",
                        column: x => x.GroupGroupId,
                        principalSchema: "ControlSystem",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupOperator_Operator_OperatorOperatorId",
                        column: x => x.OperatorOperatorId,
                        principalSchema: "ControlSystem",
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Alarms", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "GroupLocations", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "GroupOperators", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Holidays", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Logs", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "OperatorLocations", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "SceneSegments", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "AlarmMessages", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Groups", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "LogDescription", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "DeviceLocations", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Operators", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Scenes", schema: "ControlSystem");
            migrationBuilder.DropTable(name: "Customers", schema: "ControlSystem");
        }
    }
}
