using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class LikeEntityAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberLikes_Members_SourceMemberId",
                table: "MemberLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberLikes_Members_TargetMemberId",
                table: "MemberLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberLikes",
                table: "MemberLikes");

            migrationBuilder.RenameTable(
                name: "MemberLikes",
                newName: "Likes");

            migrationBuilder.RenameIndex(
                name: "IX_MemberLikes_TargetMemberId",
                table: "Likes",
                newName: "IX_Likes_TargetMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                columns: new[] { "SourceMemberId", "TargetMemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Members_SourceMemberId",
                table: "Likes",
                column: "SourceMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Members_TargetMemberId",
                table: "Likes",
                column: "TargetMemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Members_SourceMemberId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Members_TargetMemberId",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "MemberLikes");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_TargetMemberId",
                table: "MemberLikes",
                newName: "IX_MemberLikes_TargetMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberLikes",
                table: "MemberLikes",
                columns: new[] { "SourceMemberId", "TargetMemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MemberLikes_Members_SourceMemberId",
                table: "MemberLikes",
                column: "SourceMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberLikes_Members_TargetMemberId",
                table: "MemberLikes",
                column: "TargetMemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}
