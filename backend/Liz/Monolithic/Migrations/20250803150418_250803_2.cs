using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monolithic.Migrations
{
    /// <inheritdoc />
    public partial class _250803_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "RoomParticipants",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateIndex(
                name: "IX_RoomParticipants_RoomId_DisplayName_IsActive",
                table: "RoomParticipants",
                columns: new[] { "RoomId", "DisplayName", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = true"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_RoomParticipants_RoomId_DisplayName_IsActive", table: "RoomParticipants");

            migrationBuilder.DropColumn(name: "DisplayName", table: "RoomParticipants");
        }
    }
}
