using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jim.wiki.back.infrastructure.Repository.Migrations
{
    /// <inheritdoc />
    public partial class useralternativekey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Guid",
                table: "Users",
                column: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Guid",
                table: "Users");
        }
    }
}
