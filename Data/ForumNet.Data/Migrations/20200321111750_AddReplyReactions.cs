namespace ForumNet.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddReplyReactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReaction_AspNetUsers_AuthorId",
                table: "PostReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReaction_Posts_PostId",
                table: "PostReaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReaction",
                table: "PostReaction");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Replies");

            migrationBuilder.RenameTable(
                name: "PostReaction",
                newName: "PostReactions");

            migrationBuilder.RenameIndex(
                name: "IX_PostReaction_PostId",
                table: "PostReactions",
                newName: "IX_PostReactions_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostReaction_AuthorId",
                table: "PostReactions",
                newName: "IX_PostReactions_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReactions",
                table: "PostReactions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ReplyReactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReactionType = table.Column<int>(nullable: false),
                    ReplyId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplyReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReplyReactions_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReplyReactions_Replies_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Replies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplyReactions_AuthorId",
                table: "ReplyReactions",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyReactions_ReplyId",
                table: "ReplyReactions",
                column: "ReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_AspNetUsers_AuthorId",
                table: "PostReactions",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_Posts_PostId",
                table: "PostReactions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_AspNetUsers_AuthorId",
                table: "PostReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_Posts_PostId",
                table: "PostReactions");

            migrationBuilder.DropTable(
                name: "ReplyReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReactions",
                table: "PostReactions");

            migrationBuilder.RenameTable(
                name: "PostReactions",
                newName: "PostReaction");

            migrationBuilder.RenameIndex(
                name: "IX_PostReactions_PostId",
                table: "PostReaction",
                newName: "IX_PostReaction_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostReactions_AuthorId",
                table: "PostReaction",
                newName: "IX_PostReaction_AuthorId");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Replies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReaction",
                table: "PostReaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReaction_AspNetUsers_AuthorId",
                table: "PostReaction",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReaction_Posts_PostId",
                table: "PostReaction",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
