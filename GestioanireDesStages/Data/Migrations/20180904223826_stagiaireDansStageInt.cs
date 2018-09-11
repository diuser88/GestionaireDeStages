using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GestioanireDesStages.Data.Migrations
{
    public partial class stagiaireDansStageInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Stagiaire",
                table: "Stages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Stagiaire",
                table: "Stages",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
