using Microsoft.EntityFrameworkCore.Migrations;

namespace TORSHIA.Migrations
{
    public partial class DbSetAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffectedSector_Tasks_TaskId",
                table: "AffectedSector");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AffectedSector",
                table: "AffectedSector");

            migrationBuilder.RenameTable(
                name: "AffectedSector",
                newName: "AffectedSectors");

            migrationBuilder.RenameIndex(
                name: "IX_AffectedSector_TaskId",
                table: "AffectedSectors",
                newName: "IX_AffectedSectors_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AffectedSectors",
                table: "AffectedSectors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AffectedSectors_Tasks_TaskId",
                table: "AffectedSectors",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffectedSectors_Tasks_TaskId",
                table: "AffectedSectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AffectedSectors",
                table: "AffectedSectors");

            migrationBuilder.RenameTable(
                name: "AffectedSectors",
                newName: "AffectedSector");

            migrationBuilder.RenameIndex(
                name: "IX_AffectedSectors_TaskId",
                table: "AffectedSector",
                newName: "IX_AffectedSector_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AffectedSector",
                table: "AffectedSector",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AffectedSector_Tasks_TaskId",
                table: "AffectedSector",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
