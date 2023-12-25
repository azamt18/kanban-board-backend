using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanbanBoard.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    createdon = table.Column<DateTime>(name: "created_on", type: "TEXT", nullable: false),
                    updatedon = table.Column<DateTime>(name: "updated_on", type: "TEXT", nullable: false),
                    isclosed = table.Column<bool>(name: "is_closed", type: "INTEGER", nullable: false),
                    closedon = table.Column<DateTime>(name: "closed_on", type: "TEXT", nullable: true),
                    title = table.Column<string>(type: "text", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    createdon = table.Column<DateTime>(name: "created_on", type: "TEXT", nullable: false),
                    updatedon = table.Column<DateTime>(name: "updated_on", type: "TEXT", nullable: false),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "INTEGER", nullable: false),
                    deletedon = table.Column<DateTime>(name: "deleted_on", type: "TEXT", nullable: true),
                    title = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    priority = table.Column<int>(type: "INTEGER", nullable: false),
                    activelistid = table.Column<int>(name: "active_list_id", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_cards_lists_active_list_id",
                        column: x => x.activelistid,
                        principalTable: "lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "card_histories",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    cardid = table.Column<int>(name: "card_id", type: "INTEGER", nullable: false),
                    createdon = table.Column<DateTime>(name: "created_on", type: "TEXT", nullable: false),
                    type = table.Column<int>(type: "INTEGER", nullable: false),
                    movedsourcelistid = table.Column<int>(name: "moved_source_list_id", type: "INTEGER", nullable: true),
                    movedtargetlistid = table.Column<int>(name: "moved_target_list_id", type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_card_histories_cards_card_id",
                        column: x => x.cardid,
                        principalTable: "cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_histories_card_id",
                table: "card_histories",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_cards_active_list_id",
                table: "cards",
                column: "active_list_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_histories");

            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "lists");
        }
    }
}
