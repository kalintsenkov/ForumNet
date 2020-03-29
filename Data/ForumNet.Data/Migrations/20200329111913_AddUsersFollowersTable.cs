namespace ForumNet.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddUsersFollowersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersFollowers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    FollowerId = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersFollowers", x => new { x.UserId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_UsersFollowers_AspNetUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersFollowers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplyReports_IsDeleted",
                table: "ReplyReports",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PostReports_IsDeleted",
                table: "PostReports",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UsersFollowers_FollowerId",
                table: "UsersFollowers",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersFollowers_IsDeleted",
                table: "UsersFollowers",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersFollowers");

            migrationBuilder.DropIndex(
                name: "IX_ReplyReports_IsDeleted",
                table: "ReplyReports");

            migrationBuilder.DropIndex(
                name: "IX_PostReports_IsDeleted",
                table: "PostReports");
        }
    }
}
