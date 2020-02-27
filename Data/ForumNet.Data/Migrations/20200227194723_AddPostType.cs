namespace ForumNet.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddPostType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                maxLength: 130,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "ImageOrVideoUrl",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageOrVideoUrl",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 130);
        }
    }
}
