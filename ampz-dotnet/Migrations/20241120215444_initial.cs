using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ampz_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_AMP_COMMUNITY",
                columns: table => new
                {
                    id_community = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ds_name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ds_description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMP_COMMUNITY", x => x.id_community);
                });

            migrationBuilder.CreateTable(
                name: "TB_AMZ_USER",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ds_name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ds_email = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ds_password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dt_birth = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    ds_city = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ds_state = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMZ_USER", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "TB_AMZ_KID",
                columns: table => new
                {
                    id_kid = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ds_name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dt_birth = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    total_score = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    total_energy_saved = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMZ_KID", x => x.id_kid);
                    table.ForeignKey(
                        name: "FK_TB_AMZ_KID_TB_AMZ_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_AMZ_USER",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_AMP_DEVICE",
                columns: table => new
                {
                    id_device = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ds_name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ds_type = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ds_operating_system = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    KidId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMP_DEVICE", x => x.id_device);
                    table.ForeignKey(
                        name: "FK_TB_AMP_DEVICE_TB_AMZ_KID_KidId",
                        column: x => x.KidId,
                        principalTable: "TB_AMZ_KID",
                        principalColumn: "id_kid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_AMP_DEVICE_KidId",
                table: "TB_AMP_DEVICE",
                column: "KidId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AMZ_KID_UserId",
                table: "TB_AMZ_KID",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AMZ_USER_ds_email",
                table: "TB_AMZ_USER",
                column: "ds_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AMP_COMMUNITY");

            migrationBuilder.DropTable(
                name: "TB_AMP_DEVICE");

            migrationBuilder.DropTable(
                name: "TB_AMZ_KID");

            migrationBuilder.DropTable(
                name: "TB_AMZ_USER");
        }
    }
}
