using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace jim.wiki.back.infrastructure.Repository.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastAction = table.Column<string>(type: "text", nullable: false),
                    CreateadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.UniqueConstraint("AK_Roles_Guid", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    RolId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastAction = table.Column<string>(type: "text", nullable: false),
                    CreateadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Guid", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Id",
                        column: x => x.Id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ValidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateadAt", "CreatedBy", "Description", "Guid", "IsDeleted", "LastAction", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 7, 21, 8, 57, 38, 908, DateTimeKind.Utc).AddTicks(4428), "_System", "Admin", new Guid("abc10fde-95db-4731-9e14-ee2e0fa7d1aa"), false, "Admin", new DateTime(2024, 7, 21, 8, 57, 38, 908, DateTimeKind.Utc).AddTicks(4457), "_System", "Admin" },
                    { 2L, new DateTime(2024, 7, 21, 8, 57, 38, 908, DateTimeKind.Utc).AddTicks(4509), "_System", "User", new Guid("a94d144e-297e-45ff-bd4b-1e81c7470a0b"), false, "User", new DateTime(2024, 7, 21, 8, 57, 38, 908, DateTimeKind.Utc).AddTicks(4510), "_System", "User" },
                    { 3L, new DateTime(2024, 7, 21, 8, 57, 38, 908, DateTimeKind.Utc).AddTicks(4513), "_System", "Guest", new Guid("3623ea86-3b21-4527-949b-8b59bb7f93fe"), false, "Guest", new DateTime(2024, 7, 21, 8, 57, 38, 908, DateTimeKind.Utc).AddTicks(4514), "_System", "Guest" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreateadAt", "CreatedBy", "Email", "Hash", "IsDeleted", "LastAction", "ModifiedAt", "ModifiedBy", "Name", "RolId" },
                values: new object[] { 1L, new DateTime(2024, 7, 21, 8, 57, 39, 29, DateTimeKind.Utc).AddTicks(1082), "_Systenm", "email@mail.com", "$2a$11$jkBKuOyTqRNTIrFuUw2Cm.8vBcgTk4QvQi7TiFMHTkz39uBvZ530S", false, "Added", new DateTime(2024, 7, 21, 8, 57, 39, 29, DateTimeKind.Utc).AddTicks(1086), "_System", "Admin", 1L });

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
