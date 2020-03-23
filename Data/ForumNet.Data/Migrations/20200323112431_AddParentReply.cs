namespace ForumNet.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddParentReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Replies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ParentId",
                table: "Replies",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Replies_ParentId",
                table: "Replies",
                column: "ParentId",
                principalTable: "Replies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_ParentId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_ParentId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Replies");
        }
    }
}
