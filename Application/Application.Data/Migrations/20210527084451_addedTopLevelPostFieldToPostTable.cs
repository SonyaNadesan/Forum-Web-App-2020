using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Data.Migrations
{
    public partial class addedTopLevelPostFieldToPostTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TopLevelPostId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TopLevelPostId",
                table: "Posts",
                column: "TopLevelPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_TopLevelPostId",
                table: "Posts",
                column: "TopLevelPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_TopLevelPostId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TopLevelPostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TopLevelPostId",
                table: "Posts");
        }
    }
}
