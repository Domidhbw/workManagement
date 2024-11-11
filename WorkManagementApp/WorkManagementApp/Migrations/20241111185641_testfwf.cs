﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class testfwf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Tasks",
                newName: "TitleTest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TitleTest",
                table: "Tasks",
                newName: "Title");
        }
    }
}
