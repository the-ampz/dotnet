using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ampz_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class initiall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_energy_saved",
                table: "TB_AMZ_KID",
                type: "DECIMAL(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)");

            migrationBuilder.CreateTable(
                name: "CommunityKid",
                columns: table => new
                {
                    CommunitiesId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    KidsId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityKid", x => new { x.CommunitiesId, x.KidsId });
                    table.ForeignKey(
                        name: "FK_CommunityKid_TB_AMP_COMMUNITY_CommunitiesId",
                        column: x => x.CommunitiesId,
                        principalTable: "TB_AMP_COMMUNITY",
                        principalColumn: "id_community",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityKid_TB_AMZ_KID_KidsId",
                        column: x => x.KidsId,
                        principalTable: "TB_AMZ_KID",
                        principalColumn: "id_kid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityKid_KidsId",
                table: "CommunityKid",
                column: "KidsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityKid");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_energy_saved",
                table: "TB_AMZ_KID",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18, 2)");
        }
    }
}
