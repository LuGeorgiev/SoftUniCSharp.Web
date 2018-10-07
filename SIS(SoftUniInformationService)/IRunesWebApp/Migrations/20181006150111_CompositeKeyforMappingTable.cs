using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRunesWebApp.Migrations
{
    public partial class CompositeKeyforMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackAblums_Albums_AlbumId",
                table: "TrackAblums");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackAblums_Tracks_TrackId",
                table: "TrackAblums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackAblums",
                table: "TrackAblums");

            migrationBuilder.DropIndex(
                name: "IX_TrackAblums_AlbumId",
                table: "TrackAblums");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TrackAblums");

            migrationBuilder.AlterColumn<string>(
                name: "TrackId",
                table: "TrackAblums",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlbumId",
                table: "TrackAblums",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackAblums",
                table: "TrackAblums",
                columns: new[] { "AlbumId", "TrackId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TrackAblums_Albums_AlbumId",
                table: "TrackAblums",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackAblums_Tracks_TrackId",
                table: "TrackAblums",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackAblums_Albums_AlbumId",
                table: "TrackAblums");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackAblums_Tracks_TrackId",
                table: "TrackAblums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackAblums",
                table: "TrackAblums");

            migrationBuilder.AlterColumn<string>(
                name: "TrackId",
                table: "TrackAblums",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AlbumId",
                table: "TrackAblums",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TrackAblums",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackAblums",
                table: "TrackAblums",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackAblums_AlbumId",
                table: "TrackAblums",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackAblums_Albums_AlbumId",
                table: "TrackAblums",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackAblums_Tracks_TrackId",
                table: "TrackAblums",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
