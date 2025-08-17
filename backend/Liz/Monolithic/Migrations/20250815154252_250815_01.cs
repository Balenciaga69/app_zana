using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monolithic.Migrations;

/// <inheritdoc />
public partial class _250815_01 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                SenderId = table.Column<string>(type: "text", nullable: false),
                Content = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "RoomParticipants",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<string>(type: "text", nullable: false),
                JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                LeftAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoomParticipants", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Rooms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(
                    type: "character varying(100)",
                    maxLength: 100,
                    nullable: false
                ),
                OwnerId = table.Column<string>(type: "text", nullable: false),
                PasswordHash = table.Column<string>(
                    type: "character varying(256)",
                    maxLength: 256,
                    nullable: true
                ),
                MaxParticipants = table.Column<int>(type: "integer", nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                LastActiveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                InviteCode = table.Column<string>(
                    type: "character varying(32)",
                    maxLength: 32,
                    nullable: false
                ),
                DestroyedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rooms", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "UserConnections",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<string>(type: "text", nullable: false),
                ConnectionId = table.Column<string>(
                    type: "character varying(128)",
                    maxLength: 128,
                    nullable: false
                ),
                ConnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                DisconnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                IpAddress = table.Column<string>(
                    type: "character varying(45)",
                    maxLength: 45,
                    nullable: true
                ),
                UserAgent = table.Column<string>(
                    type: "character varying(500)",
                    maxLength: 500,
                    nullable: true
                ),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserConnections", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DeviceFingerprint = table.Column<string>(
                    type: "character varying(128)",
                    maxLength: 128,
                    nullable: false
                ),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                LastActiveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Nickname = table.Column<string>(
                    type: "character varying(32)",
                    maxLength: 32,
                    nullable: false
                ),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            }
        );

        migrationBuilder.CreateIndex(name: "IX_Messages_RoomId", table: "Messages", column: "RoomId");

        migrationBuilder.CreateIndex(name: "IX_Messages_SenderId", table: "Messages", column: "SenderId");

        migrationBuilder.CreateIndex(
            name: "IX_RoomParticipants_RoomId_UserId",
            table: "RoomParticipants",
            columns: new[] { "RoomId", "UserId" }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_InviteCode",
            table: "Rooms",
            column: "InviteCode",
            unique: true
        );

        migrationBuilder.CreateIndex(name: "IX_Rooms_IsActive", table: "Rooms", column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_UserConnections_ConnectionId",
            table: "UserConnections",
            column: "ConnectionId",
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_UserConnections_DisconnectedAt",
            table: "UserConnections",
            column: "DisconnectedAt"
        );

        migrationBuilder.CreateIndex(
            name: "IX_UserConnections_UserId",
            table: "UserConnections",
            column: "UserId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_Users_DeviceFingerprint",
            table: "Users",
            column: "DeviceFingerprint",
            unique: true
        );

        migrationBuilder.CreateIndex(name: "IX_Users_IsActive", table: "Users", column: "IsActive");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Messages");

        migrationBuilder.DropTable(name: "RoomParticipants");

        migrationBuilder.DropTable(name: "Rooms");

        migrationBuilder.DropTable(name: "UserConnections");

        migrationBuilder.DropTable(name: "Users");
    }
}
