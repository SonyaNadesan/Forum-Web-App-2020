using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Data.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Threads_ThreadId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadCategory_ThreadCategory_ThreadCategoryId",
                table: "ThreadCategory");

            migrationBuilder.DropIndex(
                name: "IX_ThreadCategory_ThreadCategoryId",
                table: "ThreadCategory");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ThreadId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ThreadCategoryId",
                table: "ThreadCategory");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "Categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ThreadCategoryId",
                table: "ThreadCategory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ThreadId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThreadCategory_ThreadCategoryId",
                table: "ThreadCategory",
                column: "ThreadCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ThreadId",
                table: "Categories",
                column: "ThreadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Threads_ThreadId",
                table: "Categories",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadCategory_ThreadCategory_ThreadCategoryId",
                table: "ThreadCategory",
                column: "ThreadCategoryId",
                principalTable: "ThreadCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
