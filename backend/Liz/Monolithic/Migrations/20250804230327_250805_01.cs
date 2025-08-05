using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monolithic.Migrations
{
    /// <inheritdoc />
    public partial class _250805_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastActiveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "DeviceFingerprints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BrowserFingerprint = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DeviceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeviceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OperatingSystem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Browser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BrowserVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Platform = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastActiveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    FirstSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsTrusted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceFingerprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceFingerprints_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MaxParticipants = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    LastActivityAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceFingerprintId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConnectionId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DisconnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_DeviceFingerprints_DeviceFingerprintId",
                        column: x => x.DeviceFingerprintId,
                        principalTable: "DeviceFingerprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull
                    );
                    table.ForeignKey(
                        name: "FK_Connections_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull
                    );
                    table.ForeignKey(
                        name: "FK_Connections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    MessageType = table.Column<int>(type: "integer", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "RoomParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DisplayName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomParticipants_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_RoomParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "IX_Connections_ConnectionId", table: "Connections", column: "ConnectionId", unique: true);

            migrationBuilder.CreateIndex(name: "IX_Connections_DeviceFingerprintId", table: "Connections", column: "DeviceFingerprintId");

            migrationBuilder.CreateIndex(name: "IX_Connections_IsActive", table: "Connections", column: "IsActive");

            migrationBuilder.CreateIndex(name: "IX_Connections_RoomId", table: "Connections", column: "RoomId");

            migrationBuilder.CreateIndex(name: "IX_Connections_UserId", table: "Connections", column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceFingerprints_BrowserFingerprint",
                table: "DeviceFingerprints",
                column: "BrowserFingerprint",
                unique: true
            );

            migrationBuilder.CreateIndex(name: "IX_DeviceFingerprints_IsActive", table: "DeviceFingerprints", column: "IsActive");

            migrationBuilder.CreateIndex(name: "IX_DeviceFingerprints_LastActiveAt", table: "DeviceFingerprints", column: "LastActiveAt");

            migrationBuilder.CreateIndex(name: "IX_DeviceFingerprints_UserId", table: "DeviceFingerprints", column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceFingerprints_UserId_IsActive",
                table: "DeviceFingerprints",
                columns: new[] { "UserId", "IsActive" }
            );

            migrationBuilder.CreateIndex(name: "IX_Messages_RoomId", table: "Messages", column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RoomId_SentAt",
                table: "Messages",
                columns: new[] { "RoomId", "SentAt" },
                descending: new bool[0]
            );

            migrationBuilder.CreateIndex(name: "IX_Messages_SenderId", table: "Messages", column: "SenderId");

            migrationBuilder.CreateIndex(name: "IX_Messages_SentAt", table: "Messages", column: "SentAt", descending: new bool[0]);

            migrationBuilder.CreateIndex(name: "IX_RoomParticipants_IsActive", table: "RoomParticipants", column: "IsActive");

            migrationBuilder.CreateIndex(name: "IX_RoomParticipants_RoomId", table: "RoomParticipants", column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomParticipants_RoomId_DisplayName_IsActive",
                table: "RoomParticipants",
                columns: new[] { "RoomId", "DisplayName", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = true"
            );

            migrationBuilder.CreateIndex(
                name: "IX_RoomParticipants_RoomId_UserId_IsActive",
                table: "RoomParticipants",
                columns: new[] { "RoomId", "UserId", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = true"
            );

            migrationBuilder.CreateIndex(name: "IX_RoomParticipants_UserId", table: "RoomParticipants", column: "UserId");

            migrationBuilder.CreateIndex(name: "IX_Rooms_CreatedById", table: "Rooms", column: "CreatedById");

            migrationBuilder.CreateIndex(name: "IX_Rooms_IsActive", table: "Rooms", column: "IsActive");

            migrationBuilder.CreateIndex(name: "IX_Rooms_LastActivityAt", table: "Rooms", column: "LastActivityAt");

            migrationBuilder.CreateIndex(name: "IX_Users_IsOnline", table: "Users", column: "IsOnline");

            migrationBuilder.CreateIndex(name: "IX_Users_LastActiveAt", table: "Users", column: "LastActiveAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Connections");

            migrationBuilder.DropTable(name: "Messages");

            migrationBuilder.DropTable(name: "RoomParticipants");

            migrationBuilder.DropTable(name: "DeviceFingerprints");

            migrationBuilder.DropTable(name: "Rooms");

            migrationBuilder.DropTable(name: "Users");
        }
    }
}
